using Hammer.Callsigns;
using Hammer.Core.Callsigns;
using Hammer.Core.Helpers;
using Hammer.Core.Models;
using Hammer.Core.WebServices;
using Hammer.Helpers.Maps;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;

namespace Hammer.Views
{
    /// <summary>
    /// The license search result page, meant to be navigated to within a Frame.
    /// </summary>
    public sealed partial class SearchPage : Page
    {
        //public LicenseViewModel ViewModel { get; set; }

        public SearchPage()
        {
            this.InitializeComponent();
            //if (this.ViewModel == null) { this.ViewModel = new LicenseViewModel(); }
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is string p && !string.IsNullOrWhiteSpace(p))
            {
                try
                {
                    await CallsignSearch(p).ConfigureAwait(true);
                }
                catch (Exception ex)
                {
                    ContentDialog contentDialog = new ContentDialog()
                    {
                        Title = ex.Message,
                    };
                    await contentDialog.ShowAsync();
                }
            }
            if (string.IsNullOrWhiteSpace((string)e.Parameter))
            {
                //MessageDialog dialog = new MessageDialog("Hammer would try to find that callsign, but the void remains unlicensed.", "The void is not a valid callsign");
                //dialog.ShowAsync();
            }
        }

        License licenseSearchResult = new License();
        Geopoint licenseGeopoint;
        ContentDialog dialog;

        private async Task CallsignSearch(string callsign)
        {
            /* Spinny blade wall! Actually it's just a spinner.
             * Can we call it a fidget spinner? Can computers fidget?
             * This needs to be a thing for generalized AI. Occasional fidgeting.
             * Imagine it pinging a random host for funsies. Toggling the power light.
             * Or, if the machine is old, opening and closing the optical drive tray.
             */

            //TODO: Trigger these with events instead of synchronously?
            SearchResultsStackPanel.Visibility = Visibility.Collapsed;
            SearchProgressRing.IsActive = true;
            SearchProgressRing.Visibility = Visibility.Visible;

            // Clean the callsign
            callsign = Sanitizers.SanitizeCallsign(callsign);

            if (string.IsNullOrEmpty(callsign))
            {
                dialog = new ContentDialog()
                {
                    Title = "A void is not a valid callsign",
                    Content = "Hammer would try to find that callsign, but the void remains unlicensed. (The search field cannot be empty.)",
                };
                await dialog.ShowAsync();
                throw new ApplicationException(ex);
            }

            // Make sure it's a US callsign.
            Prefixes.TryGetRegion(callsign, out string region);

            if (region != "us")
            {
                dialog = new ContentDialog() {
                    Title = $"{callsign} is not a US callsign",
                    Content = $"{callsign} was not issued by the FCC, so Hammer can't look it up its license. Hammer doesn't yet support non-US callsigns.",
                };
                await dialog.ShowAsync();
                string ex = "Can't look up non-FCC callsigns.";
                throw new ApplicationException(ex);
            }

            // Get the API endpoint URI for the callsign
            APIs.TryMakeUri(region, callsign, out Uri uri);

            // Parse JObject from API payload
            JObject jResult;
            jResult = await APIs.GetLicenseJObjectAsync(uri);

            // Parse new License from JObject
            License.TryParse(jResult, out licenseSearchResult);

            switch (licenseSearchResult.Status)
            {
                case "VALID":
                    // Display the results in their fields
                    SearchResultsHeader.Text = callsign;
                    SearchResultsSubheader.Text = licenseSearchResult.Name;

                    SearchTrusteeButton.Visibility = Visibility.Collapsed;
                    
                    if (licenseSearchResult.Trustee != null)
                    {
                        string trusteeCallsign = licenseSearchResult.Trustee.Callsign;
                        SearchTrusteeButtonText.Text = $"Trustee {trusteeCallsign}";
                        SearchTrusteeButton.Visibility = Visibility.Visible;
                    }

                    AddressLine1Field.Text = licenseSearchResult.AddressLine1;
                    AddressLine2Field.Text = licenseSearchResult.AddressLine2;

                    if (licenseSearchResult.AddressAttn == null)
                    {
                        AddressAttnField.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        AddressAttnField.Text = licenseSearchResult.AddressAttn;
                        AddressAttnField.Visibility = Visibility.Visible;
                    }

                    LocationLatitudeField.Text = licenseSearchResult.Location.Latitude.ToString(CultureInfo.InvariantCulture);
                    LocationLongitudeField.Text = licenseSearchResult.Location.Longitude.ToString(CultureInfo.InvariantCulture);
                    GridSquareField.Text = licenseSearchResult.GridSquare;

                    DateGrantedField.Text = licenseSearchResult.GrantDate.ToString("d", CultureInfo.InvariantCulture);
                    DateExpiryField.Text = licenseSearchResult.ExpiryDate.ToString("d", CultureInfo.InvariantCulture);
                    DateLastActionField.Text = licenseSearchResult.LastActionDate.ToString("d", CultureInfo.InvariantCulture);

                    LicenseExternalUriButton.NavigateUri = licenseSearchResult.UlsUri;

                    //UlsUriField.Text = licenseSearchResult.UlsUri.ToString();

                    licenseGeopoint = new Geopoint(new BasicGeoposition { Latitude = licenseSearchResult.Location.Latitude, Longitude = licenseSearchResult.Location.Longitude });

                    break;

                case "UPDATING":
                    dialog = new ContentDialog()
                    {
                        Title = "Updating license data",
                        Content = "The data source is updating their license data from the FCC. This might take a bit. Please try again later.",
                    };
                    await dialog .ShowAsync();
                    break;

                default:
                    dialog = new ContentDialog()
                    {
                        Title = "Either the callsign is invalid or something unexpected happened. Try again?",
                    };
                    await dialog.ShowAsync();
                    break;
            }

            // All done. Nothing to see here.
            SearchProgressRing.Visibility = Visibility.Collapsed;
            SearchProgressRing.IsActive = false;
            SearchResultsStackPanel.Visibility = Visibility.Visible;
        }

        private void ShowMapButton_Click(object sender, RoutedEventArgs e)
        {
            Placecard.ShowPlacecard(sender,
                                    licenseGeopoint,
                                    licenseSearchResult.Callsign,
                                    licenseSearchResult.AddressLine1,
                                    licenseSearchResult.AddressLine2);
        }

        private async void UlsUriButton_Click(object sender, RoutedEventArgs e)
        {
            var options = new LauncherOptions
            {
                TreatAsUntrusted = true
            };

            await Launcher.LaunchUriAsync(licenseSearchResult.UlsUri, options);
        }

        private async void SearchTrusteeButton_Click(object sender, RoutedEventArgs e)
        {
            string trusteeCallsign = licenseSearchResult.Trustee.Callsign;
            await CallsignSearch(trusteeCallsign);
        }
    }
}
