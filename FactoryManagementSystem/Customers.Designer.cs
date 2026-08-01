using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SalesDashboard.Pages
{
    partial class Customers
    {
        private System.ComponentModel.IContainer components = null;

        // ============================================================
        // UI Controls
        //
        // Contains controls used for Customer Management page:
        // - Customer information fields
        // - Search controls
        // - Customer data grid
        // - Navigation button
        // ============================================================

        private TextBox txtCustomerName;
        private TextBox txtAddress;
        private TextBox txtPhone;
        private TextBox txtSearchCustomer;
        private Button btnAddCustomer;
        private Button btnEditCustomer;
        private Button btnSearchCustomer;
        private DataGridView dgvCustomers;
        private Button btnBack;

        // ============================================================
        // Creates a rounded container around input controls.
        //
        // Provides:
        // - Custom border
        // - Padding
        // - Modern textbox appearance
        //
        // Used for customer input and search fields.
        // ============================================================

        private Panel CreateRoundedBox(Control inner)
        {
            Panel container =
                new Panel
                {
                    Width = inner.Width + 24,
                    Height = inner.Height + 28,
                    BackColor = Color.White
                };

            // Center control inside container
            inner.Location =
                new Point(
                    12,
                    12);

            // Apply textbox styling
            if (inner is TextBox tb)
            {
                tb.BorderStyle = BorderStyle.None;
                tb.Font = new Font("Segoe UI", 13F);
                tb.BackColor = Color.White;
            }

            container.Controls.Add(inner);

            // Draw rounded border
            container.Paint += (s, e) =>
            {
                int radius = 12;

                Rectangle rect = new Rectangle(0, 0, container.Width - 1, container.Height - 1);

                using GraphicsPath path = new GraphicsPath();

                path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
                path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
                path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
                path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);

                path.CloseFigure();

                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                using Pen pen = new Pen(Color.FromArgb(180, 190, 210), 1.5f);

                e.Graphics.DrawPath(pen, path);
            };

            return container;
        }

        // ============================================================
        // Applies modern rounded styling to buttons.
        //
        // Used for:
        // - Add Customer
        // - Update Customer
        // - Search
        // - Back
        // ============================================================

        private void MakeRoundedButton(
            Button btn,
            Color color)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = color;
            btn.ForeColor = Color.White;
            btn.Font = new Font("Segoe UI", 13F, FontStyle.Bold);

            GraphicsPath path = new GraphicsPath();

            int radius = 18;

            path.AddArc(0, 0, radius, radius, 180, 90);
            path.AddArc(btn.Width - radius, 0, radius, radius, 270, 90);
            path.AddArc(btn.Width - radius, btn.Height - radius, radius, radius, 0, 90);
            path.AddArc(0, btn.Height - radius, radius, radius, 90, 90);

            path.CloseFigure();

            btn.Region = new Region(path);
        }

        // ============================================================
        // Initializes Customer Management UI.
        //
        // Creates:
        // - Page title
        // - Customer input fields
        // - Action buttons
        // - Search section
        // - Customer DataGridView
        // - Back navigation button
        //
        // Also applies styling and adds controls to the page.
        // ============================================================

        private void InitializeComponent()
        {
            SuspendLayout();

            // ========================================================
            // Main Layout Container
            //
            // Uses FlowLayoutPanel to automatically arrange controls
            // vertically and provide scrolling support.
            // ========================================================

            FlowLayoutPanel main =
                new FlowLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    FlowDirection = FlowDirection.TopDown,
                    WrapContents = false,
                    AutoScroll = true,
                    Padding = new Padding(40, 20, 40, 30),
                    BackColor = Color.White
                };

            // ========================================================
            // Page Title
            // ========================================================

            Label title =
                new Label
                {
                    Text = "Customer Management",
                    Font = new Font("Segoe UI", 22F, FontStyle.Bold),
                    AutoSize = true,
                    Margin = new Padding(0, 0, 0, 20)
                };

            main.Controls.Add(title);

            // ========================================================
            // Customer Name Section
            // ========================================================

            Label lblName =
                new Label
                {
                    Text = "Customer Name:",
                    Font = new Font("Segoe UI", 14F),
                    AutoSize = true
                };

            main.Controls.Add(lblName);

            txtCustomerName =
                new TextBox
                {
                    Width = 500,
                    Height = 35
                };

            main.Controls.Add(
                CreateRoundedBox(
                    txtCustomerName));

            // ========================================================
            // Address Section
            // ========================================================

            Label lblAddress =
                new Label
                {
                    Text = "Address:",
                    Font = new Font("Segoe UI", 14F),
                    AutoSize = true,
                    Margin = new Padding(0, 15, 0, 0)
                };

            main.Controls.Add(lblAddress);

            txtAddress =
                new TextBox
                {
                    Width = 500,
                    Height = 35,
                    ScrollBars = ScrollBars.Vertical
                };

            main.Controls.Add(
                CreateRoundedBox(
                    txtAddress));

            // ========================================================
            // Phone Number Section
            // ========================================================

            Label lblPhone =
                new Label
                {
                    Text = "Phone Number:",
                    Font = new Font("Segoe UI", 14F),
                    AutoSize = true,
                    Margin = new Padding(0, 15, 0, 0)
                };

            main.Controls.Add(lblPhone);

            txtPhone =
                new TextBox
                {
                    Width = 500,
                    Height = 35
                };

            main.Controls.Add(
                CreateRoundedBox(
                    txtPhone));

            // ========================================================
            // Customer Action Buttons
            //
            // Provides:
            // - Add Customer
            // - Update Customer
            // ========================================================

            FlowLayoutPanel buttonRow =
                new FlowLayoutPanel
                {
                    Width = 900,
                    Height = 65,
                    Margin = new Padding(0, 25, 0, 20)
                };

            btnAddCustomer =
                new Button
                {
                    Text =
                        "Add Customer",
                    Width = 220,
                    Height = 50
                };

            btnEditCustomer =
                new Button
                {
                    Text =
                        "Update Customer",
                    Width = 220,
                    Height = 50
                };
            MakeRoundedButton(btnAddCustomer, Color.FromArgb(34, 197, 94));

            MakeRoundedButton(btnEditCustomer, Color.FromArgb(59, 130, 246));

            buttonRow.Controls.Add(btnAddCustomer);
            buttonRow.Controls.Add(btnEditCustomer);
            main.Controls.Add(buttonRow);

            // ========================================================
            // Customer Search Section
            //
            // Allows filtering customers from database/list.
            // ========================================================

            Label lblSearch =
                new Label
                {
                    Text = "Search Customer:",
                    Font = new Font("Segoe UI", 14F),
                    AutoSize = true
                };

            main.Controls.Add(lblSearch);

            FlowLayoutPanel searchRow =
                new FlowLayoutPanel
                {
                    Width = 800,
                    Height = 80
                };

            txtSearchCustomer =
                new TextBox
                {
                    Width = 400,
                    Height = 35
                };

            btnSearchCustomer =
                new Button
                {
                    Text =
                        "Search",
                    Width = 120,
                    Height = 50
                };

            btnSearchCustomer.Margin = new Padding(40, 10, 0, 0);

            MakeRoundedButton(btnSearchCustomer, Color.FromArgb(99, 102, 241));

            searchRow.Controls.Add(CreateRoundedBox(txtSearchCustomer));
            searchRow.Controls.Add(btnSearchCustomer);

            main.Controls.Add(searchRow);
            // ========================================================
            // Customer DataGridView
            //
            // Displays customer records.
            //
            // Features:
            // - Read-only mode
            // - Full row selection
            // - Auto column sizing
            // - Hidden row headers
            // ========================================================

            dgvCustomers =
                new DataGridView
                {
                    Width = 1100,
                    Height = 300,
                    AllowUserToAddRows = false,
                    ReadOnly = true,
                    RowHeadersVisible = false,
                    AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                    SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                    BackgroundColor = Color.White,
                    Margin = new Padding(0, 20, 0, 0)
                };

            // ========================================================
            // Back Navigation Button
            //
            // Returns user back to Sales Dashboard.
            // ========================================================

            btnBack =
                new Button
                {
                    Text =
                        "Back",
                    Width = 180,
                    Height = 50
                };

            MakeRoundedButton(btnBack, Color.FromArgb(107, 114, 128));

            // Container for bottom navigation area
            Panel bottomPanel =
                new Panel
                {
                    Width = 1100,
                    Height = 70,
                    Margin = new Padding(0, 20, 0, 20)
                };

            btnBack.Location =
                new Point(
                    0,
                    10);

            bottomPanel.Controls.Add(
                btnBack);

            // ========================================================
            // DataGridView Columns
            //
            // Defines displayed customer information columns.
            //
            // These can later be replaced with database binding.
            // ========================================================

            dgvCustomers.Columns.Add(
                "CustomerName",
                "Customer Name");

            dgvCustomers.Columns.Add(
                "Phone",
                "Phone");

            dgvCustomers.Columns.Add(
                "Address",
                "Address");

            // ========================================================
            // Add Controls To Main Layout
            //
            // Controls are added in display order.
            // ========================================================

            main.Controls.Add(dgvCustomers);
                
            main.Controls.Add(bottomPanel);
                
            // Add main layout to UserControl
            Controls.Add(main);

            // ========================================================
            // UserControl Settings
            // ========================================================

            ClientSize = new Size(1200, 850);
            BackColor = Color.White;

            ResumeLayout(false);
        }
    }
}