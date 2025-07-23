using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MultipleCopyPaste
{
    public partial class SearchForm : Form
    {
        private List<CopiedItem> allItems;
        private ListBox listBoxResults;
        private TextBox txtSearch;
        private Button btnSearch;
        private Button btnCopy;
        private Button btnClose;

        public SearchForm(List<CopiedItem> items)
        {
            allItems = items;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Form settings
            this.Text = "Search Copied Items - Syaiful Wachid";
            this.Size = new Size(700, 500);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Search Panel
            Panel searchPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.LightBlue
            };

            Label lblSearch = new Label
            {
                Text = "Search Items:",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(10, 20),
                Size = new Size(100, 20)
            };

            txtSearch = new TextBox
            {
                Location = new Point(120, 18),
                Size = new Size(300, 25),
                Font = new Font("Segoe UI", 10)
            };
            txtSearch.KeyPress += TxtSearch_KeyPress;

            btnSearch = new Button
            {
                Text = "Search",
                Location = new Point(430, 17),
                Size = new Size(80, 28),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                BackColor = Color.DarkBlue,
                ForeColor = Color.White
            };
            btnSearch.Click += BtnSearch_Click;

            Button btnClear = new Button
            {
                Text = "Clear",
                Location = new Point(520, 17),
                Size = new Size(80, 28),
                Font = new Font("Segoe UI", 9)
            };
            btnClear.Click += BtnClear_Click;

            searchPanel.Controls.AddRange(new Control[] { lblSearch, txtSearch, btnSearch, btnClear });

            // Results Panel
            Panel resultsPanel = new Panel
            {
                Dock = DockStyle.Fill
            };

            Label lblResults = new Label
            {
                Text = "Search Results:",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(10, 10),
                Size = new Size(120, 20)
            };

            listBoxResults = new ListBox
            {
                Location = new Point(10, 35),
                Size = new Size(660, 350),
                Font = new Font("Consolas", 9),
                SelectionMode = SelectionMode.One
            };
            listBoxResults.DoubleClick += ListBoxResults_DoubleClick;

            resultsPanel.Controls.AddRange(new Control[] { lblResults, listBoxResults });

            // Button Panel
            Panel buttonPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 50,
                BackColor = Color.LightGray
            };

            btnCopy = new Button
            {
                Text = "Copy Selected",
                Location = new Point(10, 12),
                Size = new Size(120, 30),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                BackColor = Color.Green,
                ForeColor = Color.White
            };
            btnCopy.Click += BtnCopy_Click;

            btnClose = new Button
            {
                Text = "Close",
                Location = new Point(140, 12),
                Size = new Size(80, 30),
                Font = new Font("Segoe UI", 9)
            };
            btnClose.Click += BtnClose_Click;

            buttonPanel.Controls.AddRange(new Control[] { btnCopy, btnClose });

            // Add panels to form
            this.Controls.Add(resultsPanel);
            this.Controls.Add(buttonPanel);
            this.Controls.Add(searchPanel);

            this.ResumeLayout(false);
        }

        private void TxtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13) // Enter key
            {
                e.Handled = true;
                PerformSearch();
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            PerformSearch();
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            listBoxResults.Items.Clear();
        }

        private void BtnCopy_Click(object sender, EventArgs e)
        {
            CopySelectedItem();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ListBoxResults_DoubleClick(object sender, EventArgs e)
        {
            CopySelectedItem();
        }

        private void PerformSearch()
        {
            string searchTerm = txtSearch.Text.Trim().ToLower();
            
            if (string.IsNullOrEmpty(searchTerm))
            {
                MessageBox.Show("Please enter a search term.", "Search", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            listBoxResults.Items.Clear();
            
            var results = allItems.Where(item => 
                item.Content.ToLower().Contains(searchTerm))
                .ToList();

            if (results.Count == 0)
            {
                listBoxResults.Items.Add("No items found matching your search.");
                return;
            }

            foreach (var item in results)
            {
                string displayText = item.Content;
                if (displayText.Length > 80)
                    displayText = displayText.Substring(0, 77) + "...";
                
                listBoxResults.Items.Add($"[{item.Timestamp:yyyy-MM-dd HH:mm}] {displayText}");
            }

            // Store the actual items in Tag for easy access
            listBoxResults.Tag = results;
        }

        private void CopySelectedItem()
        {
            if (listBoxResults.SelectedIndex >= 0 && listBoxResults.Tag is List<CopiedItem> results)
            {
                var selectedItem = results[listBoxResults.SelectedIndex];
                Clipboard.SetText(selectedItem.Content);
                
                MessageBox.Show($"Item copied to clipboard!\n\nContent: {selectedItem.Content.Substring(0, Math.Min(50, selectedItem.Content.Length))}...", 
                    "Copied", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
} 