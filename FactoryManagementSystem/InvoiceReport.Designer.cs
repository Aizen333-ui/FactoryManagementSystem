using System.Drawing;
using System.Windows.Forms;

namespace SalesDashboard.Pages
{
    partial class InvoiceReport
    {
        private System.ComponentModel.IContainer components = null;

        // Main layout panels
        private Panel pnlHeader;   // Company name and invoice title section
        private Panel pnlInfo;     // Invoice/customer/payment information section
        private Panel pnlBottom;   // Invoice totals section

        // Header labels
        private Label lblCompanyName;
        private Label lblInvoiceTitle;

        // Invoice information labels
        private Label lblInvoiceNo;
        private Label lblDate;
        private Label lblCustomer;
        private Label lblPayment;

        // Invoice summary labels
        private Label lblSubTotal;
        private Label lblDiscount;
        private Label lblTax;
        private Label lblTotal;

        // Invoice product details table
        private DataGridView dgvInvoiceItems;

        // Print invoice button
        private Button btnPrint;

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            // ================= INITIALIZE CONTROLS =================

            pnlHeader = new Panel();
            pnlInfo = new Panel();
            pnlBottom = new Panel();

            lblCompanyName = new Label();
            lblInvoiceTitle = new Label();

            lblInvoiceNo = new Label();
            lblDate = new Label();
            lblCustomer = new Label();
            lblPayment = new Label();

            lblSubTotal = new Label();
            lblDiscount = new Label();
            lblTax = new Label();
            lblTotal = new Label();

            dgvInvoiceItems = new DataGridView();

            btnPrint = new Button();

            SuspendLayout();

            // ================= HEADER SECTION =================
            // Displays company branding and invoice heading

            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Height = 100;
            pnlHeader.BackColor = Color.White;

            lblCompanyName.Text = "MS Crete";
            lblCompanyName.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblCompanyName.AutoSize = true;
            lblCompanyName.Location = new Point(30, 20);

            lblInvoiceTitle.Text = "SALES INVOICE";
            lblInvoiceTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblInvoiceTitle.AutoSize = true;
            lblInvoiceTitle.Location = new Point(650, 30);

            pnlHeader.Controls.Add(lblCompanyName);
            pnlHeader.Controls.Add(lblInvoiceTitle);

            // ================= CUSTOMER / INVOICE INFO SECTION =================
            // Shows invoice number, date, customer and payment details

            pnlInfo.Location = new Point(30, 120);
            pnlInfo.Size = new Size(840, 100);
            pnlInfo.BackColor = Color.White;

            lblInvoiceNo.Text = "Invoice No:";
            lblInvoiceNo.AutoSize = true;
            lblInvoiceNo.Location = new Point(20, 20);

            lblDate.Text = "Date:";
            lblDate.AutoSize = true;
            lblDate.Location = new Point(20, 55);

            lblCustomer.Text = "Customer:";
            lblCustomer.AutoSize = true;
            lblCustomer.Location = new Point(400, 20);

            lblPayment.Text = "Payment:";
            lblPayment.AutoSize = true;
            lblPayment.Location = new Point(400, 55);

            pnlInfo.Controls.Add(lblInvoiceNo);
            pnlInfo.Controls.Add(lblDate);
            pnlInfo.Controls.Add(lblCustomer);
            pnlInfo.Controls.Add(lblPayment);

            // ================= INVOICE ITEMS GRID =================
            // Displays purchased products, quantity, price and totals

            dgvInvoiceItems.Location = new Point(30, 240);
            dgvInvoiceItems.Size = new Size(840, 250);

            dgvInvoiceItems.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            dgvInvoiceItems.AllowUserToAddRows = false;

            // Invoice data should only be viewed, not edited
            dgvInvoiceItems.ReadOnly = true;

            // Removes unnecessary row selector column
            dgvInvoiceItems.RowHeadersVisible = false;

            // ================= TOTAL SUMMARY SECTION =================
            // Displays subtotal, discount, tax and final payable amount

            pnlBottom.Location = new Point(30, 520);
            pnlBottom.Size = new Size(840, 140);
            pnlBottom.BackColor = Color.White;

            lblSubTotal.Text = "Subtotal:";
            lblSubTotal.Font = new Font("Segoe UI", 11F);
            lblSubTotal.AutoSize = true;
            lblSubTotal.Location = new Point(520, 10);

            lblDiscount.Text = "Discount:";
            lblDiscount.Font = new Font("Segoe UI", 11F);
            lblDiscount.AutoSize = true;
            lblDiscount.Location = new Point(520, 35);

            lblTax.Text = "Tax:";
            lblTax.Font = new Font("Segoe UI", 11F);
            lblTax.AutoSize = true;
            lblTax.Location = new Point(520, 60);

            // Highlight final invoice amount
            lblTotal.Text = "Grand Total:";
            lblTotal.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTotal.AutoSize = true;
            lblTotal.Location = new Point(520, 95);

            pnlBottom.Controls.Add(lblSubTotal);
            pnlBottom.Controls.Add(lblDiscount);
            pnlBottom.Controls.Add(lblTax);
            pnlBottom.Controls.Add(lblTotal);

            // ================= PRINT BUTTON =================
            // Allows user to print generated invoice

            btnPrint.Text = "Print Invoice";

            btnPrint.Size = new Size(150, 45);

            btnPrint.Font = new Font("Segoe UI", 11F, FontStyle.Bold);

            btnPrint.Location = new Point(375, 640);

            // Print event handler
            btnPrint.Click += btnPrint_Click;

            // ================= FORM SETTINGS =================
            // Main invoice report window configuration

            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(920, 720);
            BackColor = Color.White;
            Text = "Sales Invoice";
            StartPosition = FormStartPosition.CenterScreen;

            // ================= ADD CONTROLS TO FORM =================

            Controls.Add(btnPrint);
            Controls.Add(pnlBottom);
            Controls.Add(dgvInvoiceItems);
            Controls.Add(pnlInfo);
            Controls.Add(pnlHeader);

            ResumeLayout(false);
        }
    }
}