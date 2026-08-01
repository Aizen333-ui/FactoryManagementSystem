using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SalesDashboard.Pages
{
    partial class SalesHistory
    {
        private System.ComponentModel.IContainer components = null;

        // ==============================
        // Search and Filter Controls
        // ==============================

        // Text input for searching sales records
        private TextBox txtSearch;

        // Dropdown filter for payment status
        private ComboBox cmbPaymentStatus;

        // ==============================
        // Sales Data Display
        // ==============================

        // Grid displaying previous sales records
        private DataGridView dgvSalesHistory;

        // ==============================
        // Action Buttons
        // ==============================

        // Executes search/filter operation
        private Button btnSearch;

        // Clears search and restores default view
        private Button btnClear;

        // Returns user back to previous page
        private Button btnBack;

        // Applies modern flat rounded style to buttons
        private void MakeRoundedButton(Button btn, Color color)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = color;
            btn.ForeColor = Color.White;
            btn.Font = new Font("Segoe UI", 13F, FontStyle.Bold);

            // Creates rounded button corners
            ApplyRoundedRegion(btn, 18);
        }

        // Creates rounded region shape for buttons

        private void ApplyRoundedRegion(Button btn, int radius)
        {
            Rectangle rect = btn.ClientRectangle;

            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
                path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
                path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
                path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);

                path.CloseFigure();

                btn.Region = new Region(path);
            }
        }

        // Creates a rounded white container around input controls
        // Used for TextBox and ComboBox styling

        private Panel CreateRoundedBox(Control inner)
        {
            Panel container = new Panel
            {
                Width = inner.Width > 0 ? inner.Width + 24 : 420,
                Height = 50,
                BackColor = Color.White,
                Margin = new Padding(0, 0, 15, 15)
            };

            // Styling for search textbox
            if (inner is TextBox tb)
            {
                tb.BorderStyle = BorderStyle.None;
                tb.Font = new Font("Segoe UI", 13F);

                tb.Size = new Size(
                    container.Width - 24,
                    tb.PreferredHeight
                );

                tb.Location = new Point(
                    12,
                    (container.Height - tb.PreferredHeight) / 2
                );
            }

            // Styling for payment status dropdown
            else if (inner is ComboBox cb)
            {
                cb.FlatStyle = FlatStyle.Flat;
                cb.DropDownStyle = ComboBoxStyle.DropDownList;
                cb.Font = new Font("Segoe UI", 13F);

                cb.Width = container.Width - 24;
                cb.Height = 36;

                cb.Location = new Point(
                    12,
                    (container.Height - cb.Height) / 2
                );
            }

            container.Controls.Add(inner);

            // Draws rounded border around input container
            container.Paint += (s, e) =>
            {
                using GraphicsPath path = new GraphicsPath();

                Rectangle rect = new Rectangle(
                    1,
                    1,
                    container.Width - 2,
                    container.Height - 2
                );

                int radius = 12;

                path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
                path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
                path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
                path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);

                path.CloseFigure();

                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                using Pen pen = new Pen(Color.LightGray, 1.5f);
                e.Graphics.DrawPath(pen, path);
            };

            return container;
        }

        // ==================================================
        // BUILD SALES HISTORY PAGE UI
        // ==================================================
        //
        // Creates:
        // 1. Page title
        // 2. Search and filter controls
        // 3. Sales history DataGridView
        // 4. Back navigation button
        //
        // Database loading/filter logic is handled
        // separately inside SalesHistory.cs
        //
        // ==================================================

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Main vertical layout container
            FlowLayoutPanel main = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                Padding = new Padding(50, 30, 50, 30)
            };

            // Page heading
            Label title = new Label
            {
                Text = "Sales History",
                Font = new Font("Segoe UI", 22F, FontStyle.Bold),
                AutoSize = true
            };

            main.Controls.Add(title);


            // ==============================
            // Search and Filter Section
            // ==============================

            FlowLayoutPanel filterRow = new FlowLayoutPanel
            {
                Width = 1200,
                Height = 70,
                Margin = new Padding(0, 20, 0, 15)
            };

            // Search textbox
            txtSearch = new TextBox
            {
                Width = 420
            };

            filterRow.Controls.Add(
                CreateRoundedBox(txtSearch)
            );

            // Payment status filter dropdown
            cmbPaymentStatus = new ComboBox
            {
                Width = 260
            };

            cmbPaymentStatus.Items.AddRange(new object[]
            {
                "All Statuses",
                "Paid",
                "Pending",
                "Partial"
            });

            cmbPaymentStatus.SelectedIndex = 0;

            filterRow.Controls.Add(
                CreateRoundedBox(cmbPaymentStatus)
            );

            // Search button
            btnSearch = new Button
            {
                Text = "Search",
                Width = 160,
                Height = 50
            };

            MakeRoundedButton(
                btnSearch,
                Color.FromArgb(94, 60, 255)
            );

            filterRow.Controls.Add(btnSearch);

            // Clear filter button
            btnClear = new Button
            {
                Text = "Clear",
                Width = 160,
                Height = 50
            };

            MakeRoundedButton(
                btnClear,
                Color.FromArgb(107, 114, 128)
            );

            filterRow.Controls.Add(btnClear);

            main.Controls.Add(filterRow);

            // ==============================
            // Sales History Data Grid
            // ==============================

            dgvSalesHistory = new DataGridView
            {
                Width = 1300,
                Height = 520,

                // Prevent editing sales history
                ReadOnly = true,

                // Disable empty row creation
                AllowUserToAddRows = false,

                // Hide default row header column
                RowHeadersVisible = false,

                // Automatically resize columns
                AutoSizeColumnsMode =
                    DataGridViewAutoSizeColumnsMode.Fill
            };

            main.Controls.Add(dgvSalesHistory);

            // ==============================
            // Navigation Button
            // ==============================

            btnBack = new Button
            {
                Text = "Back",
                Width = 180,
                Height = 50,
                Margin = new Padding(0, 20, 0, 0)
            };

            MakeRoundedButton(
                btnBack,
                Color.Gray
            );

            main.Controls.Add(btnBack);

            // Add complete UI layout
            // into the SalesHistory UserControl
            this.Controls.Add(main);

            this.ResumeLayout(false);
        }
    }
}