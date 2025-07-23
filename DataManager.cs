using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;

namespace MultipleCopyPaste
{
    public class DataManager
    {
        private string dbPath;

        public DataManager(string databasePath)
        {
            dbPath = databasePath;
        }

        public bool ExportToJson(string filePath)
        {
            try
            {
                var items = new List<CopiedItem>();
                
                using (SQLiteConnection conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
                {
                    conn.Open();
                    string sql = "SELECT id, content, timestamp FROM copied_items ORDER BY timestamp DESC";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                items.Add(new CopiedItem
                                {
                                    Id = Convert.ToInt32(reader["id"]),
                                    Content = reader["content"].ToString(),
                                    Timestamp = DateTime.Parse(reader["timestamp"].ToString())
                                });
                            }
                        }
                    }
                }

                var exportData = new ExportData
                {
                    ExportDate = DateTime.Now,
                    TotalItems = items.Count,
                    Items = items,
                    ExportedBy = "Syaiful Wachid - Fiberhome Indonesia"
                };

                string json = JsonSerializer.Serialize(exportData, new JsonSerializerOptions 
                { 
                    WriteIndented = true 
                });
                
                File.WriteAllText(filePath, json);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting data: {ex.Message}", "Export Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public bool ImportFromJson(string filePath)
        {
            try
            {
                string json = File.ReadAllText(filePath);
                var exportData = JsonSerializer.Deserialize<ExportData>(json);

                if (exportData?.Items == null)
                {
                    MessageBox.Show("Invalid export file format.", "Import Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                using (SQLiteConnection conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
                {
                    conn.Open();
                    
                    // Clear existing data
                    using (SQLiteCommand cmd = new SQLiteCommand("DELETE FROM copied_items", conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    // Insert imported data
                    string sql = "INSERT INTO copied_items (content, timestamp) VALUES (@content, @timestamp)";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        foreach (var item in exportData.Items)
                        {
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@content", item.Content);
                            cmd.Parameters.AddWithValue("@timestamp", item.Timestamp);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }

                MessageBox.Show($"Successfully imported {exportData.Items.Count} items.", "Import Success", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error importing data: {ex.Message}", "Import Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public List<CopiedItem> GetAllItems()
        {
            var items = new List<CopiedItem>();
            
            using (SQLiteConnection conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
            {
                conn.Open();
                string sql = "SELECT id, content, timestamp FROM copied_items ORDER BY timestamp DESC";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            items.Add(new CopiedItem
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                Content = reader["content"].ToString(),
                                Timestamp = DateTime.Parse(reader["timestamp"].ToString())
                            });
                        }
                    }
                }
            }
            
            return items;
        }

        public bool ClearAllData()
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
                {
                    conn.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand("DELETE FROM copied_items", conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error clearing data: {ex.Message}", "Clear Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public int GetItemCount()
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
                {
                    conn.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand("SELECT COUNT(*) FROM copied_items", conn))
                    {
                        return Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
            catch
            {
                return 0;
            }
        }
    }

    public class CopiedItem
    {
        public int Id { get; set; }
        public string Content { get; set; } = "";
        public DateTime Timestamp { get; set; }
    }

    public class ExportData
    {
        public DateTime ExportDate { get; set; }
        public int TotalItems { get; set; }
        public List<CopiedItem> Items { get; set; } = new List<CopiedItem>();
        public string ExportedBy { get; set; } = "";
    }
} 