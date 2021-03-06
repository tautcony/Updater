﻿using Microsoft.Win32;

namespace Updater
{
    public static class RegistryStorage
    {
        public static string DefaultSubKey => $@"Software\{Utils.SoftwareName}";

        public static string Load(string subKey = null, string name = "ExecutePath")
        {
            if (subKey == null) subKey = DefaultSubKey;
            var path = string.Empty;
            // HKCU_CURRENT_USER\Software\
            var registryKey = Registry.CurrentUser.OpenSubKey(subKey);
            if (registryKey == null) return path;
            path = (string)registryKey.GetValue(name);
            registryKey.Close();
            return path;
        }

        public static void Save(string value, string subKey = null, string name = "ExecutePath")
        {
            if (subKey == null) subKey = DefaultSubKey;
            // HKCU_CURRENT_USER\Software\
            var registryKey = Registry.CurrentUser.CreateSubKey(subKey);
            registryKey?.SetValue(name, value);
            registryKey?.Close();
        }

        public static int RegistryAddCount(string subKey, string name, int delta = 1)
        {
            var countS = Load(subKey, name);
            var count = string.IsNullOrEmpty(countS) ? 0 : int.Parse(countS);
            count += delta;
            Save(count.ToString(), subKey, name);
            return count - delta;
        }
    }
}
