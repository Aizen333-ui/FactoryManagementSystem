using System.Data;
using FactoryManagementCore;

namespace FactoryManagementSystem
{
    public partial class SalesDash : UserControl
    {
        public SalesDash()
        {
            InitializeComponent();

            // Load dashboard summary cards
            // (sales count, revenue, orders, stock alerts)
            LoadDashboardData();

            // Load latest sales transactions
            // into dashboard grid
            LoadRecentSales();
        }

        // ==================================================
        // LOAD DASHBOARD SUMMARY DATA
        // ==================================================
        // Retrieves today's sales statistics and displays them
        // on dashboard summary cards.
        //
        // Cards:
        // - Today's Sales
        // - Today's Revenue
        // - Orders Today
        // - Low Stock Products
        // ==================================================
        private void LoadDashboardData()
        {
            try
            {

                // Count number of sales invoices created today
                object salesCount =
                    DBHelper.ExecuteScalar(
                        @"SELECT COUNT(*)
                          FROM Sales
                          WHERE CONVERT(date, SaleDate)
                          =
                          CONVERT(date, GETDATE())",
                        null
                    );

                lblTodaysSales.Text =
                    salesCount?.ToString() ?? "0";

                // Calculate today's received payments
                object revenue =
                    DBHelper.ExecuteScalar(
                        @"SELECT ISNULL(SUM(AmountPaid),0)
                          FROM SalesPayment
                          WHERE CONVERT(date, PaymentDate)
                          =
                          CONVERT(date, GETDATE())",
                        null
                    );

                lblTodaysRevenue.Text =
                    "Rs. " +
                    Convert.ToDecimal(revenue ?? 0)
                    .ToString("N2");

                // Count today's orders
                // Currently same as sales count because
                // every sale represents one order.
                object orders =
                    DBHelper.ExecuteScalar(
                        @"SELECT COUNT(*)
                          FROM Sales
                          WHERE CONVERT(date, SaleDate)
                          =
                          CONVERT(date, GETDATE())",
                        null
                    );

                lblOrdersToday.Text =
                    orders?.ToString() ?? "0";

                // Count products below minimum stock level
                // Threshold currently set to 500 units.
                object lowStock =
                    DBHelper.ExecuteScalar(
                        @"SELECT COUNT(*)
                          FROM Production
                          WHERE Quantity <= 500",
                        null
                    );

                lblLowStockProducts.Text =
                    lowStock?.ToString() ?? "0";
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error loading dashboard data: "
                    + ex.Message,
                    "Sales Dashboard",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ==================================================
        // LOAD RECENT SALES
        // ==================================================
        // Displays the latest 10 sales transactions.
        //
        // Data shown:
        // - Invoice number
        // - Customer name
        // - Sale date
        // - Grand total
        // - Payment status
        // ==================================================

        private void LoadRecentSales()
        {
            try
            {

                string query = @"
                SELECT TOP 10
                    s.InvoiceNo,
                    c.CustomerName,
                    s.SaleDate,
                    s.GrandTotal,
                    s.PaymentStatus

                FROM Sales s

                LEFT JOIN Customers c
                ON s.CustomerID = c.CustomerID

                ORDER BY s.SaleDate DESC;";

                DataTable dt =
                    DBHelper.ExecuteDataTable(
                        query,
                        null);

                dgvRecentSales.DataSource = dt;

                // Improve grid appearance
                dgvRecentSales.AutoSizeColumnsMode =
                    DataGridViewAutoSizeColumnsMode.Fill;

            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error loading recent sales: "
                    + ex.Message,
                    "Sales Dashboard",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                dgvRecentSales.DataSource = null;
            }
        }
    }
}