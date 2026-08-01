using System;
using System.Data;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using FactoryManagementCore;

namespace SalesDashboard.Pages
{
    public partial class Returns : UserControl
    {
        // Stores currently loaded invoice information
        // These values are required before processing a return
        private int? currentSaleId = null;
        private int? currentCustomerId = null;

        public Returns()
        {
            InitializeComponent();

            // Button event bindings
            btnBack.Click += btnBack_Click;
            btnSearchInvoice.Click += btnSearchInvoice_Click;
            btnClearInvoice.Click += btnClearInvoice_Click;
            btnProcessReturn.Click += btnProcessReturn_Click;

            // Grid events
            dgvReturnItems.SelectionChanged += dgvReturnItems_SelectionChanged;

            // Recalculate refund amount whenever quantity changes
            txtReturnQty.TextChanged += txtReturnQty_TextChanged;

            // Initial UI state
            ClearReturnFields();

            // Load previously processed returns
            LoadReturnHistory();
        }

        // ==================================================
        // CLEAR CURRENT INVOICE DETAILS
        // ==================================================
        // Removes loaded invoice data and resets return controls
        // ==================================================

        private void btnClearInvoice_Click(object? sender, EventArgs e)
        {
            currentSaleId = null;
            currentCustomerId = null;

            txtInvoiceNumber.Clear();

            lblInvoiceNo.Text = "-";
            lblCustomerName.Text = "-";
            lblSaleDate.Text = "-";
            lblPaymentMethod.Text = "-";
            lblOriginalTotal.Text = "-";

            // Remove loaded sale items
            dgvReturnItems.DataSource = null;

            ClearReturnFields();

            MessageBox.Show(
                "Invoice details cleared.",
                "Returns",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        // ==================================================
        // LOAD RETURN HISTORY
        // ==================================================
        // Displays all previously processed returns
        // ==================================================
        private void LoadReturnHistory()
        {
            try
            {
                string query = @"
                SELECT 
                    c.CustomerName,
                    p.ProductName,
                    r.Quantity,
                    r.RefundAmount,
                    r.RefundMethod,
                    r.Reason,
                    r.ReturnDate

                FROM Returns r

                INNER JOIN Sales s
                ON r.SaleID = s.SaleID

                INNER JOIN Customers c
                ON s.CustomerID = c.CustomerID

                INNER JOIN Production p
                ON r.ProductionID = p.ProductionID

                ORDER BY r.ReturnDate DESC;";

                DataTable dt =
                    DBHelper.ExecuteDataTable(query, null);

                dgvReturnHistory.AutoGenerateColumns = true;
                dgvReturnHistory.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error loading return history: " + ex.Message,
                    "Returns",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                dgvReturnHistory.DataSource = null;
            }
        }

        // ==================================================
        // SEARCH INVOICE
        // ==================================================
        // Finds an invoice and loads its details for return
        // ==================================================
        private void btnSearchInvoice_Click(object? sender, EventArgs e)
        {
            string invoice =
                txtInvoiceNumber.Text.Trim();


            if (string.IsNullOrWhiteSpace(invoice))
            {
                MessageBox.Show(
                    "Enter invoice number."
                );

                return;
            }

            LoadInvoice(invoice);
        }

        // ==================================================
        // LOAD INVOICE DETAILS
        // ==================================================
        // Retrieves customer, payment and total information
        // for the selected invoice
        // ==================================================
        private void LoadInvoice(string invoice)
        {
            try
            {
                string query = @"
                SELECT
                    s.SaleID,
                    s.InvoiceNo,
                    s.SaleDate,
                    s.PaymentMethod,
                    s.GrandTotal,
                    c.CustomerID,
                    c.CustomerName

                FROM Sales s

                INNER JOIN Customers c
                ON s.CustomerID = c.CustomerID

                WHERE s.InvoiceNo=@invoice";

                DataTable dt =
                    DBHelper.ExecuteDataTable(
                        query,
                        new[]
                        {
                            new SqlParameter(
                                "@invoice",
                                invoice)
                        });

                // Stop if invoice does not exist
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show(
                        "Invoice not found."
                    );

                    return;
                }

                DataRow row = dt.Rows[0];

                // Store IDs for later return processing
                currentSaleId =
                    Convert.ToInt32(row["SaleID"]);

                currentCustomerId =
                    Convert.ToInt32(row["CustomerID"]);

                // Display invoice information
                lblInvoiceNo.Text =
                    row["InvoiceNo"].ToString();

                lblCustomerName.Text =
                    row["CustomerName"].ToString();

                lblSaleDate.Text =
                    Convert.ToDateTime(row["SaleDate"])
                    .ToString("dd-MM-yyyy");

                lblPaymentMethod.Text =
                    row["PaymentMethod"].ToString();

                lblOriginalTotal.Text =
                    Convert.ToDecimal(row["GrandTotal"])
                    .ToString("N2");

                // Load products purchased in this invoice
                LoadSaleItems();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error loading invoice: " + ex.Message
                );
            }
        }

        // ==================================================
        // LOAD SOLD PRODUCTS
        // ==================================================
        // Loads all products purchased under the selected invoice
        // These products become available for return selection
        // ==================================================

        private void LoadSaleItems()
        {
            try
            {
                string query = @"
                SELECT
                    ProductionID,
                    ProductName,
                    Quantity,
                    UnitPrice,
                    TotalAmount

                FROM SaleItems

                WHERE SaleID=@sale";

                DataTable dt =
                    DBHelper.ExecuteDataTable(
                        query,
                        new[]
                        {
                            new SqlParameter(
                                "@sale",
                                currentSaleId)
                        });

                dgvReturnItems.AutoGenerateColumns = true;
                dgvReturnItems.DataSource = dt;

                // ProductionID is only required internally
                // so hide it from the user
                dgvReturnItems.Columns["ProductionID"].Visible = false;

                // Prevent accidental selection after loading
                dgvReturnItems.ClearSelection();

                // Reset return input fields
                ClearReturnFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error loading sale items: " + ex.Message
                );
            }
        }

        // ==================================================
        // PRODUCT SELECTION CHANGE
        // ==================================================
        // When user selects a product, automatically load
        // the sold quantity into the return quantity textbox
        // ==================================================

        private void dgvReturnItems_SelectionChanged(
            object? sender,
            EventArgs e)
        {

            if (dgvReturnItems.SelectedRows.Count == 0)
                return;

            DataGridViewRow row =
                dgvReturnItems.SelectedRows[0];

            // Default return quantity is the sold quantity
            // User can reduce it if partial return is needed
            txtReturnQty.Text =
                row.Cells["Quantity"]
                .Value?
                .ToString() ?? "0";

        }

        // ==================================================
        // CALCULATE REFUND AMOUNT
        // ==================================================
        // Calculates refund automatically:
        //
        // Return Quantity × Product Unit Price
        //
        // This keeps refund amount consistent with sale data
        // ==================================================

        private void txtReturnQty_TextChanged(
            object? sender,
            EventArgs e)
        {

            // Cannot calculate without selected product
            if (dgvReturnItems.SelectedRows.Count == 0)
                return;

            // Validate quantity input
            if (!decimal.TryParse(
                txtReturnQty.Text,
                out decimal qty))
            {
                txtReturnAmount.Clear();
                txtRefundAmount.Clear();

                return;
            }

            DataGridViewRow row =
                dgvReturnItems.SelectedRows[0];

            decimal price =
                Convert.ToDecimal(
                    row.Cells["UnitPrice"].Value);

            decimal amount =
                qty * price;

            // Display calculated refund
            txtReturnAmount.Text =
                amount.ToString("N2");

            txtRefundAmount.Text =
                amount.ToString("N2");

        }

        // ==================================================
        // CLEAR RETURN INPUT FIELDS
        // ==================================================
        // Resets all return-related controls after:
        // - invoice clear
        // - successful return
        // - loading a new invoice
        // ==================================================
        private void ClearReturnFields()
        {
            txtReturnQty.Clear();
            txtReturnAmount.Clear();
            txtRefundAmount.Clear();
            txtNotes.Clear();

            // Select placeholder option
            // Example: "Select Reason"
            if (cmbReturnReason.Items.Count > 0)
                cmbReturnReason.SelectedIndex = 0;

            // Select placeholder option
            // Example: "Select Refund Method"
            if (cmbRefundMethod.Items.Count > 0)
                cmbRefundMethod.SelectedIndex = 0;
        }
        // ==================================================
        // PROCESS RETURN
        // ==================================================
        // Handles complete return workflow:
        //
        // 1. Validate return request
        // 2. Insert return record
        // 3. Restore stock (if applicable)
        // 4. Commit database transaction
        // 5. Add audit log
        // 6. Reset return screen
        //
        // Transaction is used so that inserting the return
        // and updating inventory happen together.
        // If one fails, both changes are cancelled.
        // ==================================================
        private void btnProcessReturn_Click(object? sender, EventArgs e)
        {

            // Invoice must be loaded before processing return
            if (currentSaleId == null || currentCustomerId == null)
            {
                MessageBox.Show(
                    "Search an invoice first."
                );

                return;
            }

            // User must select a product from invoice items
            if (dgvReturnItems.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                    "Select an item to return."
                );

                return;
            }

            // Validate return reason selection
            if (cmbReturnReason.SelectedIndex == 0)
            {
                MessageBox.Show(
                    "Please select return reason.");

                return;
            }

            // Validate refund method selection
            if (cmbRefundMethod.SelectedIndex == 0)
            {
                MessageBox.Show(
                    "Please select refund method.");

                return;
            }

            // Validate entered quantity
            if (!decimal.TryParse(
                txtReturnQty.Text,
                out decimal returnQty)
                || returnQty <= 0)
            {
                MessageBox.Show(
                    "Enter valid return quantity."
                );

                return;
            }

            DataGridViewRow row =
                dgvReturnItems.SelectedRows[0];

            // Product identifier used for stock update
            int productionId =
                Convert.ToInt32(
                    row.Cells["ProductionID"].Value);

            string productName =
                row.Cells["ProductName"]
                .Value?
                .ToString() ?? "";

            // Original sold quantity
            decimal soldQuantity =
                Convert.ToDecimal(
                    row.Cells["Quantity"].Value);

            decimal unitPrice =
                Convert.ToDecimal(
                    row.Cells["UnitPrice"].Value);

            // Prevent returning more quantity than purchased
            if (returnQty > soldQuantity)
            {
                MessageBox.Show(
                    "Return quantity cannot exceed sold quantity."
                );

                return;
            }

            string reason =
                cmbReturnReason.SelectedItem?
                .ToString()
                ?? "Other";

            string refundMethod =
                cmbRefundMethod.SelectedItem?
                .ToString()
                ?? "Cash";

            decimal refundAmount =
                decimal.Parse(
                    txtRefundAmount.Text);

            try
            {

                using (SqlConnection con =
                    new SqlConnection(DBHelper.ConnectionString))
                {

                    con.Open();

                    // Begin transaction because return insertion
                    // and inventory update must succeed together
                    using (SqlTransaction tr =
                        con.BeginTransaction())
                    {

                        try
                        {

                            // ======================================
                            // INSERT RETURN RECORD
                            // ======================================
                            string query = @"

                            INSERT INTO Returns
                            (
                                SaleID,
                                ProductionID,
                                Quantity,
                                RefundAmount,
                                RefundMethod,
                                Reason,
                                Notes,
                                ReturnDate
                            )

                            VALUES
                            (
                                @sale,
                                @production,
                                @qty,
                                @refund,
                                @method,
                                @reason,
                                @notes,
                                GETDATE()
                            );";

                            using (SqlCommand cmd =
                                new SqlCommand(
                                    query,
                                    con,
                                    tr))
                            {

                                cmd.Parameters.AddWithValue(
                                    "@sale",
                                    currentSaleId.Value);

                                cmd.Parameters.AddWithValue(
                                    "@production",
                                    productionId);

                                cmd.Parameters.AddWithValue(
                                    "@qty",
                                    returnQty);

                                cmd.Parameters.AddWithValue(
                                    "@refund",
                                    refundAmount);


                                cmd.Parameters.AddWithValue(
                                    "@method",
                                    refundMethod);

                                cmd.Parameters.AddWithValue(
                                    "@reason",
                                    reason);

                                cmd.Parameters.AddWithValue(
                                    "@notes",
                                    txtNotes.Text.Trim());

                                cmd.ExecuteNonQuery();
                            }

                            // ======================================
                            // RESTORE STOCK
                            // ======================================
                            // Damaged products are not returned to
                            // available inventory.
                            //
                            // All other returns increase stock again.
                            // ======================================
                            if (reason != "Damaged")
                            {
                                string stockQuery = @"
                                UPDATE Production
                                SET Quantity = Quantity + @qty
                                WHERE ProductionID = @id";



                                using (SqlCommand cmd =
                                    new SqlCommand(
                                        stockQuery,
                                        con,
                                        tr))
                                {

                                    cmd.Parameters.AddWithValue(
                                        "@qty",
                                        returnQty);

                                    cmd.Parameters.AddWithValue(
                                        "@id",
                                        productionId);

                                    cmd.ExecuteNonQuery();
                                }
                            }

                            // Save all changes permanently
                            tr.Commit();

                            MessageBox.Show(
                                "Return processed successfully.",
                                "Returns",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);

                            // ======================================
                            // AUDIT LOG
                            // ======================================
                            // Records user activity for tracking
                            // who processed the return.
                            // ======================================
                            try
                            {
                                Logger.AddLog(
                                    Session.CurrentUser ?? "system",
                                    "Return Processed",
                                    "Returns",
                                    $"Product: {productName}, Qty: {returnQty}",
                                    "Success");
                            }
                            catch
                            {
                                // Logging failure should not
                                // interrupt successful return
                            }

                            // ======================================
                            // RESET SCREEN AFTER SUCCESS
                            // ======================================
                            currentSaleId = null;
                            currentCustomerId = null;

                            txtInvoiceNumber.Clear();

                            lblInvoiceNo.Text = "-";
                            lblCustomerName.Text = "-";
                            lblSaleDate.Text = "-";
                            lblPaymentMethod.Text = "-";
                            lblOriginalTotal.Text = "-";

                            dgvReturnItems.DataSource = null;

                            // Refresh history grid
                            LoadReturnHistory();

                            ClearReturnFields();

                            dgvReturnItems.ClearSelection();

                        }
                        catch (Exception ex)
                        {

                            // Undo database changes if anything fails
                            tr.Rollback();

                            Logger.AddLog(
                                Session.CurrentUser ?? "system",
                                "Return Processed",
                                "Returns",
                                $"Product: {productName}, Qty: {returnQty}",
                                "Failed: " + ex.Message
                            );

                            MessageBox.Show(
                                "Return failed: " + ex.Message
                            );
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Database error: " + ex.Message
                );
            }
        }

        // ==================================================
        // BACK BUTTON
        // ==================================================
        // Returns user back to Sales Dashboard screen
        // and resets sidebar selection.
        // ==================================================

        private void btnBack_Click(object? sender, EventArgs e)
        {

            var dashboard =
                this.FindForm()
                as FactoryManagementSystem.SalesDashboard;

            if (dashboard != null)
            {

                // Remove active selection from Returns page
                dashboard.ResetSidebarSelection();

                // Load default sales dashboard page
                dashboard.LoadPage(
                    new FactoryManagementSystem.SalesDash()
                );
            }
        }
    }
}