using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Hammer.Core.Helpers;
using Windows.Storage;

namespace Hammer.Settings
{
    /// <summary>
    /// Provides access to Hammer's app data stores.
    /// </summary>
    [Serializable]
    public class SettingsStore : INotifyPropertyChanged
    {
        public bool KeepSearchHistory { get; set; }
        public MostRecentlyUsedList<string> SearchHistory { get; set; }

        static readonly string settingsContainerName = "settings";
        static readonly string cacheContainerName = "cache";

        static readonly ApplicationDataContainer roamingSettings = ApplicationData.Current.RoamingSettings.CreateContainer(settingsContainerName, ApplicationDataCreateDisposition.Always);
        static readonly ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings.CreateContainer(cacheContainerName, ApplicationDataCreateDisposition.Always);

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public SettingsStore()
        {
            if (roamingSettings.Values.ContainsKey("searchHistoryEnabled"))
            {
                KeepSearchHistory = (bool)roamingSettings.Values["searchHistoryEnabled"];
            }
            else
            {
                roamingSettings.Values["searchHistoryEnabled"] = true;
                KeepSearchHistory = true;
            }
        }

        public void SetSetting<T>(string settingKey, T settingValue)
        {
            roamingSettings.Values[settingKey] = settingValue;
            OnPropertyChanged();
        }

        public bool TryGetSetting<T>(string settingKey, out T settingValue)
        {
            bool exists = roamingSettings.Values.TryGetValue(settingKey, out object _value);
            settingValue = (T)_value;
            return exists;
        }

        public void SetSearchHistory(bool isEnabled)
        {
            SetSetting("searchHistoryEnabled", isEnabled);
            OnPropertyChanged(nameof(KeepSearchHistory));
        }
    }
}
