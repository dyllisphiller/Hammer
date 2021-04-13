using System;
using System.Globalization;
using System.Threading.Tasks;
using Hammer.Core.Helpers;
using Hammer.Core.Models;
using Hammer.Helpers.Maps;
using MUXC = Microsoft.UI.Xaml.Controls;
using Windows.Devices.Geolocation;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

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
            // this means the page was navigated to without a search parameter, so it should show a search box instead
            if (string.IsNullOrWhiteSpace((string)e.Parameter))
            {
                SearchPageDefaultHeader.Visibility = Visibility.Visible;
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

            if (string.IsNullOrEmpty(callsign))
            {
                dialog = new ContentDialog()
                {
                    Title = "A void is not a valid callsign",
                    Content = "Hammer would try to find that callsign, but the void remains unlicensed. (The search field cannot be empty.)",
                };
                await dialog.ShowAsync();
                string ex = "Search field must not be empty.";
                throw new ApplicationException(ex);
            }

            // Make sure it's a US callsign.
            //Prefixes.TryGetRegion(callsign, out string region);

            //if (region != "us")
            //{
            //    dialog = new ContentDialog() {
            //        Title = $"{callsign} is not a US callsign",
            //        Content = $"{callsign} was not issued by the FCC, so Hammer can't look it up its license. Hammer doesn't yet support non-US callsigns.",
            //    };
            //    await dialog.ShowAsync();
            //    string ex = "Can't look up non-FCC callsigns.";
            //    throw new ApplicationException(ex);
            //}

            // Get the API endpoint URI for the callsign
            //APIs.TryMakeUri(region, callsign, out Uri uri);


            // Parse JObject from API payload
            //JObject jResult;
            //jResult = await APIs.GetLicenseJObjectAsync(uri);

            // Parse new License from JObject
            //License.TryParse(jResult, out licenseSearchResult);

            License licenseSearchResult = await Parsers.GetLicenseFromJsonAsync(callsign);

            if (licenseSearchResult.Status == LicenseStatus.Updating)
            {
                dialog.Title = "Updating license data";
                dialog.Content = "The data source is updating their license data from the FCC. This might take a bit. Please try again later.";
                await dialog.ShowAsync();
            }

            switch (ViewModel.License.Status)
            {
                case LicenseStatus.EDEFAULTVIEWMODEL:
                    {
                        break;
                    }
                case LicenseStatus.ESIGNNOTUS:
                    {
                        ContentDialog dialog = new ContentDialog()
                        {
                            Title = "Hammer can only look up US callsigns",
                            CloseButtonText = "OK",
                        };
                        await dialog.ShowAsync();
                        break;
                    }

                case LicenseStatus.Updating:
                    {
                        ContentDialog dialog = new ContentDialog()
                        {
                            Title = "Updating license data",
                            Content = "The server is fetching new license data from the FCC. This might take a bit; try again later.",
                            CloseButtonText = "OK",
                        };
                        await dialog.ShowAsync();
                        break;
                    }

                case LicenseStatus.Invalid:
                    {
                        ContentDialog dialog = new ContentDialog()
                        {
                            Title = "Either the callsign is invalid or something unexpected happened.",
                            Content = "Try again later. If the error persists, please consider filing a bug report.",
                            CloseButtonText = "OK",
                        };
                        await dialog.ShowAsync();
                        break;
                    }

                default:
                    {
                        SearchTrusteeButton.Visibility = ViewModel.License.Trustee != null ? Visibility.Visible : Visibility.Collapsed;

                        AddressAttnField.Visibility = ViewModel.License.AddressAttn == null ? Visibility.Collapsed : Visibility.Visible;

                        //BasicGeoposition mapPosition = new BasicGeoposition() { Latitude = licenseSearchResult.Location.Latitude, Longitude = licenseSearchResult.Location.Longitude };
                        //Geopoint mapPositionCenter = new Geopoint(mapPosition);
                        //LicenseLocationMapControl.Center = mapPositionCenter;
                        //LicenseLocationMapControl.ZoomLevel = 12;
                        //LicenseLocationMapControl.LandmarksVisible = true;

                        break;
                    }
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
                licenseSearchResult.Callsign.ToString(),
                licenseSearchResult.AddressLine1,
                licenseSearchResult.AddressLine2);
        }

        private async void UlsUriButton_Click(object sender, RoutedEventArgs e)
        {
            LauncherOptions options = new LauncherOptions
            {
                TreatAsUntrusted = true
            };

            await Launcher.LaunchUriAsync(licenseSearchResult.UlsUri, options);
        }

        private async void SearchTrusteeButton_Click(object sender, RoutedEventArgs e)
        {
            await CallsignSearch((string)licenseSearchResult.Trustee);
        }
    }
}
