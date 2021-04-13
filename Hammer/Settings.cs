using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.Storage;

namespace Hammer.Settings
{
    /// <summary>
    /// Provides access to Hammer's app data stores.
    /// </summary>
    [Serializable]
    class SettingsStore : INotifyPropertyChanged
    {
        public bool KeepSearchHistory { get; set; }
        public IList<string> SearchHistory { get; set; }

        static readonly string settingsContainerName = "settings";
        static readonly string cacheContainerName = "cache";

        static readonly ApplicationDataContainer roamingSettings = ApplicationData.Current.RoamingSettings.CreateContainer(settingsContainerName, ApplicationDataCreateDisposition.Always);
        static readonly ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings.CreateContainer(cacheContainerName, ApplicationDataCreateDisposition.Always);

        public event PropertyChangedEventHandler PropertyChanged;

        static void SetSetting(string settingKey, object settingValue) => roamingSettings.Values[settingKey] = settingValue;

        static bool TryGetSetting(string settingKey, out object settingValue)
        {
            return roamingSettings.Values.TryGetValue(settingKey, out settingValue);
        }

        public static void SetSearchHistory(bool isEnabled)
        {
            SetSetting("searchHistoryEnabled", isEnabled);
        }
    }
}
