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
using System.ComponentModel;

namespace Hammer.Views
{
    /// <summary>
    /// The license search result page, meant to be navigated to within a Frame.
    /// </summary>
    public sealed partial class SearchPage : Page, INotifyPropertyChanged
    {
        internal CultureInfo invariantCulture = CultureInfo.InvariantCulture;
        internal LicenseViewModel ViewModel { get; set; } = new LicenseViewModel();

        public event PropertyChangedEventHandler PropertyChanged;

        public SearchPage()
        {
            InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            string p = (string)e.Parameter;
            // if the user navigates to the page without a search parameter,
            // the page should guide the user to search
            if (string.IsNullOrWhiteSpace(p))
            {
                SearchPageDefaultHeader.Visibility = Visibility.Visible;
            }

            else
            {
                SearchPageDefaultHeader.Visibility = Visibility.Collapsed;

                try
                {
                    await CallsignSearch(p);
                }
                catch (Exception ex)
                {
                    ContentDialog contentDialog = new ContentDialog()
                    {
                        Title = ex.Message,
                        CloseButtonText = "OK",
                    };
                    await contentDialog.ShowAsync();
                }
            }
        }

        private async Task CallsignSearch(string callsign)
        {
            if (string.IsNullOrEmpty(callsign))
            {
                ContentDialog dialog = new ContentDialog()
                {
                    Title = "Enter a callsign to look up a license",
                    Content = "The search field cannot be empty.",
                    CloseButtonText = "OK",
                };
                await dialog.ShowAsync();
            }

            SearchResultsStackPanel.Visibility = Visibility.Collapsed;
            SearchProgressRing.IsActive = true;
            SearchProgressRing.Visibility = Visibility.Visible;

            ViewModel.License = await Parsers.GetLicenseFromJsonAsync(callsign);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ViewModel.License"));

            switch (ViewModel.License.Status)
            {
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
                        ShowMapButton.IsEnabled = true;
                        SearchTrusteeButton.Visibility = ViewModel.License.Trustee == null ? Visibility.Collapsed : Visibility.Visible;
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
                new Geopoint(new BasicGeoposition() { Latitude = ViewModel.License.Location.Latitude, Longitude = ViewModel.License.Location.Longitude }),
                ViewModel.License.Callsign.Sign,
                ViewModel.License.AddressLine1,
                ViewModel.License.AddressLine2);
        }

        private async void UlsUriButton_Click(object sender, RoutedEventArgs e)
        {
            LauncherOptions options = new LauncherOptions
            {
                TreatAsUntrusted = true
            };

            await Launcher.LaunchUriAsync(ViewModel.License.UlsUri, options);
        }

        private async void SearchTrusteeButton_Click(object sender, RoutedEventArgs e)
        {
            await CallsignSearch(ViewModel.License.Trustee.Sign);
        }
    }
}
