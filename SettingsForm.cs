using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace MultipleCopyPaste
{
    public partial class SettingsForm : Form
    {
        private CheckBox chkStartWithWindows;
        private CheckBox chkMinimizeToTray;
        private CheckBox chkShowNotifications;
        private NumericUpDown numMaxItems;
        private TextBox txtCustomHotkey1;
        private TextBox txtCustomHotkey2;
        private Button btnSave;
        private Button btnCancel;
        private Button btnReset;

        public SettingsForm()
        {
            InitializeComponent();
            LoadSettings();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Form settings
            this.Text = "Settings - Multiple Copy Paste";
            this.Size = new Size(600, 500);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Title
            Label lblTitle = new Label
            {
                Text = "Application Settings",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.DarkBlue,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 50
            };

            // Main Panel
            Panel mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(25)
            };

            // General Settings Group
            GroupBox grpGeneral = new GroupBox
            {
                Text = "General Settings",
                Location = new Point(25, 25),
                Size = new Size(540, 160),
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };

            chkStartWithWindows = new CheckBox
            {
                Text = "Start application with Windows",
                Location = new Point(10, 25),
                Size = new Size(200, 20),
                Font = new Font("Segoe UI", 9)
            };

            chkMinimizeToTray = new CheckBox
            {
                Text = "Minimize to system tray on startup",
                Location = new Point(10, 50),
                Size = new Size(250, 20),
                Font = new Font("Segoe UI", 9)
            };

            chkShowNotifications = new CheckBox
            {
                Text = "Show notification balloons",
                Location = new Point(10, 75),
                Size = new Size(200, 20),
                Font = new Font("Segoe UI", 9)
            };

            CheckBox chkAutoBackup = new CheckBox
            {
                Text = "Enable automatic backup",
                Location = new Point(10, 100),
                Size = new Size(200, 20),
                Font = new Font("Segoe UI", 9)
            };

            grpGeneral.Controls.AddRange(new Control[] { chkStartWithWindows, chkMinimizeToTray, chkShowNotifications, chkAutoBackup });

            // Performance Settings Group
            GroupBox grpPerformance = new GroupBox
            {
                Text = "Performance Settings",
                Location = new Point(20, 170),
                Size = new Size(440, 80),
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };

            Label lblMaxItems = new Label
            {
                Text = "Maximum items to store:",
                Location = new Point(10, 25),
                Size = new Size(150, 20),
                Font = new Font("Segoe UI", 9)
            };

            numMaxItems = new NumericUpDown
            {
                Location = new Point(170, 23),
                Size = new Size(80, 25),
                Minimum = 10,
                Maximum = 1000,
                Value = 100,
                Font = new Font("Segoe UI", 9)
            };

            grpPerformance.Controls.AddRange(new Control[] { lblMaxItems, numMaxItems });

            // Hotkey Settings Group
            GroupBox grpHotkeys = new GroupBox
            {
                Text = "Hotkey Settings (Advanced)",
                Location = new Point(20, 260),
                Size = new Size(440, 80),
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };

            Label lblHotkey1 = new Label
            {
                Text = "Copy Hotkey:",
                Location = new Point(10, 25),
                Size = new Size(80, 20),
                Font = new Font("Segoe UI", 9)
            };

            txtCustomHotkey1 = new TextBox
            {
                Location = new Point(100, 23),
                Size = new Size(100, 25),
                Text = "F1",
                Font = new Font("Segoe UI", 9),
                ReadOnly = true
            };

            Label lblHotkey2 = new Label
            {
                Text = "Paste Hotkey:",
                Location = new Point(220, 25),
                Size = new Size(80, 20),
                Font = new Font("Segoe UI", 9)
            };

            txtCustomHotkey2 = new TextBox
            {
                Location = new Point(310, 23),
                Size = new Size(100, 25),
                Text = "F2",
                Font = new Font("Segoe UI", 9),
                ReadOnly = true
            };

            grpHotkeys.Controls.AddRange(new Control[] { lblHotkey1, txtCustomHotkey1, lblHotkey2, txtCustomHotkey2 });

            // Button Panel
            Panel buttonPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 60,
                BackColor = Color.LightGray
            };

            btnSave = new Button
            {
                Text = "Save Settings",
                Location = new Point(10, 15),
                Size = new Size(100, 30),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                BackColor = Color.Green,
                ForeColor = Color.White
            };
            btnSave.Click += BtnSave_Click;

            btnReset = new Button
            {
                Text = "Reset to Default",
                Location = new Point(120, 15),
                Size = new Size(120, 30),
                Font = new Font("Segoe UI", 9)
            };
            btnReset.Click += BtnReset_Click;

            btnCancel = new Button
            {
                Text = "Cancel",
                Location = new Point(250, 15),
                Size = new Size(80, 30),
                Font = new Font("Segoe UI", 9)
            };
            btnCancel.Click += BtnCancel_Click;

            buttonPanel.Controls.AddRange(new Control[] { btnSave, btnReset, btnCancel });

            // Add controls to main panel
            mainPanel.Controls.AddRange(new Control[] { grpGeneral, grpPerformance, grpHotkeys });

            // Add panels to form
            this.Controls.Add(buttonPanel);
            this.Controls.Add(mainPanel);
            this.Controls.Add(lblTitle);

            this.ResumeLayout(false);
        }

        private void LoadSettings()
        {
            try
            {
                string settingsFile = "settings.json";
                if (File.Exists(settingsFile))
                {
                    string json = File.ReadAllText(settingsFile);
                    var settings = System.Text.Json.JsonSerializer.Deserialize<AppSettings>(json);
                    
                    if (settings != null)
                    {
                        chkStartWithWindows.Checked = settings.StartWithWindows;
                        chkMinimizeToTray.Checked = settings.MinimizeToTray;
                        chkShowNotifications.Checked = settings.ShowNotifications;
                        numMaxItems.Value = settings.MaxItems;
                        txtCustomHotkey1.Text = settings.CopyHotkey;
                        txtCustomHotkey2.Text = settings.PasteHotkey;
                    }
                }

                // Check actual startup status
                bool actualStartupStatus = StartupManager.IsStartupEnabled();
                if (chkStartWithWindows.Checked != actualStartupStatus)
                {
                    chkStartWithWindows.Checked = actualStartupStatus;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading settings: {ex.Message}", "Settings Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void SaveSettings()
        {
            try
            {
                // Handle startup setting
                bool newStartupStatus = chkStartWithWindows.Checked;
                bool currentStartupStatus = StartupManager.IsStartupEnabled();
                
                if (newStartupStatus != currentStartupStatus)
                {
                    StartupManager.ToggleStartup(newStartupStatus);
                }

                var settings = new AppSettings
                {
                    StartWithWindows = chkStartWithWindows.Checked,
                    MinimizeToTray = chkMinimizeToTray.Checked,
                    ShowNotifications = chkShowNotifications.Checked,
                    MaxItems = (int)numMaxItems.Value,
                    CopyHotkey = txtCustomHotkey1.Text,
                    PasteHotkey = txtCustomHotkey2.Text
                };

                string json = System.Text.Json.JsonSerializer.Serialize(settings, new System.Text.Json.JsonSerializerOptions 
                { 
                    WriteIndented = true 
                });
                
                File.WriteAllText("settings.json", json);
                
                MessageBox.Show("Settings saved successfully!", "Settings", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving settings: {ex.Message}", "Settings Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ResetToDefault()
        {
            if (MessageBox.Show("Are you sure you want to reset all settings to default?", 
                "Reset Settings", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                chkStartWithWindows.Checked = false;
                chkMinimizeToTray.Checked = true;
                chkShowNotifications.Checked = true;
                numMaxItems.Value = 100;
                txtCustomHotkey1.Text = "F1";
                txtCustomHotkey2.Text = "F2";
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            SaveSettings();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            ResetToDefault();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }

}

public class AppSettings
{
    public bool StartWithWindows { get; set; } = false;
    public bool MinimizeToTray { get; set; } = true;
    public bool ShowNotifications { get; set; } = true;
    public int MaxItems { get; set; } = 100;
    public string CopyHotkey { get; set; } = "F1";
    public string PasteHotkey { get; set; } = "F2";
} 