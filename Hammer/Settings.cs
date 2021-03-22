using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Hammer.Settings
{
    /// <summary>
    /// Provides access to Hammer's roaming app data store.
    /// </summary>
    public class Roaming
    {
        /// <summary>
        /// The name of the roaming app data store container.
        /// </summary>
        static readonly string containerName = "settings";

        /// <summary>
        /// The roaming app data store settings container.
        /// </summary>
        static readonly ApplicationDataContainer roamingSettings = ApplicationData.Current.RoamingSettings;

        static void SetSetting(string settingKey, string settingValue) => roamingSettings.Values[settingKey] = settingValue;

        /// <summary>
        /// Gets a setting by key from the roaming settings container.
        /// </summary>
        /// <param name="settingKey">The key of the setting to return.</param>
        /// <param name="settingValue">If it exists, the value of the setting as an object; otherwise, null.</param>
        static bool TryGetSetting(string settingKey, out object settingValue)
        {
            return roamingSettings.Values.TryGetValue(settingKey, out settingValue);
        }
    }
}
