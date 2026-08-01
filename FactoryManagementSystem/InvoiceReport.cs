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

            // Load invoice details and products from database
            LoadInvoice();

            // Configure printing components
            InitializePrinting();
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

            Font titleFont =
                new Font("Segoe UI", 18, FontStyle.Bold);

            Font normalFont =
                new Font("Segoe UI", 11);

            int x = 50;
            int y = 50;

            // ================= INVOICE HEADER =================

            g.DrawString(
                lblCompanyName.Text,
                titleFont,
                Brushes.Black,
                x,
                y
            );

            y += 50;

            // ================= CUSTOMER INFORMATION =================

            g.DrawString(lblInvoiceNo.Text, normalFont, Brushes.Black, x, y);

            y += 25;

            g.DrawString(lblDate.Text, normalFont, Brushes.Black, x, y);

            y += 25;

            g.DrawString(lblCustomer.Text, normalFont, Brushes.Black, x, y);

            y += 25;

            g.DrawString(lblPayment.Text, normalFont, Brushes.Black, x, y);


            y += 40;

            // ================= ITEMS TABLE HEADER =================

            g.DrawString("Product", normalFont, Brushes.Black, x, y);

            g.DrawString("Qty", normalFont, Brushes.Black, x + 250, y);

            g.DrawString("Price", normalFont, Brushes.Black, x + 330, y);

            g.DrawString("Amount", normalFont, Brushes.Black, x + 430, y);

            y += 30;

            // ================= PRINT PRODUCT ITEMS =================
            // Reads rows from invoice DataGridView and prints them

            foreach (DataGridViewRow row in dgvInvoiceItems.Rows)
            {
                if (row.IsNewRow)
                    continue;

                g.DrawString(
                    row.Cells["ProductName"].Value.ToString(),
                    normalFont,
                    Brushes.Black,
                    x,
                    y
                );

                g.DrawString(
                    row.Cells["Quantity"].Value.ToString(),
                    normalFont,
                    Brushes.Black,
                    x + 250,
                    y
                );

                g.DrawString(
                    row.Cells["UnitPrice"].Value.ToString(),
                    normalFont,
                    Brushes.Black,
                    x + 330,
                    y
                );

                g.DrawString(
                    row.Cells["TotalAmount"].Value.ToString(),
                    normalFont,
                    Brushes.Black,
                    x + 430,
                    y
                );

                y += 25;
            }

            y += 40;

            // Print final payable amount
            g.DrawString(
                lblTotal.Text,
                new Font("Segoe UI", 14, FontStyle.Bold),
                Brushes.Black,
                x + 350,
                y
            );

            y += 50;

            // Invoice footer message
            g.DrawString(
                "Thank you for your business!",
                normalFont,
                Brushes.Black,
                x,
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