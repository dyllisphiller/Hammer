using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;
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
            string CallsignTextLowercase = CallsignTextBox.Text.ToLower();
            string ResponseContent = null;
            string CallsignStatus = null;

            // Reset result fields
            LookupResultTextBlock.Text = "";

            // Construct the HTTP request parameters
            string URLParams = $"{CallsignTextLowercase}/json";

            // Set the HttpClient instance's base address
            client.BaseAddress = new Uri(EndpointURL);

            // Make it clear that we really do want JSON.
            // This might be omitted; API requests with /json return JSON
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = client.GetAsync(URLParams).Result;

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    ResponseContent = await response.Content.ReadAsStringAsync();
                    jResult = JObject.Parse(ResponseContent);

                    // Assign variables from JSON values
                    CallsignStatus = (string)jResult["status"];

                    // Display the results in their fields
                    LookupResultTextBlock.Text = CallsignStatus;
                }
                catch(Exception ex)
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
            await RetrieveData();
        }

    }
}
