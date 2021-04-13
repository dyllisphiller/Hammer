﻿using Hammer.Settings;
using MUXC = Microsoft.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Storage;

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
    }
}
