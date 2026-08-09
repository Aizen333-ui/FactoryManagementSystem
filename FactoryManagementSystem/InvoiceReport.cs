using FactoryManagementCore;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Drawing.Printing;

namespace SalesDashboard.Pages
{
    public partial class InvoiceReport : Form
    {
        // Handles invoice printing functionality
        private PrintDocument printDocument;

        // Displays print preview before printing
        private PrintPreviewDialog printPreview;

        // Current invoice/sale ID
        private int saleID;

        public InvoiceReport(int id)
        {
            InitializeComponent();

            saleID = id;

            
            dgvInvoiceItems.ReadOnly = true;
            dgvInvoiceItems.AllowUserToAddRows = false;
            dgvInvoiceItems.RowHeadersVisible = false;
            dgvInvoiceItems.MultiSelect = false;
            // Load invoice details and products from database
            LoadInvoice();

            // Configure printing components
            InitializePrinting();
            this.Shown += InvoiceReport_Shown;
        }
        // Clears selection in the DataGridView when the form is shown
        private void InvoiceReport_Shown(object sender, EventArgs e)
        {
            dgvInvoiceItems.ClearSelection();
            dgvInvoiceItems.CurrentCell = null;
        }

        // ================= PRINT INITIALIZATION =================
        // Creates print document and connects print event handler

        private void InitializePrinting()
        {
            printDocument = new PrintDocument();

            // Method responsible for drawing invoice content
            printDocument.PrintPage += PrintDocument_PrintPage;

            printPreview = new PrintPreviewDialog();

            printPreview.Document = printDocument;
        }

        // ================= PRINT BUTTON EVENT =================
        // Opens printer selection and prints the invoice

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                if (printDocument == null)
                {
                    MessageBox.Show("Print document not initialized.");
                    return;
                }

                // Check if system has a valid printer installed
                PrinterSettings settings = new PrinterSettings();

                if (!settings.IsValid)
                {
                    MessageBox.Show("No valid printer is installed.");
                    return;
                }

                printDocument.PrinterSettings = settings;

                using (PrintDialog dialog = new PrintDialog())
                {
                    dialog.Document = printDocument;

                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        // Send invoice to selected printer
                        printDocument.Print();

                        // Store successful print activity in audit logs
                        try
                        {
                            Logger.AddLog(
                                Session.CurrentUser ?? "system",
                                "Print Invoice",
                                "Invoice Report",
                                $"Printed invoice #{saleID}",
                                "Success");
                        }
                        catch { }
                    }
                }
            }
            catch (Exception ex)
            {
                // Store failed print attempt
                try
                {
                    Logger.AddLog(
                        Session.CurrentUser ?? "system",
                        "Print Invoice",
                        "Invoice Report",
                        ex.Message,
                        "Failed");
                }
                catch { }

                MessageBox.Show(
                    "Printing error: " + ex.Message,
                    "Print",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        // ================= PRINT PAGE DESIGN =================
        // Draws invoice content on paper using Graphics object

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;

            Font companyFont =
                new Font("Segoe UI", 18, FontStyle.Bold);

            Font invoiceTitleFont =
                new Font("Segoe UI", 16, FontStyle.Bold);

            Font normalFont =
                new Font("Segoe UI", 11);

            Font boldFont =
                new Font("Segoe UI", 11, FontStyle.Bold);

            Font totalFont =
                new Font("Segoe UI", 14, FontStyle.Bold);

            // ============================================================
            // PAGE DIMENSIONS
            // ============================================================

            float pageWidth = e.PageBounds.Width;
            float pageHeight = e.PageBounds.Height;

            float leftMargin = 50;
            float rightMargin = pageWidth - 50;

            float y = 50;

            // ============================================================
            // COMPANY NAME - CENTERED
            // ============================================================

            string companyName = "MS Crete";

            SizeF companySize =
                g.MeasureString(companyName, companyFont);

            float companyX =
                (pageWidth - companySize.Width) / 2;

            g.DrawString(
                companyName,
                companyFont,
                Brushes.Black,
                companyX,
                y
            );

            y += 40;

            // ============================================================
            // SALES INVOICE - CENTERED
            // ============================================================

            string invoiceTitle = "SALES INVOICE";

            SizeF invoiceTitleSize =
                g.MeasureString(invoiceTitle, invoiceTitleFont);

            float invoiceTitleX =
                (pageWidth - invoiceTitleSize.Width) / 2;

            g.DrawString(
                invoiceTitle,
                invoiceTitleFont,
                Brushes.Black,
                invoiceTitleX,
                y
            );

            y += 50;

            // ============================================================
            // CUSTOMER INFORMATION
            // ============================================================

            g.DrawString(
                lblInvoiceNo.Text,
                normalFont,
                Brushes.Black,
                leftMargin,
                y
            );

            g.DrawString(
                lblDate.Text,
                normalFont,
                Brushes.Black,
                pageWidth / 2,
                y
            );

            y += 25;

            g.DrawString(
                lblCustomer.Text,
                normalFont,
                Brushes.Black,
                leftMargin,
                y
            );

            g.DrawString(
                lblPayment.Text,
                normalFont,
                Brushes.Black,
                pageWidth / 2,
                y
            );

            y += 35;

            // ============================================================
            // SEPARATOR
            // ============================================================

            g.DrawLine(
                Pens.Black,
                leftMargin,
                y,
                rightMargin,
                y
            );

            y += 15;

            // ============================================================
            // ITEMS HEADER
            // ============================================================

            float productX = leftMargin;
            float qtyX = 350;
            float priceX = 420;
            float amountX = 520;

            g.DrawString(
                "Product",
                boldFont,
                Brushes.Black,
                productX,
                y
            );

            g.DrawString(
                "Qty",
                boldFont,
                Brushes.Black,
                qtyX,
                y
            );

            g.DrawString(
                "Price",
                boldFont,
                Brushes.Black,
                priceX,
                y
            );

            g.DrawString(
                "Amount",
                boldFont,
                Brushes.Black,
                amountX,
                y
            );

            y += 30;

            // ============================================================
            // PRODUCT ITEMS
            // ============================================================

            foreach (DataGridViewRow row in dgvInvoiceItems.Rows)
            {
                if (row.IsNewRow)
                    continue;

                string product =
                    row.Cells["ProductName"].Value?.ToString() ?? "";

                string quantity =
                    row.Cells["Quantity"].Value?.ToString() ?? "";

                string price =
                    row.Cells["UnitPrice"].Value?.ToString() ?? "";

                string amount =
                    row.Cells["TotalAmount"].Value?.ToString() ?? "";

                g.DrawString(
                    product,
                    normalFont,
                    Brushes.Black,
                    productX,
                    y
                );

                g.DrawString(
                    quantity,
                    normalFont,
                    Brushes.Black,
                    qtyX,
                    y
                );

                g.DrawString(
                    price,
                    normalFont,
                    Brushes.Black,
                    priceX,
                    y
                );

                g.DrawString(
                    amount,
                    normalFont,
                    Brushes.Black,
                    amountX,
                    y
                );

                y += 25;
            }

            // ============================================================
            // SEPARATOR BEFORE TOTALS
            // ============================================================

            y += 10;

            g.DrawLine(
                Pens.Black,
                leftMargin,
                y,
                rightMargin,
                y
            );

            y += 20;

            // ============================================================
            // FINANCIAL SUMMARY
            // ============================================================

            float summaryLabelX = 350;
            float summaryValueX = 520;

            g.DrawString(
                lblSubTotal.Text,
                normalFont,
                Brushes.Black,
                summaryLabelX,
                y
            );

            y += 25;

            g.DrawString(
                lblDiscount.Text,
                normalFont,
                Brushes.Black,
                summaryLabelX,
                y
            );

            y += 25;

            g.DrawString(
                lblTax.Text,
                normalFont,
                Brushes.Black,
                summaryLabelX,
                y
            );

            y += 30;

            // ============================================================
            // GRAND TOTAL
            // ============================================================

            g.DrawLine(
                Pens.Black,
                summaryLabelX,
                y,
                rightMargin,
                y
            );

            y += 10;

            g.DrawString(
                lblTotal.Text,
                totalFont,
                Brushes.Black,
                summaryLabelX,
                y
            );

            y += 50;

            // ============================================================
            // FOOTER
            // ============================================================

            string footer = "Thank you for your business!";

            SizeF footerSize =
                g.MeasureString(footer, normalFont);

            float footerX =
                (pageWidth - footerSize.Width) / 2;

            g.DrawString(
                footer,
                normalFont,
                Brushes.Black,
                footerX,
                y
            );
        }

        // ================= LOAD INVOICE DATA =================
        // Retrieves invoice header and item details from database

        private void LoadInvoice()
        {
            try
            {
                // Load invoice main information
                DataTable sale = DBHelper.ExecuteDataTable(
                    @"
                    SELECT
                        s.InvoiceNo,
                        s.SaleDate,
                        c.CustomerName,
                        s.SubTotal,
                        s.Discount,
                        s.Tax,
                        s.GrandTotal,
                        s.PaymentMethod
                    FROM Sales s
                    INNER JOIN Customers c
                    ON s.CustomerID = c.CustomerID
                    LEFT JOIN SalesPayment sp
                    ON s.SaleID = sp.SaleID
                    WHERE s.SaleID = @id
                    ",
                    new SqlParameter[]
                    {
                        new SqlParameter("@id", saleID)
                    }
                );

                if (sale.Rows.Count == 0)
                {
                    MessageBox.Show("Invoice data not found.");
                    return;
                }

                DataRow row = sale.Rows[0];

                // Display invoice header information
                lblInvoiceNo.Text =
                    "Invoice No: " + row["InvoiceNo"].ToString();

                lblDate.Text =
                    "Date: " +
                    Convert.ToDateTime(row["SaleDate"])
                    .ToString("dd-MM-yyyy");

                lblCustomer.Text =
                    "Customer: " + row["CustomerName"].ToString();

                lblPayment.Text =
                    "Payment: " +
                    (row["PaymentMethod"] == DBNull.Value
                    ? "Pending"
                    : row["PaymentMethod"].ToString());

                // Display invoice financial summary
                lblSubTotal.Text =
                    "Subtotal: Rs. " +
                    Convert.ToDecimal(row["SubTotal"]).ToString("N2");

                lblDiscount.Text =
                    "Discount: Rs. " +
                    Convert.ToDecimal(row["Discount"]).ToString("N2");

                lblTax.Text =
                    "Tax: Rs. " +
                    Convert.ToDecimal(row["Tax"]).ToString("N2");

                lblTotal.Text =
                    "Grand Total: Rs. " +
                    Convert.ToDecimal(row["GrandTotal"])
                    .ToString("N2");

                // Load purchased products into invoice grid
                dgvInvoiceItems.DataSource =
                    DBHelper.ExecuteDataTable(
                    @"
                    SELECT
                        ProductName,
                        Quantity,
                        UnitPrice,
                        TotalAmount
                    FROM SaleItems
                    WHERE SaleID=@id
                    ",
                    new SqlParameter[]
                    {
                        new SqlParameter("@id", saleID)
                    });
                dgvInvoiceItems.ClearSelection();
                dgvInvoiceItems.CurrentCell = null;
                // Save invoice viewing activity
                try
                {
                    Logger.AddLog(
                        Session.CurrentUser ?? "system",
                        "View Invoice",
                        "Invoice Report",
                        $"Viewed invoice {row["InvoiceNo"]}",
                        "Success");
                }
                catch { }
            }
            catch (Exception ex)
            {
                // Save failed loading attempt
                try
                {
                    Logger.AddLog(
                        Session.CurrentUser ?? "system",
                        "View Invoice",
                        "Invoice Report",
                        ex.Message,
                        "Failed");
                }
                catch { }

                MessageBox.Show(
                    "Invoice loading failed: " + ex.Message
                );
            }
        }
    }
}