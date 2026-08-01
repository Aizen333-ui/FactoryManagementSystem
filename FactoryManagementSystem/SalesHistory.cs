using System.Data;
using Microsoft.Data.SqlClient;

namespace SalesDashboard.Pages
{
    public partial class SalesHistory : UserControl
    {
        public SalesHistory()
        {
            InitializeComponent();


            // Register button click events
            btnBack.Click += btnBack_Click;
            btnSearch.Click += btnSearch_Click;
            btnClear.Click += btnClear_Click;

            // Load all sales records when page opens
            LoadSalesHistory();
        }

        // Clears all search/filter values
        // and reloads complete sales history
        private void btnClear_Click(object? sender, EventArgs e)
        {
            txtSearch.Clear();

            cmbPaymentStatus.SelectedIndex = 0;

            LoadSalesHistory();
        }

        // ==================================================
        // LOAD SALES HISTORY DATA
        // ==================================================
        //
        // Fetches sales records from database.
        //
        // Supports:
        // - Invoice number search
        // - Customer name search
        // - Payment status filtering
        //
        // Results are displayed inside dgvSalesHistory
        //
        // ==================================================

        private void LoadSalesHistory()
        {
            try
            {
                // Base query for loading sales information
                // Customer data is joined to show customer name
                string query = @"
                SELECT 
                    s.InvoiceNo,
                    c.CustomerName,
                    s.SaleDate,
                    s.SubTotal,
                    s.Discount,
                    s.Tax,
                    s.GrandTotal,
                    s.PaymentMethod,
                    s.PaymentStatus
                FROM Sales s
                LEFT JOIN Customers c 
                    ON s.CustomerID = c.CustomerID
                WHERE 1=1";

                // Stores SQL parameters safely
                // to prevent SQL injection
                List<SqlParameter> parameters = new List<SqlParameter>();

                // ==============================
                // Search Filter
                // ==============================
                //
                // Searches by:
                // - Invoice number
                // - Customer name

                if (!string.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    query += @"
                    AND (
                        s.InvoiceNo LIKE @search
                        OR c.CustomerName LIKE @search
                    )";

                    parameters.Add(
                        new SqlParameter(
                            "@search",
                            "%" + txtSearch.Text.Trim() + "%"
                        ));
                }

                // ==============================
                // Payment Status Filter
                // ==============================
                //
                // Applies filter only when
                // user selects a specific status

                if (cmbPaymentStatus.SelectedItem != null)
                {
                    string status =
                        cmbPaymentStatus.SelectedItem.ToString();

                    // Ignore default "All Statuses" option
                    if (status != "All Statuses")
                    {
                        query +=
                            " AND s.PaymentStatus = @status";

                        parameters.Add(
                            new SqlParameter(
                                "@status",
                                status
                            ));
                    }
                }

                // Display newest sales first
                query +=
                    " ORDER BY s.SaleDate DESC";

                // Execute database query
                DataTable dt =
                    FactoryManagementCore.DBHelper.ExecuteDataTable(
                        query,
                        parameters.ToArray()
                    );

                // Bind database result to DataGridView
                dgvSalesHistory.AutoGenerateColumns = true;
                dgvSalesHistory.DataSource = dt;
            }

            catch (Exception ex)
            {
                // Prevent application crash
                // and inform user about loading failure
                MessageBox.Show(
                    "Error loading sales history: " + ex.Message
                );

                // Clear grid if loading fails
                dgvSalesHistory.DataSource = null;
            }
        }

        // Search button reloads data
        // using current filters
        private void btnSearch_Click(object? sender, EventArgs e)
        {
            LoadSalesHistory();
        }

        // Returns user back to Sales Dashboard
        private void btnBack_Click(object? sender, EventArgs e)
        {
            // Get parent dashboard form
            var dashboard = this.FindForm()
                as FactoryManagementSystem.SalesDashboard;

            if (dashboard != null)
            {
                // Remove active sidebar selection
                dashboard.ResetSidebarSelection();

                // Navigate back to dashboard home page
                dashboard.LoadPage(
                    new FactoryManagementSystem.SalesDash()
                );
            }
        }
    }
}