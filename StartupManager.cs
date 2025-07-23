using Microsoft.Win32;
using System;
using System.IO;
using System.Windows.Forms;

namespace MultipleCopyPaste
{
    public static class StartupManager
    {
        private const string StartupKey = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";
        private const string AppName = "MultipleCopyPaste";

        public static bool IsStartupEnabled()
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(StartupKey))
                {
                    if (key != null)
                    {
                        string value = key.GetValue(AppName) as string;
                        return !string.IsNullOrEmpty(value);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error checking startup status: {ex.Message}", "Startup Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return false;
        }

        public static bool EnableStartup()
        {
            try
            {
                string exePath = Application.ExecutablePath;
                
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(StartupKey, true))
                {
                    if (key != null)
                    {
                        key.SetValue(AppName, $"\"{exePath}\"");
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error enabling startup: {ex.Message}", "Startup Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return false;
        }

        public static bool DisableStartup()
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(StartupKey, true))
                {
                    if (key != null)
                    {
                        key.DeleteValue(AppName, false);
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error disabling startup: {ex.Message}", "Startup Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return false;
        }

        public static void ToggleStartup(bool enable)
        {
            if (enable)
            {
                if (EnableStartup())
                {
                    MessageBox.Show("Application will now start with Windows.", "Startup Enabled", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                if (DisableStartup())
                {
                    MessageBox.Show("Application will no longer start with Windows.", "Startup Disabled", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
} 