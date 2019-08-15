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
            JObject jResult;
            string ResponseContent;
            LicenseModel License = new LicenseModel();

            string Callsign = CallsignSearchBox.Text.ToUpper();
            string CallsignLower = CallsignSearchBox.Text.ToLower();

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
                    License.TryParse(jResult);

                    if (License.Meta.Status == "VALID")
                    {
                        /*
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

                        */
                        // Display the results in their fields
                        CallsignHeaderTextBlock.Text = Callsign;
                        LookupResultTextBlock.Text = String.Format(
                            "{0} CALLSIGN: {1}",
                            License.Meta.Status,
                            License.Meta.Type);
                        AddressAttnTextBlock.Text = License.Address.Attn;
                        AddressLine1TextBlock.Text = License.Address.Line1;
                        AddressLine2TextBlock.Text = License.Address.Line2;
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

        /*private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            await CallsignSearch();
        }*/

        public async Task CallsignSearch()
        {
            // Spinny blade wall! Actually it's just a spinner.
            // Can we call it a fidget spinner? Can computers fidget?
            // This needs to be a thing for generalized AI. Occasional fidgeting.
            // Imagine it pinging a random host for funsies. Toggling the power light.
            // Or if the machine is old, opening and closing the optical drive tray.
            // ...but I digress...
            // Activate **the spinnerizer**!
            ProgressRing.IsActive = true;
            ProgressRing.Visibility = Visibility.Visible;

            Regex _rgx = new Regex(@"[a-zA-Z0-9]{3,12}");
            if (String.IsNullOrWhiteSpace(CallsignSearchBox.Text))
            {
                throw new ArgumentNullException();
            }
            else if (_rgx.IsMatch(CallsignSearchBox.Text) == false)
            {
                string exception = String.Format("The query '{0}' does not appear to be a invalid callsign.", CallsignSearchBox.Text.ToUpper());
                throw new ApplicationException(exception);
            }
            else
            {
                await RetrieveData();
            }

            // All done. Nothing to see here.
            ProgressRing.Visibility = Visibility.Collapsed;
            ProgressRing.IsActive = false;
        }

        private async void CallsignSearchBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            await CallsignSearch();
        }
    }
}
