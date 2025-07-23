using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MultipleCopyPaste
{
    public partial class StatisticsForm : Form
    {
        private string dbPath;
        private DataGridView dgvStats;
        private Label lblTotalItems;
        private Label lblTodayItems;
        private Label lblMostUsedWords;

        public StatisticsForm(string databasePath)
        {
            dbPath = databasePath;
            InitializeComponent();
            LoadStatistics();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Form settings
            this.Text = "Usage Statistics - Syaiful Wachid";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Title
            Label lblTitle = new Label
            {
                Text = "Multiple Copy Paste - Usage Statistics",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.DarkBlue,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 50
            };

            // Stats Panel
            Panel statsPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 120,
                BackColor = Color.LightYellow
            };

            lblTotalItems = new Label
            {
                Text = "Total Items: 0",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(20, 20),
                Size = new Size(200, 25),
                ForeColor = Color.DarkGreen
            };

            lblTodayItems = new Label
            {
                Text = "Today's Items: 0",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(20, 50),
                Size = new Size(200, 25),
                ForeColor = Color.DarkGreen
            };

            lblMostUsedWords = new Label
            {
                Text = "Most Used Words: None",
                Font = new Font("Segoe UI", 10),
                Location = new Point(20, 80),
                Size = new Size(400, 25),
                ForeColor = Color.DarkBlue
            };

            statsPanel.Controls.AddRange(new Control[] { lblTotalItems, lblTodayItems, lblMostUsedWords });

            // DataGridView
            dgvStats = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                Font = new Font("Segoe UI", 9)
            };

            // Button Panel
            Panel buttonPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 50,
                BackColor = Color.LightGray
            };

            Button btnRefresh = new Button
            {
                Text = "Refresh Statistics",
                Location = new Point(10, 12),
                Size = new Size(120, 30),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                BackColor = Color.Blue,
                ForeColor = Color.White
            };
            btnRefresh.Click += BtnRefresh_Click;

            Button btnClose = new Button
            {
                Text = "Close",
                Location = new Point(140, 12),
                Size = new Size(80, 30),
                Font = new Font("Segoe UI", 9)
            };
            btnClose.Click += BtnClose_Click;

            buttonPanel.Controls.AddRange(new Control[] { btnRefresh, btnClose });

            // Add controls
            this.Controls.Add(dgvStats);
            this.Controls.Add(buttonPanel);
            this.Controls.Add(statsPanel);
            this.Controls.Add(lblTitle);

            this.ResumeLayout(false);
        }

        private void LoadStatistics()
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
                {
                    conn.Open();

                    // Get total items
                    string totalSql = "SELECT COUNT(*) FROM copied_items";
                    using (SQLiteCommand cmd = new SQLiteCommand(totalSql, conn))
                    {
                        int totalItems = Convert.ToInt32(cmd.ExecuteScalar());
                        lblTotalItems.Text = $"Total Items: {totalItems:N0}";
                    }

                    // Get today's items
                    string todaySql = "SELECT COUNT(*) FROM copied_items WHERE DATE(timestamp) = DATE('now')";
                    using (SQLiteCommand cmd = new SQLiteCommand(todaySql, conn))
                    {
                        int todayItems = Convert.ToInt32(cmd.ExecuteScalar());
                        lblTodayItems.Text = $"Today's Items: {todayItems:N0}";
                    }

                    // Get most used words
                    var wordStats = GetMostUsedWords(conn);
                    if (wordStats.Count > 0)
                    {
                        var topWords = wordStats.Take(5).Select(w => $"{w.Key}({w.Value})").ToArray();
                        lblMostUsedWords.Text = $"Most Used Words: {string.Join(", ", topWords)}";
                    }

                    // Load detailed statistics
                    LoadDetailedStats(conn);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading statistics: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Dictionary<string, int> GetMostUsedWords(SQLiteConnection conn)
        {
            var wordStats = new Dictionary<string, int>();

            string sql = "SELECT content FROM copied_items";
            using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
            {
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string content = reader["content"].ToString();
                        var words = content.Split(new[] { ' ', '\t', '\n', '\r', '.', ',', ';', ':', '!', '?' }, 
                            StringSplitOptions.RemoveEmptyEntries);

                        foreach (var word in words)
                        {
                            string cleanWord = word.ToLower().Trim();
                            if (cleanWord.Length > 2) // Only count words with more than 2 characters
                            {
                                if (wordStats.ContainsKey(cleanWord))
                                    wordStats[cleanWord]++;
                                else
                                    wordStats[cleanWord] = 1;
                            }
                        }
                    }
                }
            }

            return wordStats.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
        }

        private void LoadDetailedStats(SQLiteConnection conn)
        {
            dgvStats.Columns.Clear();
            dgvStats.Rows.Clear();

            // Add columns
            dgvStats.Columns.Add("Date", "Date");
            dgvStats.Columns.Add("Items", "Items Count");
            dgvStats.Columns.Add("Percentage", "Percentage");

            string sql = @"
                SELECT 
                    DATE(timestamp) as date,
                    COUNT(*) as count
                FROM copied_items 
                GROUP BY DATE(timestamp) 
                ORDER BY date DESC 
                LIMIT 30";

            using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
            {
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    int totalItems = 0;
                    var dailyStats = new List<(string date, int count)>();

                    while (reader.Read())
                    {
                        string date = reader["date"].ToString();
                        int count = Convert.ToInt32(reader["count"]);
                        dailyStats.Add((date, count));
                        totalItems += count;
                    }

                    // Add rows to DataGridView
                    foreach (var stat in dailyStats)
                    {
                        double percentage = totalItems > 0 ? (double)stat.count / totalItems * 100 : 0;
                        dgvStats.Rows.Add(stat.date, stat.count, $"{percentage:F1}%");
                    }
                }
            }

            // Style the DataGridView
            dgvStats.Columns[0].Width = 120;
            dgvStats.Columns[1].Width = 100;
            dgvStats.Columns[2].Width = 100;
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadStatistics();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
} 