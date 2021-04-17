using Hammer.Settings;
using MUXC = Microsoft.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Storage;
using System;

namespace Hammer.Views
{
    /// <summary>
    /// The settings page.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        private SettingsStore Settings { get; set; } = new SettingsStore();

        public SettingsPage()
        {
            InitializeComponent();
        }

        private void KeepSearchHistory_Toggled(object sender, RoutedEventArgs e)
        {
            if (sender is ToggleSwitch toggle)
            {
                Settings.SetSearchHistory(toggle.IsOn);
            }
        }

        private async void FeedbackButton_Click(object sender, RoutedEventArgs e)
        {
            var launcher = Microsoft.Services.Store.Engagement.StoreServicesFeedbackLauncher.GetDefault();
            await launcher.LaunchAsync();
        }

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
