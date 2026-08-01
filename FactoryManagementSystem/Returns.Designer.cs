using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SalesDashboard.Pages
{
    partial class Returns
    {
        private System.ComponentModel.IContainer components = null;


        // ================= UI CONTROLS =================

        // Invoice search controls
        private TextBox txtInvoiceNumber;
        private Button btnSearchInvoice;
        private Button btnClearInvoice;

        // Invoice items and return history grids
        private DataGridView dgvReturnItems;
        private DataGridView dgvReturnHistory;

        // Return detail controls
        private TextBox txtReturnQty;
        private ComboBox cmbReturnReason;
        private TextBox txtNotes;
        private TextBox txtReturnAmount;

        // Refund controls
        private ComboBox cmbRefundMethod;
        private TextBox txtRefundAmount;

        // Action buttons and sale information labels
        private Button btnProcessReturn;
        private Button btnBack;

        private Label lblInvoiceNo;
        private Label lblCustomerName;
        private Label lblSaleDate;
        private Label lblPaymentMethod;
        private Label lblOriginalTotal;

        private void MakeRoundedButton(Button btn, Color color)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;

            btn.BackColor = color;
            btn.ForeColor = Color.White;

            btn.Font =
                new Font(
                    "Segoe UI",
                    13F,
                    FontStyle.Bold);

            ApplyRoundedRegion(btn, 18);
        }

        private void ApplyRoundedRegion(Button btn, int radius)
        {
            Rectangle rect =
                btn.ClientRectangle;

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

        private Panel CreateRoundedBox(Control inner)
        {
            Panel box =
            new Panel
            {
                Width = inner.Width + 24,
                Height = 50,
                BackColor = Color.White
            };

            inner.Location =
                new Point(
                    12,
                    (box.Height - inner.Height) / 2);

            if (inner is ComboBox cb)
            {
                cb.FlatStyle = FlatStyle.Flat;
                cb.Font = new Font("Segoe UI", 13F);
                cb.DropDownStyle = ComboBoxStyle.DropDownList;

                // Remove default margins
                cb.Margin = Padding.Empty;

                // Give the combo a fixed height
                cb.Height = 36;

                // Vertically center it inside the rounded panel
                cb.Location = new Point(
                    12,
                    (box.Height - cb.Height) / 2
                );

                cb.Width = box.Width - 24;
            }
            else
            {
                inner.Location = new Point(12, 12);
            }
            
            if (inner is TextBox tb)
            {
                tb.BorderStyle =
                    BorderStyle.None;

                tb.Font =
                    new Font(
                        "Segoe UI",
                        13F);
            }

            box.Controls.Add(inner);

            if (inner is ComboBox cb2)
            {
                cb2.Location = new Point(
                    12,
                    (box.Height - cb2.Height) / 2
                );
            }

            box.Paint += (s, e) =>
            {
                Rectangle rect = new Rectangle(0, 0, box.Width - 1, box.Height - 1);
                int radius = 12;

                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
                    path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
                    path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
                    path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);

                    path.CloseFigure();

                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                    using Pen pen = new Pen(Color.FromArgb(180, 190, 210), 1.5f);
                    e.Graphics.DrawPath(pen, path);
                }
            };

            return box;
        }

        private void InitializeComponent()
        {
            SuspendLayout();

            this.Size = new Size(1200, 900);
            this.BackColor = Color.White;

            FlowLayoutPanel main =
                new FlowLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    FlowDirection = FlowDirection.TopDown,
                    WrapContents = false,
                    AutoScroll = true,
                    Padding = new Padding(40, 20, 40, 30)
                };

            // TITLE

            Label title =
                new Label
                {
                    Text = "Returns",
                    Font = new Font("Segoe UI", 22F, FontStyle.Bold),
                    AutoSize = true,
                    Margin = new Padding(0, 0, 0, 20)
                };

            main.Controls.Add(title);

            // ================= INVOICE SEARCH =================

            Label lblInvoice =
                new Label
                {
                    Text = "Invoice Number",
                    Font = new Font("Segoe UI", 14F),
                    AutoSize = true
                };

            main.Controls.Add(lblInvoice);

            txtInvoiceNumber =
                new TextBox
                {
                    Width = 500,
                    Height = 35
                };

            FlowLayoutPanel invoiceRow =
                new FlowLayoutPanel
                {
                    Width = 1300,
                    Height = 65
                };

            invoiceRow.Controls.Add(CreateRoundedBox(txtInvoiceNumber));
                
            btnSearchInvoice =
                new Button
                {
                    Text = "Search Invoice",
                    Width =200,
                    Height =50   
                };

            MakeRoundedButton(btnSearchInvoice, Color.FromArgb(94, 60, 255));

            invoiceRow.Controls.Add(btnSearchInvoice);

            btnClearInvoice =
                new Button
                {
                    Text = "Clear",
                    Width = 150,
                    Height = 50
                };

            MakeRoundedButton(
                btnClearInvoice,
                Color.Gray);

            invoiceRow.Controls.Add(
                btnClearInvoice);

            main.Controls.Add(invoiceRow);

            // ================= SALE INFO =================

            Panel saleInfo =
                new Panel
                {
                    Width = 1100,
                    Height = 120,
                    BackColor = Color.White
                };

            Label lblInvoiceTitle = new Label
            {
                Text = "Invoice:",
                Location = new Point(0, 10),
                Font = new Font("Segoe UI", 13F),
                AutoSize = true
            };

            lblInvoiceNo = new Label
            {
                Text = "-",
                Location = new Point(120, 10),
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                AutoSize = true
            };

            Label lblCustomerTitle = new Label
            {
                Text = "Customer:",
                Location = new Point(300, 10),
                Font = new Font("Segoe UI", 13F),
                AutoSize = true
            };

            lblCustomerName = new Label
            {
                Text = "-",
                Location = new Point(430, 10),
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                AutoSize = true
            };

            Label lblDateTitle = new Label
            {
                Text = "Date:",
                Location = new Point(0, 50),
                Font = new Font("Segoe UI", 13F),
                AutoSize = true
            };

            lblSaleDate = new Label
            {
                Text = "-",
                Location = new Point(120, 50),
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                AutoSize = true
            };

            Label lblPaymentTitle = new Label
            {
                Text = "Payment Method:",
                Location = new Point(300, 50),
                Font = new Font("Segoe UI", 13F),
                AutoSize = true
            };

            lblPaymentMethod = new Label
            {
                Text = "-",
                Location = new Point(430, 50),
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                AutoSize = true
            };

            Label lblTotalTitle = new Label
            {
                Text = "Total:",
                Location = new Point(700, 50),
                Font = new Font("Segoe UI", 13F),
                AutoSize = true
            };

            lblOriginalTotal = new Label
            {
                Text = "-",
                Location = new Point(780, 50),
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                AutoSize = true
            };

            saleInfo.Controls.Add(lblInvoiceTitle);
            saleInfo.Controls.Add(lblInvoiceNo);

            saleInfo.Controls.Add(lblCustomerTitle);
            saleInfo.Controls.Add(lblCustomerName);

            saleInfo.Controls.Add(lblDateTitle);
            saleInfo.Controls.Add(lblSaleDate);

            saleInfo.Controls.Add(lblPaymentTitle);
            saleInfo.Controls.Add(lblPaymentMethod);

            saleInfo.Controls.Add(lblTotalTitle);
            saleInfo.Controls.Add(lblOriginalTotal);


            main.Controls.Add(saleInfo);

            // ================= ITEMS GRID =================


            Label lblItems =
                new Label
                {
                    Text = "Invoice Items",
                    Font = new Font("Segoe UI", 15F, FontStyle.Bold),
                    AutoSize = true
                };

            main.Controls.Add(lblItems);

            dgvReturnItems =
                new DataGridView
                {
                    Width = 1100,
                    Height = 220,

                    AllowUserToAddRows =false,
                    ReadOnly =true,
                    RowHeadersVisible =false,
                    SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                    AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                    BackgroundColor =
                        Color.White
                };

            dgvReturnItems.Columns.Add(
                "SaleItemID",
                "SaleItemID");

            dgvReturnItems.Columns["SaleItemID"]
                .Visible = false;

            dgvReturnItems.Columns.Add(
                "ProductionID",
                "ProductionID");

            dgvReturnItems.Columns["ProductionID"]
                .Visible = false;
           
            main.Controls.Add(
                dgvReturnItems);
            // ================= RETURN DETAILS =================

            Label lblReturnDetails =
                new Label
                {
                    Text = "Return Details",
                    Font = new Font("Segoe UI", 15F, FontStyle.Bold),
                    AutoSize = true,
                    Margin = new Padding(0, 25, 0, 10)
                };

            main.Controls.Add(lblReturnDetails);

            Panel returnPanel =
                new Panel
                {
                    Width = 1100,
                    Height = 260
                };

            // Quantity

            Label lblQty =
                new Label
                {
                    Text = "Return Quantity",
                    Font = new Font("Segoe UI", 13F),
                    Location = new Point(0, 0),
                    AutoSize = true
                };

            txtReturnQty =
                new TextBox
                {
                    Width = 300,
                    Height = 35
                };

            Panel qtyBox =
                CreateRoundedBox(
                    txtReturnQty);

            qtyBox.Location =
                new Point(
                    0,
                    50);

            returnPanel.Controls.Add(lblQty);
            returnPanel.Controls.Add(qtyBox);

            // Reason

            Label lblReason =
                new Label
                {
                    Text = "Reason",
                    Font = new Font("Segoe UI", 13F),
                    Location = new Point(370, 0),
                    AutoSize = true
                };

            cmbReturnReason =
                new ComboBox
                {
                    Width = 300,
                    Height = 35,

                    DropDownStyle =
                        ComboBoxStyle.DropDownList
                };

            cmbReturnReason.Items.AddRange(
                new object[]
                {
                    "Select Reason...",
                    "Damaged",
                    "Wrong Product",
                    "Customer Request",
                    "Other"
                });

            cmbReturnReason.SelectedIndex = 0;

            Panel reasonBox =
                CreateRoundedBox(
                    cmbReturnReason);

            reasonBox.Location =
                new Point(
                    370,
                    50);

            returnPanel.Controls.Add(lblReason);
            returnPanel.Controls.Add(reasonBox);

            // Notes

            Label lblNotes =
                new Label
                {
                    Text = "Notes",
                    Font = new Font("Segoe UI", 13F),
                    Location = new Point(0, 105),
                    AutoSize = true
                };

            txtNotes =
                new TextBox
                {
                    Width = 670,
                    Height = 60,
                    ScrollBars =
                        ScrollBars.Vertical
                };

            Panel notesBox =
                CreateRoundedBox(
                    txtNotes);

            notesBox.Location =
                new Point(
                    0,
                    150);

            returnPanel.Controls.Add(lblNotes);
            returnPanel.Controls.Add(notesBox);

            // Return Amount

            Label lblReturnAmount =
                new Label
                {
                    Text = "Return Amount",
                    Font = new Font("Segoe UI", 13F),
                    Location = new Point(750, 105),
                    AutoSize = true
                };

            txtReturnAmount =
                new TextBox
                {
                    Width = 300,
                    Height = 35,
                    ReadOnly = true,
                    BackColor = Color.White,
                    ForeColor = Color.Black
                };

            Panel amountBox =
                CreateRoundedBox(
                    txtReturnAmount);

            amountBox.Location =
                new Point(
                    750,
                    150);

            returnPanel.Controls.Add(lblReturnAmount);
            returnPanel.Controls.Add(amountBox);

            main.Controls.Add(returnPanel);

            // ================= REFUND SECTION =================

            Label lblRefund =
                new Label
                {
                    Text = "Refund Details",
                    Font = new Font("Segoe UI", 15F, FontStyle.Bold),
                    AutoSize = true,
                    Margin = new Padding(0, 20, 0, 10)
                };

            main.Controls.Add(lblRefund);

            Panel refundPanel =
                new Panel
                {
                    Width = 1100,
                    Height = 130
                };

            Label lblRefundMethod =
                new Label
                {
                    Text = "Refund Method",
                    Font = new Font("Segoe UI", 13F),
                    Location = new Point(0, 0),
                    AutoSize = true
                };

            cmbRefundMethod =
                new ComboBox
                {
                    Width = 300,
                    Height = 35,
                    DropDownStyle =
                        ComboBoxStyle.DropDownList
                };

            cmbRefundMethod.Items.AddRange(
                 new object[]
                 {
                    "Select Refund Method...",
                    "Cash",
                    "Bank Transfer",
                    "Credit Adjustment"
                 });

            cmbRefundMethod.SelectedIndex = 0;

            Panel refundMethodBox = CreateRoundedBox(cmbRefundMethod);

            refundMethodBox.Location = new Point(0, 50);

            refundPanel.Controls.Add(lblRefundMethod);
            refundPanel.Controls.Add(refundMethodBox);

            Label lblRefundAmount =
                new Label
                {
                    Text = "Refund Amount",
                    Font = new Font("Segoe UI", 13F),
                    Location = new Point(400, 0),
                    AutoSize = true
                };

            txtRefundAmount =
                new TextBox
                {
                    Width = 300,
                    Height = 35,
                    ReadOnly = true,
                    BackColor = Color.White,
                    ForeColor = Color.Black
                };

            Panel refundAmountBox = CreateRoundedBox(txtRefundAmount);

            refundAmountBox.Location = new Point(400, 50);

            refundPanel.Controls.Add(lblRefundAmount);
            refundPanel.Controls.Add(refundAmountBox);

            btnProcessReturn =
                new Button
                {
                    Text = "Process Return",
                    Width = 220,
                    Height = 50,
                    Location = new Point(800, 30)
                };

            MakeRoundedButton(
                btnProcessReturn,
                Color.FromArgb(
                    220,
                    38,
                    38));



            refundPanel.Controls.Add(btnProcessReturn);
                
            main.Controls.Add(refundPanel);

            // ================= RETURN HISTORY =================

            Label lblHistory =
                new Label
                {
                    Text = "Return History",
                    Font = new Font("Segoe UI", 15F, FontStyle.Bold),
                    AutoSize = true,
                    Margin = new Padding(0, 20, 0, 10)
                };

            main.Controls.Add(lblHistory);

            dgvReturnHistory =
                new DataGridView
                {
                    Width = 1100,
                    Height = 220,
                    AllowUserToAddRows = false,
                    ReadOnly = true,
                    RowHeadersVisible = false,
                    AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                    BackgroundColor = Color.White
                };

            main.Controls.Add(
                dgvReturnHistory);

            // ================= BACK BUTTON =================

            btnBack =
                new Button
                {
                    Text = "Back",
                    Width = 180,
                    Height = 50,
                    Margin = new Padding(0, 20, 0, 20)
                };

            MakeRoundedButton(
                btnBack,
                Color.Gray);

            main.Controls.Add(
                btnBack);

            this.Controls.Add(main);

            ResumeLayout(false);
        }
    }
}