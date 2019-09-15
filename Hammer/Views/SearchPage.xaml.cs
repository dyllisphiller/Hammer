using Hammer.Models;
using Hammer.Callsigns;
using Hammer.Callsigns.Search;
using Newtonsoft.Json.Linq;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.Web.Http;
using Windows.Web.Http.Headers;
using Windows.UI.Xaml.Navigation;

namespace Hammer.Views
{
    /// <summary>
    /// The callsign SERP, meant to be navigated to within a Frame.
    /// </summary>
    public sealed partial class SearchPage : Page
    {
        public LicenseViewModel ViewModel { get; set; }

        public SearchPage()
        {
            this.InitializeComponent();
            this.ViewModel = new LicenseViewModel();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is string && !string.IsNullOrWhiteSpace((string)e.Parameter))
            {
                await CallsignSearch((string)e.Parameter).ConfigureAwait(true);
            }
        }

        /// <summary>
        /// The base address for the callsign lookup endpoint with a trailing slash.
        /// </summary>
        // callook.info is pretty awesome with their API terms
        // If anyone there is reading this: thanks!
        const string EndpointURL = "https://callook.info/";

        public async Task RetrieveData(string callsign)
        {
            // Initialize variables
            JObject jResult;
            string result;
            License License = new License();

            string ErrorMessage = "";
            string CallsignUpper = callsign.ToUpperInvariant();

            // TODO: Reset result fields
            // LookupResultField.Text = "";

            // assemble the address
            Uri requestUri = new Uri(EndpointURL + $"{CallsignUpper}/json");

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
                catch (Exception)
                {
                    throw;
                }
            }

            try
            {
                jResult = JObject.Parse(result);
                License.TryParse(jResult);
                

                switch (License.Status)
                {
                    case "VALID":
                        // Display the results in their fields
                        SearchHeaderField.Text = CallsignUpper;
                        RegistrantTypeField.Text = License.Type;
                        AddressAttnField.Text = License.AddressAttn;
                        AddressLine1Field.Text = License.AddressLine1;
                        AddressLine2Field.Text = License.AddressLine2;

                        LocationCoordinatesField.Text = License.Location.Coordinates;
                        GridSquareField.Text = License.GridSquare;

                        DateGrantedField.Text = License.GrantDate.ToString("d");
                        DateExpiryField.Text = License.ExpiryDate.ToString("d");
                        DateLastActionField.Text = License.LastActionDate.ToString("d");

                        UlsUriField.Text = License.UlsUri.ToString();
                        UlsUriButton.NavigateUri = License.UlsUri;

                        break;

                    case "UPDATING":
                        throw new Exception(message: "Callook is getting the latest license data from the FCC. This might take a bit. Please try again later.");

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
                string exception = $"'{callsign.ToUpperInvariant()}' does not appear to be a valid callsign.";
                throw new ApplicationException(exception);
            }
            else if (Parser.IsCallsignIssuedByUnitedStates(callsign))
            {
                // Spinny blade wall! Actually it's just a spinner.
                // Can we call it a fidget spinner? Can computers fidget?
                // This needs to be a thing for generalized AI. Occasional fidgeting.
                // Imagine it pinging a random host for funsies. Toggling the power light.
                // Or, if the machine is old, opening and closing the optical drive tray.
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

        private async void TemporarySearchButton_Click(object sender, RoutedEventArgs e)
        {
            await CallsignSearch(TemporarySearchTextBox.Text).ConfigureAwait(true);
        }
    }
}
