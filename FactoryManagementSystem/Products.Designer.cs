using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SalesDashboard.Pages
{
    partial class Products
    {
        private System.ComponentModel.IContainer components = null;

        // Search and filter controls
        private TextBox txtSearch;
        private ComboBox cmbFilter;

        // Product listing table
        private DataGridView dgvProducts;

        // Action buttons
        private Button btnSearch;
        private Button btnBack;
        private Button btnClear;

        // Applies common styling to buttons with rounded corners
        private void MakeRoundedButton(Button btn, Color color)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = color;
            btn.ForeColor = Color.White;
            btn.Font = new Font("Segoe UI", 13F, FontStyle.Bold);

            // Creates rounded shape for button
            ApplyRoundedRegion(btn, 18);
        }

        // Creates rounded button region using GraphicsPath
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

        // Creates a white rounded container around input controls
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

            // Custom styling for search textbox
            if (inner is TextBox tb)
            {
                tb.BorderStyle = BorderStyle.None;
                tb.Font = new Font("Segoe UI", 13F);
                tb.Size = new Size(container.Width - 24, tb.PreferredHeight
                );
                tb.Location = new Point(
                    12,
                    (container.Height - tb.PreferredHeight) / 2
                );
            }

            // Custom styling for filter dropdown
            else if (inner is ComboBox cb)
            {
                cb.FlatStyle = FlatStyle.Flat;
                cb.DropDownStyle = ComboBoxStyle.DropDownList;
                cb.Font = new Font("Segoe UI", 13F);
                cb.Width = container.Width - 24;
                cb.Height = 36;
                cb.Location = new Point(12,(container.Height - cb.Height) / 2);
            }

            container.Controls.Add(inner);

            // Draw rounded border around input container
            container.Paint += (s, e) =>
            {
                using GraphicsPath path = new GraphicsPath();

                Rectangle rect = new Rectangle(1,1,container.Width - 2,container.Height - 2 );

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

        // Initializes all UI components for Products page
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

            // Page title
            Label title = new Label
            {
                Text = "Products",
                Font = new Font("Segoe UI", 22F, FontStyle.Bold),
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 10)
            };

            main.Controls.Add(title);

            // Page description
            Label subtitle = new Label
            {
                Text = "View product catalog and stock levels.",
                Font = new Font("Segoe UI", 11F),
                ForeColor = Color.Gray,
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 20)
            };

            main.Controls.Add(subtitle);

            // Search and filter controls row
            FlowLayoutPanel filterRow = new FlowLayoutPanel
            {
                Width = 1200,
                Height = 70,
                Margin = new Padding(0, 0, 0, 15)
            };

            // Product search textbox
            txtSearch = new TextBox
            {
                Width = 420
            };

            filterRow.Controls.Add(CreateRoundedBox(txtSearch));

            // Product stock filter dropdown
            cmbFilter = new ComboBox
            {
                Width = 260
            };

            cmbFilter.Items.AddRange(new object[]
            {
                "All Products",
                "In Stock",
                "Low Stock"
            });

            cmbFilter.SelectedIndex = 0;

            filterRow.Controls.Add(CreateRoundedBox(cmbFilter));

            // Search button
            btnSearch = new Button
            {
                Text = "Search",
                Width = 160,
                Height = 50
            };

            MakeRoundedButton(btnSearch, Color.FromArgb(94, 60, 255));

            filterRow.Controls.Add(btnSearch);

            // Clear filters button
            btnClear = new Button
            {
                Text = "Clear",
                Width = 160,
                Height = 50
            };

            MakeRoundedButton(btnClear, Color.FromArgb(107, 114, 128));

            filterRow.Controls.Add(btnClear);

            main.Controls.Add(filterRow);

            // Product data display grid
            dgvProducts = new DataGridView
            {
                Width = 1160,
                Height = 520,

                // User can only view data
                ReadOnly = true,

                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,

                RowHeadersVisible = false,

                // Automatically adjusts columns according to available space
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            main.Controls.Add(dgvProducts);

            // Navigation button
            btnBack = new Button
            {
                Text = "Back",
                Width = 180,
                Height = 50,
                Margin = new Padding(0, 20, 0, 0)
            };

            MakeRoundedButton(btnBack, Color.Gray);

            main.Controls.Add(btnBack);

            // Add main layout to UserControl/Form
            this.Controls.Add(main);

            this.ResumeLayout(false);
        }
    }
}