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
        /// The name of the container within the roaming app data store.
        /// </summary>
        static string containerName = "settings";

        /// <summary>
        /// The application settings container in the roaming app data store.
        /// </summary>
        static ApplicationDataContainer roamingSettings = ApplicationData.Current.RoamingSettings;

        // Creates the container specified by containerName if it does not already exist.
        ApplicationDataContainer makeContainer =
            roamingSettings.CreateContainer(containerName, ApplicationDataCreateDisposition.Always);

        public static void SetSetting(Tuple<string, string> settingTuple)
        {
            Contract.Requires<ArgumentNullException>(settingTuple != null, "settingTuple");
            string key = settingTuple.Item1;
            string value = settingTuple.Item2;
            roamingSettings.Containers[containerName].Values[key] = value;
        }
        /// <summary>
        /// Gets a setting by key from the roaming settings container.
        /// </summary>
        /// <param name="key">The key of the setting to return.</param>
        public static string GetSetting(string key)
        {
            bool hasKey = roamingSettings.Containers[containerName].Values.ContainsKey(key);
            string settingValue;

            if (hasKey)
            {
                settingValue = roamingSettings.Containers[containerName].Values[key].ToString();
                //Containers[roamingSettingsContainerName].Values.ContainsKey[key];
                return settingValue;
            }
            else
            {
                throw new ArgumentException($"The specified key \"{key}\" does not exist.");
            }
        }
    }
}
