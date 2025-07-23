using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MultipleCopyPaste
{
    public partial class MainForm : Form
    {
        private NotifyIcon notifyIcon;
        private ContextMenuStrip contextMenu;
        private List<string> copiedItems = new List<string>();
        private string dbPath = "copied_items.db";
        private bool isMinimized = false;
        private DataManager dataManager;

        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private const int HOTKEY_F1 = 1;
        private const int HOTKEY_F2 = 2;

        public MainForm()
        {
            InitializeComponent();
            dataManager = new DataManager(dbPath);
            BackupManager.InitializeBackupFolder();
            InitializeDatabase();
            InitializeNotifyIcon();
            RegisterHotKeys();
            LoadCopiedItems();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            // Form settings
            this.Text = "Multiple Copy Paste - Syaiful Wachid";
            this.Size = new Size(1000, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Icon = new Icon("app.ico");
            
            // Create controls
            CreateControls();
            
            this.ResumeLayout(false);
        }

        private void CreateControls()
        {
            // Title Label
            Label lblTitle = new Label
            {
                Text = "Multiple Copy Paste Manager",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.DarkBlue,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 50
            };

            // Instructions
            Label lblInstructions = new Label
            {
                Text = "F1: Copy | F2: Paste | F4: Search | F5: Statistics | F6: Settings | F7: About | Right-click tray for menu",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.Gray,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 30
            };

            // ListBox for copied items
            ListBox listBoxItems = new ListBox
            {
                Name = "listBoxItems",
                Dock = DockStyle.Fill,
                Font = new Font("Consolas", 10),
                SelectionMode = SelectionMode.One
            };
            listBoxItems.DoubleClick += ListBoxItems_DoubleClick;

            // Buttons Panel
            Panel buttonPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 100
            };

            // Row 1 - Basic Functions
            Button btnCopy = new Button
            {
                Text = "Copy Selected (F1)",
                Size = new Size(130, 35),
                Location = new Point(10, 15),
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            btnCopy.Click += BtnCopy_Click;

            Button btnPaste = new Button
            {
                Text = "Paste Next (F2)",
                Size = new Size(130, 35),
                Location = new Point(150, 15),
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            btnPaste.Click += BtnPaste_Click;

            Button btnClear = new Button
            {
                Text = "Clear All",
                Size = new Size(110, 35),
                Location = new Point(290, 15),
                Font = new Font("Segoe UI", 9)
            };
            btnClear.Click += BtnClear_Click;

            Button btnMinimize = new Button
            {
                Text = "Minimize to Tray",
                Size = new Size(130, 35),
                Location = new Point(410, 15),
                Font = new Font("Segoe UI", 9)
            };
            btnMinimize.Click += BtnMinimize_Click;

            Button btnExport = new Button
            {
                Text = "Export Data",
                Size = new Size(110, 35),
                Location = new Point(550, 15),
                Font = new Font("Segoe UI", 9)
            };
            btnExport.Click += BtnExport_Click;

            Button btnImport = new Button
            {
                Text = "Import Data",
                Size = new Size(110, 35),
                Location = new Point(670, 15),
                Font = new Font("Segoe UI", 9)
            };
            btnImport.Click += BtnImport_Click;

            // Row 2 - Advanced Functions (sejajar dengan Delete Selected)
            Button btnDeleteSelected = new Button
            {
                Text = "Delete Selected",
                Size = new Size(130, 35),
                Location = new Point(10, 60),
                Font = new Font("Segoe UI", 9),
                BackColor = Color.Red,
                ForeColor = Color.White
            };
            btnDeleteSelected.Click += BtnDeleteSelected_Click;

            Button btnSearch = new Button
            {
                Text = "Search Items (F4)",
                Size = new Size(130, 35),
                Location = new Point(150, 60),
                Font = new Font("Segoe UI", 9),
                BackColor = Color.Orange,
                ForeColor = Color.White
            };
            btnSearch.Click += BtnSearch_Click;

            Button btnStats = new Button
            {
                Text = "Statistics (F5)",
                Size = new Size(130, 35),
                Location = new Point(290, 60),
                Font = new Font("Segoe UI", 9),
                BackColor = Color.Purple,
                ForeColor = Color.White
            };
            btnStats.Click += BtnStats_Click;

            Button btnSettings = new Button
            {
                Text = "Settings (F6)",
                Size = new Size(130, 35),
                Location = new Point(430, 60),
                Font = new Font("Segoe UI", 9),
                BackColor = Color.Gray,
                ForeColor = Color.White
            };
            btnSettings.Click += BtnSettings_Click;

            Button btnAbout = new Button
            {
                Text = "About (F7)",
                Size = new Size(130, 35),
                Location = new Point(570, 60),
                Font = new Font("Segoe UI", 9),
                BackColor = Color.Teal,
                ForeColor = Color.White
            };
            btnAbout.Click += BtnAbout_Click;

            // Author info
            Label lblAuthor = new Label
            {
                Text = "Dibuat oleh: Syaiful Wachid\nSenior Project Designer: Fiberhome Indonesia\nLinkedIn Profile: https://www.linkedin.com/in/syaiful-wachid-5373n/",
                Font = new Font("Segoe UI", 8),
                ForeColor = Color.DarkGreen,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Bottom,
                Height = 60,
                BackColor = Color.LightYellow
            };

            // Add controls
            buttonPanel.Controls.AddRange(new Control[] { btnCopy, btnPaste, btnClear, btnDeleteSelected, btnMinimize, btnExport, btnImport, btnSearch, btnStats, btnSettings, btnAbout });
            
            this.Controls.Add(lblAuthor);
            this.Controls.Add(buttonPanel);
            this.Controls.Add(listBoxItems);
            this.Controls.Add(lblInstructions);
            this.Controls.Add(lblTitle);

            // Store reference to listbox
            this.Controls.Find("listBoxItems", true)[0].Tag = listBoxItems;
        }

        private void InitializeDatabase()
        {
            if (!File.Exists(dbPath))
            {
                using (SQLiteConnection conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
                {
                    conn.Open();
                    string sql = @"
                        CREATE TABLE copied_items (
                            id INTEGER PRIMARY KEY AUTOINCREMENT,
                            content TEXT NOT NULL,
                            timestamp DATETIME DEFAULT CURRENT_TIMESTAMP
                        )";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        private void InitializeNotifyIcon()
        {
            contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("Show Main Window", null, ShowMainWindow);
            contextMenu.Items.Add("Copy Selected (F1)", null, (s, e) => CopySelected());
            contextMenu.Items.Add("Paste Next (F2)", null, (s, e) => PasteNext());
            contextMenu.Items.Add("-");
            contextMenu.Items.Add("Search Items (F4)", null, (s, e) => BtnSearch_Click(s, e));
            contextMenu.Items.Add("Statistics (F5)", null, (s, e) => BtnStats_Click(s, e));
            contextMenu.Items.Add("-");
            contextMenu.Items.Add("Settings (F6)", null, (s, e) => BtnSettings_Click(s, e));
            contextMenu.Items.Add("About (F7)", null, (s, e) => BtnAbout_Click(s, e));
            contextMenu.Items.Add("-");
            contextMenu.Items.Add("Create Backup", null, (s, e) => CreateBackup());
            contextMenu.Items.Add("Clear All Items", null, ClearAllItems);
            contextMenu.Items.Add("-");
            contextMenu.Items.Add("Exit", null, ExitApplication);

            notifyIcon = new NotifyIcon
            {
                Icon = new Icon("app.ico"),
                Text = "Multiple Copy Paste",
                Visible = true,
                ContextMenuStrip = contextMenu
            };

            notifyIcon.DoubleClick += (s, e) => ShowMainWindow(s, e);
        }

        private void RegisterHotKeys()
        {
            RegisterHotKey(this.Handle, HOTKEY_F1, 0, 0x70); // F1
            RegisterHotKey(this.Handle, HOTKEY_F2, 0, 0x71); // F2
            
            // Register additional hotkeys for other functions
            RegisterHotKey(this.Handle, 3, 0, 0x73); // F4 - Search
            RegisterHotKey(this.Handle, 4, 0, 0x74); // F5 - Statistics
            RegisterHotKey(this.Handle, 5, 0, 0x75); // F6 - Settings
            RegisterHotKey(this.Handle, 6, 0, 0x76); // F7 - About
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_HOTKEY = 0x0312;
            if (m.Msg == WM_HOTKEY)
            {
                switch (m.WParam.ToInt32())
                {
                    case HOTKEY_F1:
                        CopySelected();
                        break;
                    case HOTKEY_F2:
                        PasteNext();
                        break;
                    case 3: // F4
                        BtnSearch_Click(this, EventArgs.Empty);
                        break;
                    case 4: // F5
                        BtnStats_Click(this, EventArgs.Empty);
                        break;
                    case 5: // F6
                        BtnSettings_Click(this, EventArgs.Empty);
                        break;
                    case 6: // F7
                        BtnAbout_Click(this, EventArgs.Empty);
                        break;
                }
            }
            base.WndProc(ref m);
        }

        private void LoadCopiedItems()
        {
            copiedItems.Clear();
            using (SQLiteConnection conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
            {
                conn.Open();
                string sql = "SELECT content FROM copied_items ORDER BY timestamp DESC";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            copiedItems.Add(reader["content"].ToString());
                        }
                    }
                }
            }
            UpdateListBox();
        }

        private void UpdateListBox()
        {
            ListBox listBox = (ListBox)this.Controls.Find("listBoxItems", true)[0];
            listBox.Items.Clear();
            for (int i = 0; i < copiedItems.Count; i++)
            {
                string displayText = copiedItems[i];
                if (displayText.Length > 50)
                    displayText = displayText.Substring(0, 47) + "...";
                listBox.Items.Add($"Item {i + 1}: {displayText}");
            }
        }

        private void CopySelected()
        {
            // Simulate Ctrl+C
            SendKeys.Send("^c");
            
            // Wait a bit for clipboard to update
            System.Threading.Thread.Sleep(100);
            
            if (Clipboard.ContainsText())
            {
                string text = Clipboard.GetText();
                if (!string.IsNullOrWhiteSpace(text))
                {
                    copiedItems.Insert(0, text);
                    SaveToDatabase(text);
                    UpdateListBox();
                    ShowNotification($"Copied: {text.Substring(0, Math.Min(30, text.Length))}...");
                }
            }
        }

        private void PasteNext()
        {
            if (copiedItems.Count > 0)
            {
                string itemToPaste = copiedItems[0];
                copiedItems.RemoveAt(0);
                
                // Save current clipboard
                string originalClipboard = "";
                if (Clipboard.ContainsText())
                    originalClipboard = Clipboard.GetText();
                
                // Set new clipboard and paste
                Clipboard.SetText(itemToPaste);
                SendKeys.Send("^v");
                
                // Restore original clipboard
                System.Threading.Thread.Sleep(100);
                if (!string.IsNullOrEmpty(originalClipboard))
                    Clipboard.SetText(originalClipboard);
                
                UpdateListBox();
                RemoveFromDatabase(itemToPaste);
                ShowNotification($"Pasted. Remaining: {copiedItems.Count}");
            }
            else
            {
                ShowNotification("No items to paste!");
            }
        }

        private void SaveToDatabase(string content)
        {
            using (SQLiteConnection conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
            {
                conn.Open();
                string sql = "INSERT INTO copied_items (content) VALUES (@content)";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@content", content);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void RemoveFromDatabase(string content)
        {
            using (SQLiteConnection conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
            {
                conn.Open();
                string sql = "DELETE FROM copied_items WHERE content = @content LIMIT 1";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@content", content);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void ShowNotification(string message)
        {
            // Check if notifications are enabled in settings
            try
            {
                string settingsPath = "settings.json";
                if (File.Exists(settingsPath))
                {
                    string json = File.ReadAllText(settingsPath);
                    var settings = System.Text.Json.JsonSerializer.Deserialize<AppSettings>(json);
                    if (settings != null && !settings.ShowNotifications)
                    {
                        return; // Don't show notification if disabled
                    }
                }
            }
            catch
            {
                // If there's any error reading settings, show notification by default
            }
            
            notifyIcon.ShowBalloonTip(2000, "Multiple Copy Paste", message, ToolTipIcon.Info);
        }

        private void BtnCopy_Click(object sender, EventArgs e)
        {
            CopySelected();
        }

        private void BtnPaste_Click(object sender, EventArgs e)
        {
            PasteNext();
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            ClearAllItems(sender, e);
        }

        private void BtnDeleteSelected_Click(object sender, EventArgs e)
        {
            // Find the ListBox control more reliably
            ListBox listBox = null;
            foreach (Control control in this.Controls)
            {
                if (control is ListBox)
                {
                    listBox = control as ListBox;
                    break;
                }
            }

            if (listBox == null)
            {
                MessageBox.Show("Error: ListBox not found!", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (listBox.SelectedIndex >= 0)
            {
                string selectedItem = listBox.SelectedItem.ToString();
                string preview = selectedItem.Length > 50 ? selectedItem.Substring(0, 50) + "..." : selectedItem;
                
                if (MessageBox.Show($"Are you sure you want to delete this item?\n\n{preview}", 
                    "Delete Selected Item", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    // Remove from memory list
                    copiedItems.Remove(selectedItem);
                    
                    // Remove from database
                    RemoveFromDatabase(selectedItem);
                    
                    // Refresh the display
                    UpdateListBox();
                    
                    ShowNotification("Selected item deleted successfully!");
                }
            }
            else
            {
                MessageBox.Show("Please select an item to delete.", "Delete Selected", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnMinimize_Click(object sender, EventArgs e)
        {
            MinimizeToTray();
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
                saveDialog.FilterIndex = 1;
                saveDialog.RestoreDirectory = true;
                saveDialog.FileName = $"copied_items_{DateTime.Now:yyyyMMdd_HHmmss}.json";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    if (dataManager.ExportToJson(saveDialog.FileName))
                    {
                        ShowNotification($"Data exported successfully to {Path.GetFileName(saveDialog.FileName)}");
                    }
                }
            }
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openDialog = new OpenFileDialog())
            {
                openDialog.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
                openDialog.FilterIndex = 1;
                openDialog.RestoreDirectory = true;

                if (openDialog.ShowDialog() == DialogResult.OK)
                {
                    if (dataManager.ImportFromJson(openDialog.FileName))
                    {
                        LoadCopiedItems();
                        ShowNotification($"Data imported successfully from {Path.GetFileName(openDialog.FileName)}");
                    }
                }
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            var allItems = dataManager.GetAllItems();
            if (allItems.Count == 0)
            {
                MessageBox.Show("No items to search. Please copy some items first.", "Search", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var searchForm = new SearchForm(allItems);
            searchForm.ShowDialog(this);
        }

        private void BtnStats_Click(object sender, EventArgs e)
        {
            var statsForm = new StatisticsForm(dbPath);
            statsForm.ShowDialog(this);
        }

        private void BtnSettings_Click(object sender, EventArgs e)
        {
            var settingsForm = new SettingsForm();
            settingsForm.ShowDialog(this);
        }

        private void BtnAbout_Click(object sender, EventArgs e)
        {
            var aboutForm = new AboutForm();
            aboutForm.ShowDialog(this);
        }

        private async void CreateBackup()
        {
            bool success = await BackupManager.CreateBackupAsync(dbPath);
            if (success)
            {
                ShowNotification("Database backup created successfully!");
            }
        }

        private void ListBoxItems_DoubleClick(object sender, EventArgs e)
        {
            ListBox listBox = (ListBox)sender;
            if (listBox.SelectedIndex >= 0 && listBox.SelectedIndex < copiedItems.Count)
            {
                string selectedItem = copiedItems[listBox.SelectedIndex];
                Clipboard.SetText(selectedItem);
                ShowNotification("Item copied to clipboard!");
            }
        }

        private void ShowMainWindow(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.Activate();
            isMinimized = false;
        }

        private void MinimizeToTray()
        {
            this.Hide();
            isMinimized = true;
            ShowNotification("Application minimized to system tray");
        }

        private void ClearAllItems(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to clear all copied items?", 
                "Confirm Clear", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                copiedItems.Clear();
                UpdateListBox();
                
                // Clear database
                using (SQLiteConnection conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
                {
                    conn.Open();
                    string sql = "DELETE FROM copied_items";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
                
                ShowNotification("All items cleared!");
            }
        }

        private void ExitApplication(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to exit?", 
                "Confirm Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                UnregisterHotKey(this.Handle, HOTKEY_F1);
                UnregisterHotKey(this.Handle, HOTKEY_F2);
                notifyIcon.Visible = false;
                Application.Exit();
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                MinimizeToTray();
            }
            else
            {
                UnregisterHotKey(this.Handle, HOTKEY_F1);
                UnregisterHotKey(this.Handle, HOTKEY_F2);
                UnregisterHotKey(this.Handle, 3);
                UnregisterHotKey(this.Handle, 4);
                UnregisterHotKey(this.Handle, 5);
                UnregisterHotKey(this.Handle, 6);
                notifyIcon.Visible = false;
            }
            base.OnFormClosing(e);
        }
    }
} 