// Helpers/Settings.cs
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace App14.Helpers
{
    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        #region Setting Constants

        private const string SettingsKey = "settings_key";
        private const string DeviceTokenKey = "DeviceTokenKey";

        private static readonly string SettingsDefault = string.Empty;
        private static readonly string DeviceTokenKeyDefault = string.Empty;

        #endregion


        public static string GeneralSettings
        {
            get
            {
                return AppSettings.GetValueOrDefault(SettingsKey, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(SettingsKey, value);
            }
        }

        public static string DeviceToken
        {
            get
            {
                return AppSettings.GetValueOrDefault(DeviceTokenKey, DeviceTokenKeyDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(DeviceTokenKey, value);
            }
        }

    }
}