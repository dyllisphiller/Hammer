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

// Project originated from Microsoft's Blank Page item template https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CallsignLookup
{
    /// <summary>
    /// The callsign lookup page.
    /// Can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        // The endpoint of the callsign lookup API
        // calllook.info is pretty awesome with their API terms
        // So they're pretty awesome, and if they're reading this: thanks!
        const string EndpointURL = "https://callook.info/";

        private async Task RetrieveData()
        {
            var client = new HttpClient();
            JObject jResult = null;
            string CallsignTextLowercase = CallsignTextBox.Text.ToLower();

            string ResponseContent = null;
            string CallsignStatus = null;

            LookupResultTextBlock.Text = "";

            string URLParams = $"{CallsignTextLowercase}/json";

            client.BaseAddress = new Uri(EndpointURL);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = client.GetAsync(URLParams).Result;

            

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    ResponseContent = await response.Content.ReadAsStringAsync();
                    jResult = JObject.Parse(ResponseContent);
                    CallsignStatus = (string)jResult["status"];
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
