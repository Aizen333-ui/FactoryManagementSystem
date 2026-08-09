using System.Data;
using FactoryManagementCore;
namespace FactoryManagementSystem
{
    public partial class OwnerReportsPage : UserControl
    {
        // ===================== CONSTRUCTOR =====================
        private DataTable? reportTable;

        public OwnerReportsPage()
        {
            InitializeComponent();

            datagridReport.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            datagridReport.MultiSelect = false;
            datagridReport.ClearSelection();
            datagridReport.CurrentCell = null;
            ClearReportSelection();
        }

        private void ClearReportSelection()
        {
            datagridReport.ClearSelection();
            datagridReport.CurrentCell = null;
        }

        //Selected date confirmation
        private void btnViewReport_Click(object sender, EventArgs e)
        {
            DateTime fromDate = dtFrom.Value.Date;
            DateTime toDate = dtTo.Value.Date;

            try
            {
                datagridReport.DataSource = null;

                // ================= MATERIAL USAGE =================
                string materialQuery = @"
                SELECT MaterialName, QuantityUsed, Date
                FROM MaterialUsage
                WHERE Date BETWEEN @from AND @to
                ORDER BY Date ASC";

                DataTable materialTable = DBHelper.ExecuteDataTable(materialQuery,
                    new Microsoft.Data.SqlClient.SqlParameter[]
                    {
                    new Microsoft.Data.SqlClient.SqlParameter("@from", fromDate),
                    new Microsoft.Data.SqlClient.SqlParameter("@to", toDate)
                    });

                materialTable.TableName = "Material Usage";

                // ================= PRODUCTION =================
                string productionQuery = @"
                SELECT  ProductName, Quantity, Date
                FROM Production
                WHERE Date BETWEEN @from AND @to
                ORDER BY Date ASC";

                DataTable productionTable = DBHelper.ExecuteDataTable(productionQuery,
                    new Microsoft.Data.SqlClient.SqlParameter[]
                    {
                    new Microsoft.Data.SqlClient.SqlParameter("@from", fromDate),
                    new Microsoft.Data.SqlClient.SqlParameter("@to", toDate)
                    });

                productionTable.TableName = "Production";

                string salestable = @"
                SELECT
                    c.CustomerName,
                    s.SaleDate,
                    s.GrandTotal,
                    s.PaymentStatus
                FROM Sales s
                INNER JOIN Customers c
                    ON s.CustomerID = c.CustomerID
                WHERE s.SaleDate >= @from
                  AND s.SaleDate < DATEADD(DAY, 1, @to)
                ORDER BY s.SaleDate ASC;";

                DataTable salesTable = DBHelper.ExecuteDataTable(salestable,
                    new Microsoft.Data.SqlClient.SqlParameter[]
                    {
                    new Microsoft.Data.SqlClient.SqlParameter("@from", fromDate),
                    new Microsoft.Data.SqlClient.SqlParameter("@to", toDate)
                    });

                salesTable.TableName = "Sales";

                string returnstable = @"
                SELECT
                    c.CustomerName,
                    r.RefundAmount,
                    r.ReturnDate
                FROM Returns r
                INNER JOIN Sales s
                    ON r.SaleID = s.SaleID
                INNER JOIN Customers c
                    ON s.CustomerID = c.CustomerID
                INNER JOIN Production p
                    ON r.ProductionID = p.ProductionID
                WHERE r.ReturnDate >= @from
                  AND r.ReturnDate < DATEADD(DAY, 1, @to)
                ORDER BY r.ReturnDate ASC;";

                DataTable returnsTable = DBHelper.ExecuteDataTable(returnstable,
                    new Microsoft.Data.SqlClient.SqlParameter[]
                    {
                    new Microsoft.Data.SqlClient.SqlParameter("@from", fromDate),
                    new Microsoft.Data.SqlClient.SqlParameter("@to", toDate)
                    });

                // ================= SHOW MATERIAL FIRST =================
                DataTable report = new DataTable();

                report.Columns.Add("Type");
                report.Columns.Add("Name");
                report.Columns.Add("Amount/Qty");
                report.Columns.Add("Date");

                // ================= MATERIAL =================
                foreach (DataRow row in materialTable.Rows)
                {
                    report.Rows.Add(
                        "Material Usage",
                        row["MaterialName"],
                        row["QuantityUsed"],
                        row["Date"]
                    );
                }

                // ================= PRODUCTION =================
                foreach (DataRow row in productionTable.Rows)
                {
                    report.Rows.Add(
                        "Production",
                        row["ProductName"],
                        row["Quantity"],
                        row["Date"]
                    );
                }

                // ================= SALES =================
                foreach (DataRow row in salesTable.Rows)
                {
                    report.Rows.Add(
                        "Sale",
                        row["CustomerName"],
                        row["GrandTotal"],
                        row["SaleDate"]
                    );
                }

                // ================= RETURNS =================
                foreach (DataRow row in returnsTable.Rows)
                {
                    report.Rows.Add(
                        "Return",
                        row["CustomerName"],
                        row["RefundAmount"],
                        row["ReturnDate"]
                    );
                }

                reportTable = report;
                datagridReport.DataSource = reportTable;
                datagridReport.ClearSelection();
                datagridReport.CurrentCell = null;
                MessageBox.Show("Report loaded!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching report: " + ex.Message);
            }
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            if (reportTable == null)
            {
                MessageBox.Show("Please load the report first.");
                return;
            }

            string filter = cmbFilter.SelectedItem?.ToString() ?? "All";

            if (filter == "All")
            {
                datagridReport.DataSource = reportTable;
            }
            else
            {
                DataView view = new DataView(reportTable);
                view.RowFilter = $"Type = '{filter.Replace("'", "''")}'";

                datagridReport.DataSource = view;
            }

            datagridReport.ClearSelection();
            datagridReport.CurrentCell = null;
        }

        // BACK BUTTON - NAVIGATES TO HOME PAGE

        private void btnBack_Click(object sender, EventArgs e)
        {
            OwnerDashBoard dashboard =
                (OwnerDashBoard)this.FindForm();
            dashboard.ResetSidebarSelection();

            dashboard.LoadPage(new OwnerDash());
        }
    }


}