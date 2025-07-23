using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MultipleCopyPaste
{
    public static class BackupManager
    {
        private static readonly string BackupFolder = "Backups";
        private static readonly int MaxBackups = 10;

        public static void InitializeBackupFolder()
        {
            try
            {
                if (!Directory.Exists(BackupFolder))
                {
                    Directory.CreateDirectory(BackupFolder);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating backup folder: {ex.Message}", "Backup Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public static async Task<bool> CreateBackupAsync(string dbPath)
        {
            try
            {
                if (!File.Exists(dbPath))
                {
                    return false;
                }

                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string backupFileName = $"copied_items_backup_{timestamp}.db";
                string backupPath = Path.Combine(BackupFolder, backupFileName);

                // Copy database file
                File.Copy(dbPath, backupPath, true);

                // Clean old backups
                CleanOldBackups();

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating backup: {ex.Message}", "Backup Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public static bool RestoreBackup(string backupPath, string dbPath)
        {
            try
            {
                if (!File.Exists(backupPath))
                {
                    MessageBox.Show("Backup file not found.", "Restore Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                // Create backup of current database before restore
                if (File.Exists(dbPath))
                {
                    string currentBackup = dbPath + ".before_restore";
                    File.Copy(dbPath, currentBackup, true);
                }

                // Restore from backup
                File.Copy(backupPath, dbPath, true);

                MessageBox.Show("Backup restored successfully!", "Restore Success", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error restoring backup: {ex.Message}", "Restore Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private static void CleanOldBackups()
        {
            try
            {
                if (!Directory.Exists(BackupFolder))
                    return;

                var backupFiles = Directory.GetFiles(BackupFolder, "copied_items_backup_*.db");
                
                if (backupFiles.Length > MaxBackups)
                {
                    // Sort by creation time (oldest first)
                    Array.Sort(backupFiles, (a, b) => File.GetCreationTime(a).CompareTo(File.GetCreationTime(b)));

                    // Delete oldest files
                    int filesToDelete = backupFiles.Length - MaxBackups;
                    for (int i = 0; i < filesToDelete; i++)
                    {
                        try
                        {
                            File.Delete(backupFiles[i]);
                        }
                        catch
                        {
                            // Ignore errors when deleting old backups
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error but don't show to user
                Console.WriteLine($"Error cleaning old backups: {ex.Message}");
            }
        }

        public static string[] GetAvailableBackups()
        {
            try
            {
                if (!Directory.Exists(BackupFolder))
                    return new string[0];

                var backupFiles = Directory.GetFiles(BackupFolder, "copied_items_backup_*.db");
                Array.Sort(backupFiles, (a, b) => File.GetCreationTime(b).CompareTo(File.GetCreationTime(a)));
                return backupFiles;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error getting backup list: {ex.Message}", "Backup Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return new string[0];
            }
        }

        public static string GetBackupInfo(string backupPath)
        {
            try
            {
                if (!File.Exists(backupPath))
                    return "File not found";

                var fileInfo = new FileInfo(backupPath);
                var creationTime = fileInfo.CreationTime;
                var size = fileInfo.Length;

                return $"Created: {creationTime:yyyy-MM-dd HH:mm:ss}\nSize: {size:N0} bytes";
            }
            catch
            {
                return "Error reading file info";
            }
        }

        public static async Task ScheduleAutomaticBackup(string dbPath)
        {
            // This method can be called periodically to create automatic backups
            await CreateBackupAsync(dbPath);
        }
    }
} 