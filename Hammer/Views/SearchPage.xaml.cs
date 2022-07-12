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
        internal CultureInfo currentCulture = CultureInfo.CurrentCulture;
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
                    Callsign callsign = new Callsign(p);
                    await CallsignSearch(callsign);
                }
                catch (Exception ex)
                {
                    //await ExceptionDialog(ex);
                }
            }

            base.OnNavigatedTo(e);
        }

        private async Task CallsignSearch(Callsign callsign)
        {
            if (string.IsNullOrEmpty(callsign.Sign))
            {
                ContentDialog dialog = new ContentDialog()
                {
                    Content = "Enter a callsign to look up a license.",
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
                await ExceptionDialog(ex);
            }
            catch (LicenseDatabaseUpdatingException ex)
            {
                await ExceptionDialog(ex);
            }
            catch (Exception ex)
            {
                await ExceptionDialog(ex);
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

            //BasicGeoposition mapPosition = new BasicGeoposition() { Latitude = ViewModel.License.Location.Latitude, Longitude = ViewModel.License.Location.Longitude };
            //Geopoint mapPositionCenter = new Geopoint(mapPosition);
            //LicenseLocationMapControl.Center = mapPositionCenter;
            //LicenseLocationMapControl.ZoomLevel = 12;
            //LicenseLocationMapControl.LandmarksVisible = true;

            // All done. Nothing to see here.
            SearchProgressRing.Visibility = Visibility.Collapsed;
            SearchProgressRing.IsActive = false;
            SearchResultsStackPanel.Visibility = Visibility.Visible;
        }

        private static async Task ExceptionDialog(Exception ex)
        {
            ContentDialog dialog = new ContentDialog()
            {
                Content = ex.Message,
                CloseButtonText = "OK",
            };
            await dialog.ShowAsync();
        }

        private void ShowMapButton_Click(object sender, RoutedEventArgs e)
        {
            Placecard.ShowPlacecard(sender,
                new Geopoint(new BasicGeoposition() { Latitude = ViewModel.License.Location.Latitude, Longitude = ViewModel.License.Location.Longitude }),
                ViewModel.License.Name,
                ViewModel.License.AddressLine1,
                ViewModel.License.AddressLine2);
        }

        //private async void UlsUriButton_Click(object sender, RoutedEventArgs e)
        //{
        //    LauncherOptions options = new LauncherOptions
        //    {
        //        TreatAsUntrusted = true
        //    };

        //    await Launcher.LaunchUriAsync(ViewModel.License.UlsUri, options);
        //}

        private async void SearchTrusteeButton_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.License is ClubLicense _cl)
            {
                await CallsignSearch(_cl.Trustee.Callsign);
            }
        }
    }
}
