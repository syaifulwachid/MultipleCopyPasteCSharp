using System;
using System.Drawing;
using System.Windows.Forms;

namespace MultipleCopyPaste
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Form settings
            this.Text = "About - Multiple Copy Paste";
            this.Size = new Size(700, 650);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Title
            Label lblTitle = new Label
            {
                Text = "Multiple Copy Paste Manager",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.DarkBlue,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 60
            };

            // Main Panel
            Panel mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(25),
                AutoScroll = true
            };

            // Version Info
            Label lblVersion = new Label
            {
                Text = "Version 1.0.0",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.Gray,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(25, 25),
                Size = new Size(640, 30)
            };

            // Developer Info
            GroupBox grpDeveloper = new GroupBox
            {
                Text = "Developer Information",
                Location = new Point(25, 70),
                Size = new Size(640, 130),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            Label lblDeveloper = new Label
            {
                Text = "Dibuat oleh: Syaiful Wachid",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Location = new Point(15, 30),
                Size = new Size(610, 25),
                ForeColor = Color.DarkGreen
            };

            Label lblPosition = new Label
            {
                Text = "Senior Project Designer: Fiberhome Indonesia",
                Font = new Font("Segoe UI", 10),
                Location = new Point(15, 60),
                Size = new Size(610, 25),
                ForeColor = Color.DarkBlue
            };

            Label lblLinkedIn = new Label
            {
                Text = "LinkedIn Profile: https://www.linkedin.com/in/syaiful-wachid-5373n/",
                Font = new Font("Segoe UI", 9),
                Location = new Point(15, 90),
                Size = new Size(610, 25),
                ForeColor = Color.Blue,
                Cursor = Cursors.Hand
            };
            lblLinkedIn.Click += (s, e) => System.Diagnostics.Process.Start("https://www.linkedin.com/in/syaiful-wachid-5373n/");

            grpDeveloper.Controls.AddRange(new Control[] { lblDeveloper, lblPosition, lblLinkedIn });

            // Features Info
            GroupBox grpFeatures = new GroupBox
            {
                Text = "Key Features",
                Location = new Point(20, 190),
                Size = new Size(540, 240),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            string[] features = {
                "• F1: Copy selected text to multiple clipboard",
                "• F2: Paste next item from clipboard queue",
                "• F4: Search items with advanced interface",
                "• F5: View usage statistics and analytics",
                "• F6: Open settings and preferences",
                "• F7: Show about information",
                "• System tray integration with context menu",
                "• SQLite database for persistent storage",
                "• Export/Import data in JSON format",
                "• Auto-backup with file rotation",
                "• Auto-startup with Windows option",
                "• Modern Windows Forms interface"
            };

            for (int i = 0; i < features.Length; i++)
            {
                Label lblFeature = new Label
                {
                    Text = features[i],
                    Font = new Font("Segoe UI", 9),
                    Location = new Point(10, 25 + (i * 18)),
                    Size = new Size(520, 18),
                    ForeColor = Color.Black
                };
                grpFeatures.Controls.Add(lblFeature);
            }

            // Technical Info
            GroupBox grpTechnical = new GroupBox
            {
                Text = "Technical Information",
                Location = new Point(20, 440),
                Size = new Size(540, 100),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            Label lblFramework = new Label
            {
                Text = "Framework: .NET 6.0 Windows",
                Font = new Font("Segoe UI", 9),
                Location = new Point(10, 25),
                Size = new Size(520, 18)
            };

            Label lblDatabase = new Label
            {
                Text = "Database: SQLite 3.x",
                Font = new Font("Segoe UI", 9),
                Location = new Point(10, 45),
                Size = new Size(520, 18)
            };

            Label lblUI = new Label
            {
                Text = "UI Framework: Windows Forms",
                Font = new Font("Segoe UI", 9),
                Location = new Point(10, 65),
                Size = new Size(520, 18)
            };

            grpTechnical.Controls.AddRange(new Control[] { lblFramework, lblDatabase, lblUI });

            // Copyright
            Label lblCopyright = new Label
            {
                Text = "© 2024 Syaiful Wachid. All rights reserved.",
                Font = new Font("Segoe UI", 8),
                ForeColor = Color.Gray,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(20, 550),
                Size = new Size(540, 20)
            };

            // Button Panel
            Panel buttonPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 60,
                BackColor = Color.LightGray
            };

            Button btnClose = new Button
            {
                Text = "Close",
                Location = new Point(250, 15),
                Size = new Size(100, 30),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                BackColor = Color.DarkBlue,
                ForeColor = Color.White
            };
            btnClose.Click += (s, e) => this.Close();

            buttonPanel.Controls.Add(btnClose);

            // Add controls to main panel
            mainPanel.Controls.AddRange(new Control[] { lblVersion, grpDeveloper, grpFeatures, grpTechnical, lblCopyright });

            // Add panels to form
            this.Controls.Add(buttonPanel);
            this.Controls.Add(mainPanel);
            this.Controls.Add(lblTitle);

            this.ResumeLayout(false);
        }
    }
} 