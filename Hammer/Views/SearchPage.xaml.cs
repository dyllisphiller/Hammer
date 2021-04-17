using System;
using System.Globalization;
using System.Threading.Tasks;
using Hammer.Core;
using Hammer.Core.Helpers;
using Hammer.Core.Models;
using Hammer.Core.ViewModels;
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

            // if the user navigates here directly, guide the user to search
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

        private async Task CallsignSearch(Callsign callsign)
        {
            await CallsignSearch(callsign.ToString());
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

            try
            {
                ViewModel.License = await Parsers.GetLicenseFromJsonAsync(callsign);
            }
            catch (UnsupportedIssuerException ex)
            {
                ContentDialog _ = new ContentDialog()
                {
                    Title = ex.Message,
                    CloseButtonText = "OK",
                };
                await _.ShowAsync();
            }
            catch (LicenseDatabaseUpdatingException ex)
            {
                ContentDialog _ = new ContentDialog()
                {
                    Title = ex.Message,
                    CloseButtonText = "OK",
                };
                await _.ShowAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }


            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ViewModel.License"));

            if (ViewModel.License.Status == LicenseStatus.Invalid)
            {
                ContentDialog dialog = new ContentDialog()
                {
                    Title = "Either the callsign is invalid or something unexpected happened.",
                    Content = "Try again later. If the error persists, please consider filing a bug report.",
                    CloseButtonText = "OK",
                };
                await dialog.ShowAsync();
            }

            ShowMapButton.IsEnabled = true;
            SearchTrusteeButton.Visibility = ViewModel.License is IInTrust ? Visibility.Collapsed : Visibility.Visible;
            AddressAttnField.Visibility = ViewModel.License.AddressAttn == null ? Visibility.Collapsed : Visibility.Visible;

            //BasicGeoposition mapPosition = new BasicGeoposition() { Latitude = licenseSearchResult.Location.Latitude, Longitude = licenseSearchResult.Location.Longitude };
            //Geopoint mapPositionCenter = new Geopoint(mapPosition);
            //LicenseLocationMapControl.Center = mapPositionCenter;
            //LicenseLocationMapControl.ZoomLevel = 12;
            //LicenseLocationMapControl.LandmarksVisible = true;

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
            if (ViewModel.License is ClubLicense)
            {
                await CallsignSearch(((ClubLicense)ViewModel.License).Trustee.Callsign);
            }
        }
    }
}
