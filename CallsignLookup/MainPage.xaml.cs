using CallsignLookup;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Newtonsoft.Json.Linq;

// Project started with Microsoft's Blank Page template https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CallsignLookup
{
    /// <summary>
    /// The callsign lookup page.
    /// Can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        // The endpoint of the callsign lookup API
        // callook.info is pretty awesome with their API terms
        // If anyone from there is reading this: thanks!
        // MUST end with slash, as it's used as HttpClient.BaseAddress
        const string EndpointURL = "https://callook.info/";

        private async Task RetrieveData()
        {
            // Instantiate an HttpClient
            var client = new HttpClient();

            // Initialize variables
            JObject jResult = null;
            LicenseModel License = new LicenseModel(jResult);

            string Callsign = CallsignTextBox.Text.ToUpper();
            string CallsignLower = CallsignTextBox.Text.ToLower();
            string ResponseContent = null;

            // Reset result fields
            LookupResultTextBlock.Text = "";

            // Set the HttpClient instance's base address
            client.BaseAddress = new Uri(EndpointURL);
            // Construct the HTTP request parameters
            string URLParams = $"{CallsignLower}/json";

            // Make it clear we really do want JSON.
            // This might be omitted; API requests with /json return JSON
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = client.GetAsync(URLParams).Result;

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    ResponseContent = await response.Content.ReadAsStringAsync();
                    jResult = JObject.Parse(ResponseContent);

                    if ((string)jResult["status"] == "VALID")
                    {

                        // Assign variables from JSON values
                        License.Meta.Callsign = (string)jResult["callsign"];
                        License.Meta.Status = (string)jResult["status"];
                        License.Meta.FRN = (string)jResult["otherInfo"]["frn"];
                        License.Meta.UlsUrl = (Uri)jResult["otherInfo"]["ulsUrl"];

                        License.Address.Line1 = (string)jResult["address"]["line1"];
                        License.Address.Line2 = (string)jResult["address"]["line2"];
                        License.Address.Attn = (string)jResult["address"]["attn"];

                        License.Location.Latitude = (double)jResult["location"]["latitude"];
                        License.Location.Longitude = (double)jResult["location"]["longitude"];
                        License.Location.GridSquare = (string)jResult["location"]["gridsquare"];

                        // Wrangle and assign the dates
                        License.Dates.Grant = DateTimeOffset.Parse(
                            (string)jResult["otherInfo"]["grantDate"],
                            System.Globalization.CultureInfo.InvariantCulture);
                        License.Dates.Expiry = DateTimeOffset.Parse(
                            (string)jResult["otherInfo"]["expiryDate"],
                            System.Globalization.CultureInfo.InvariantCulture);
                        License.Dates.LastAction = DateTimeOffset.Parse(
                            (string)jResult["otherInfo"]["lastActionDate"],
                            System.Globalization.CultureInfo.InvariantCulture);

                        // Display the results in their fields
                        LookupResultTextBlock.Text = License.Meta.Status + " CALLSIGN";
                        CallsignHeaderTextBlock.Text = Callsign;
                        //CallsignHeaderTextBlock.Text = UlsUrl.ToString();
                    }
                    else if ((string)jResult["status"] == "UPDATING")
                    {
                        LookupResultTextBlock.Text = "License data is being gathered from the FCC by the callook.info API. This may take some time.";
                        //throw new ApplicationException();
                    }
                    else
                    {
                        LookupResultTextBlock.Text = "Either the callsign is invalid or something unexpected happened. Try again?";
                        //throw new ApplicationException();
                    }
                }
                    catch (Exception ex)
                {
                    LookupResultTextBlock.Text = $"{ex.Message}";
                }
            }
            else
            {
                LookupResultTextBlock.Text = $"{response.StatusCode.ToString()} {response.ReasonPhrase}";
            }
        }

        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            Regex _rgx = new Regex(@"[^A-Z0-9]");
            if (String.IsNullOrWhiteSpace(CallsignTextBox.Text))
            {
                throw new ArgumentNullException();
            }
            else if (_rgx.IsMatch(CallsignTextBox.Text))
            {
                string exception = String.Format("{0} is an invalid callsign.", CallsignTextBox.Text.ToUpper());
                throw new ApplicationException(exception);
            }
            else
            {
                ProgressRing.IsActive = true;
                ProgressRing.Visibility = Visibility.Visible;
                await RetrieveData();
                ProgressRing.Visibility = Visibility.Collapsed;
                ProgressRing.IsActive = false;
            }
        }

    }
}
