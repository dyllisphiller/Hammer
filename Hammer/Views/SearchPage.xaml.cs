using Hammer.Callsigns;
using Hammer.Core.Licenses;
using Hammer.Core.Helpers;
using Hammer.Helpers.Cartography;
using Newtonsoft.Json.Linq;
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
        public LicenseViewModel ViewModel { get; set; }

        public SearchPage()
        {
            this.InitializeComponent();
            if (this.ViewModel == null)
            {
                this.ViewModel = new LicenseViewModel();
            }
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is string && !string.IsNullOrWhiteSpace((string)e.Parameter))
            {
                try
                {
                    await CallsignSearch((string)e.Parameter).ConfigureAwait(true);
                }
                catch (Exception ex)
                {
                    var messageDialog = new MessageDialog(ex.Message);
                    await messageDialog.ShowAsync();
                }
            }
        }

        /// <summary>
        /// The base address for the callsign lookup endpoint with a trailing slash.
        /// </summary>
        // callook.info is pretty awesome with their API terms
        // If anyone there is reading this: thanks!
        const string EndpointURL = "https://callook.info/";

        private License licenseSearchResult = new License();
        Geopoint licenseGeopoint;

        public async Task RetrieveData(string callsign)
        {
            string CallsignClean;

            // Clean the callsign
            if (!String.IsNullOrEmpty(callsign))
            {
                CallsignClean = Regex.Replace(callsign.ToUpperInvariant(), @"[^\p{Lu}\p{Nd}]", "");
            }
            else
            {
                throw new ArgumentNullException(nameof(callsign), "Callsign must not be null or empty.");
            }

            // Initialize variables
            JObject jResult;
            string result;

            string ErrorMessage = "";

            // TODO: Reset result fields
            // LookupResultField.Text = "";

            // assemble the address
            Uri.TryCreate($"{EndpointURL}{CallsignClean}/json", UriKind.Absolute, out Uri requestUri);

            using (var client = new HttpClient())
            {
                try
                {
                    // Make it clear we really do want JSON.
                    // This might be omitted; API requests ending in /json always return JSON
                    client.DefaultRequestHeaders.Accept.TryParseAdd("application/json");
                    HttpResponseMessage httpResponse = await client.GetAsync(requestUri);
                    httpResponse.EnsureSuccessStatusCode();
                    result = await httpResponse.Content.ReadAsStringAsync();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    throw;
                }
            }

            try
            {
                jResult = JObject.Parse(result);
                License.TryParse(jResult, out licenseSearchResult);

                switch (licenseSearchResult.Status)
                {
                    case "VALID":
                        // Display the results in their fields
                        SearchResultsHeader.Text = CallsignClean;
                        SearchResultsSubheader.Text = $"{licenseSearchResult.Name}";

                        if (String.IsNullOrEmpty(licenseSearchResult.Trustee.Callsign))
                        SearchTrusteeButton.Content = $"{licenseSearchResult.Trustee.Callsign}";

                        AddressAttnField.Text = 
                            CultureInfo.InvariantCulture.TextInfo.ToTitleCase(
                                licenseSearchResult.AddressAttn.ToLower(CultureInfo.CurrentCulture)
                            );
                        AddressLine1Field.Text =
                            CultureInfo.InvariantCulture.TextInfo.ToTitleCase(
                                licenseSearchResult.AddressLine1.ToLower(CultureInfo.CurrentCulture)
                            );
                        AddressLine2Field.Text =
                            CultureInfo.InvariantCulture.TextInfo.ToTitleCase(
                                licenseSearchResult.AddressLine2.ToLower(CultureInfo.CurrentCulture)
                            );

                        LocationLatitudeField.Text = licenseSearchResult.Location.Latitude.ToString();
                        LocationLongitudeField.Text = licenseSearchResult.Location.Longitude.ToString();
                        GridSquareField.Text = licenseSearchResult.GridSquare;

                        DateGrantedField.Text = licenseSearchResult.GrantDate.ToString("d");
                        DateExpiryField.Text = licenseSearchResult.ExpiryDate.ToString("d");
                        DateLastActionField.Text = licenseSearchResult.LastActionDate.ToString("d");

                        UlsUriHyperlinkButton.NavigateUri = licenseSearchResult.UlsUri;

                        //UlsUriField.Text = licenseSearchResult.UlsUri.ToString();

                        licenseGeopoint = new Geopoint(new BasicGeoposition { Latitude = licenseSearchResult.Location.Latitude, Longitude = licenseSearchResult.Location.Longitude });

                        break;

                    case "UPDATING":
                        throw new Exception(message: "The data source is getting the latest license data from the FCC. This might take a bit. Please try again later.");

                    default:
                        throw new Exception(message: "Either the callsign is invalid or something unexpected happened. Try again?");
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"{ex.Message}";
            }

            if (!String.IsNullOrEmpty(ErrorMessage))
            {
                System.Diagnostics.Debug.WriteLine(ErrorMessage);
            }
        }

        private async Task CallsignSearch(string callsign)
        {
            //Contract.Requires<ArgumentNullException>(callsign != null, "callsign");
            Regex _rgx = new Regex(@"[0-z]{3,12}");

            if (String.IsNullOrEmpty(callsign))
            {
                throw new ApplicationException("Callsign field must not be empty.");
            }
            else if (!_rgx.IsMatch(callsign))
            {
                string exception = $"'{callsign}' does not appear to be a valid callsign.";
                throw new ApplicationException(exception);
            }
            else if (Parser.IsCallsignIssuedByUnitedStates(callsign))
            {
                // Spinny blade wall! Actually it's just a spinner.
                // Can we call it a fidget spinner? Can computers fidget?
                // This needs to be a thing for generalized AI. Occasional fidgeting.
                // Imagine it pinging a random host for funsies. Toggling the power light.
                // Or, if the machine is old, opening and closing the optical drive tray.
                // TODO: Trigger these with events instead of synchronously?
                SearchResultsStackPanel.Visibility = Visibility.Collapsed;
                SearchProgressRing.IsActive = true;
                SearchProgressRing.Visibility = Visibility.Visible;

                await RetrieveData(callsign).ConfigureAwait(true);

                // All done. Nothing to see here.
                SearchProgressRing.Visibility = Visibility.Collapsed;
                SearchProgressRing.IsActive = false;
                SearchResultsStackPanel.Visibility = Visibility.Visible;
            }
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
            var options = new Windows.System.LauncherOptions();
            options.TreatAsUntrusted = true;

            await Launcher.LaunchUriAsync(licenseSearchResult.UlsUri, options);
            //await Launcher.LaunchUriAsync(licenseSearchResult.UlsUri);
        }

        private async void SearchTrusteeButton_Click(object sender, RoutedEventArgs e)
        {
            // Null reference exception.
            //await CallsignSearch(licenseSearchResult.Trustee.Callsign).ConfigureAwait(true);
        }
    }
}
