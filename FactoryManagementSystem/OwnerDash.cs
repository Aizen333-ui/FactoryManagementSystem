using System.Data;
using FactoryManagementCore;
namespace FactoryManagementSystem
{
    public partial class OwnerDash : UserControl
    {
        public OwnerDash()
        {
            InitializeComponent();

            // Prevent chart controls from receiving focus (better UX)
            materialChart.TabStop = false;
            expenseChart.TabStop = false;

            // Delay chart loading until control handle is created
            this.HandleCreated += (s, e) =>
            {
                this.BeginInvoke(new Action(() =>
                {
                    LoadMaterialChart();
                    LoadExpenseChart();
                }));
            };

            // Load all dashboard KPIs and summaries
            LoadWorkers();
            LoadRevenue();
            LoadProduction();
            LoadWorkerCategoryData();
            LoadRawMaterialCards();
        }

        // TOTAL WORKERS KPI
        private void LoadWorkers()
        {
            string query = "SELECT COUNT(*) FROM Workers";

            int total = Convert.ToInt32(
                DBHelper.ExecuteScalar(query, null)
            );

            lblWorkers.Text = total.ToString();
        }

        // TOTAL REVENUE / EXPENSES KPI
        private void LoadRevenue()
        {
            string query = "SELECT ISNULL(SUM(Amount),0) FROM Payments";

            decimal total = Convert.ToDecimal(
                DBHelper.ExecuteScalar(query, null)
            );

            lblExpenses.Text = "Rs " + total.ToString("N0");
        }

        // TOTAL RAW MATERIAL / PRODUCTION KPI
        private void LoadProduction()
        {
            string query =
                "SELECT ISNULL(SUM(Quantity), 0) AS TotalRawMaterial FROM RawMaterial;";

            int total = Convert.ToInt32(
                DBHelper.ExecuteScalar(query, null)
            );

            lblProduction.Text = total.ToString();
        }

        // MATERIAL CHART: MATERIAL USAGE DISTRIBUTION
        private void LoadMaterialChart()
        {
            materialChart.Plot.Clear();

            DataTable dt = DBHelper.ExecuteDataTable(@"
        SELECT 
            MaterialName,
            SUM(QuantityUsed) AS TotalUsed
        FROM MaterialUsage
        WHERE 
            MONTH(Date) = MONTH(GETDATE())
            AND YEAR(Date) = YEAR(GETDATE())
        GROUP BY MaterialName
    ", null);

            double grandTotal = 0;

            foreach (DataRow r in dt.Rows)
            {
                grandTotal += Convert.ToDouble(r["TotalUsed"]);
            }

            List<double> values = new();
            List<string> labels = new();

            foreach (DataRow r in dt.Rows)
            {
                string material = r["MaterialName"].ToString();
                double used = Convert.ToDouble(r["TotalUsed"]);

                double percent = grandTotal > 0
                    ? (used / grandTotal) * 100
                    : 0;

                values.Add(percent);
                labels.Add(material);
            }

            // Use a bar chart instead of a pie chart for material usage
            materialChart.Plot.Add.Bars(values.ToArray());

            var tickGenMat = new ScottPlot.TickGenerators.NumericManual();
            for (int i = 0; i < labels.Count; i++)
            {
                tickGenMat.AddMajor(i, labels[i]);
            }

            if (labels.Count > 0)
                materialChart.Plot.Axes.Bottom.TickGenerator = tickGenMat;

            // Ensure Y axis shows percent range 0-100 by using manual ticks
            var yTickMat = new ScottPlot.TickGenerators.NumericManual();
            for (int v = 0; v <= 100; v += 10)
            {
                yTickMat.AddMajor(v, v.ToString());
            }
            materialChart.Plot.Axes.Left.TickGenerator = yTickMat;

            materialChart.Plot.Title("Monthly Material Usage");
            materialChart.Refresh();
        }

        // EXPENSE DISTRIBUTION CHART (Monthly)
        private void LoadExpenseChart()
        {
            expenseChart.Plot.Clear();

            DataTable dt = DBHelper.ExecuteDataTable(@"
        SELECT 
            UPPER(LTRIM(RTRIM(Reason))) AS Reason,
            SUM(Amount) AS TotalAmount
        FROM Payments
        WHERE 
            MONTH(Date) = MONTH(GETDATE())
            AND YEAR(Date) = YEAR(GETDATE())
        GROUP BY UPPER(LTRIM(RTRIM(Reason)))
    ", null);

            double grandTotal = 0;

            foreach (DataRow row in dt.Rows)
            {
                grandTotal += Convert.ToDouble(row["TotalAmount"]);
            }

            List<double> values = new();
            List<string> labels = new();

            foreach (DataRow row in dt.Rows)
            {
                string reason = row["Reason"].ToString();
                double amount = Convert.ToDouble(row["TotalAmount"]);

                double percent = grandTotal > 0
                    ? (amount / grandTotal) * 100
                    : 0;

                values.Add(percent);
                labels.Add(reason);
            }

            // Use a bar chart instead of a pie chart for expense distribution
            expenseChart.Plot.Add.Bars(values.ToArray());

            var tickGenExp = new ScottPlot.TickGenerators.NumericManual();
            for (int i = 0; i < labels.Count; i++)
            {
                tickGenExp.AddMajor(i, labels[i]);
            }

            if (labels.Count > 0)
                expenseChart.Plot.Axes.Bottom.TickGenerator = tickGenExp;

            // Ensure Y axis shows percent range 0-100 by using manual ticks
            var yTickExp = new ScottPlot.TickGenerators.NumericManual();
            for (int v = 0; v <= 100; v += 10)
            {
                yTickExp.AddMajor(v, v.ToString());
            }
            expenseChart.Plot.Axes.Left.TickGenerator = yTickExp;

            expenseChart.Plot.Title("Monthly Expense Distribution");
            expenseChart.Refresh();
        }

        // WORKER CATEGORY COUNTS
        private void LoadWorkerCategoryData()
        {
            LoadWorkerCount(lblLabourers, "Labor");
            LoadWorkerCount(lblDrivers, "Driver");
            LoadWorkerCount(lblLoaders, "Loader");
            LoadWorkerCount(lblOperators, "Machine Operator");
        }

        // Generic method to get worker count by role
        private void LoadWorkerCount(System.Windows.Forms.Label lbl, string role)
        {
            string query = "SELECT COUNT(*) FROM Workers WHERE Role=@role";

            var parameters = new Microsoft.Data.SqlClient.SqlParameter[]
            {
                new Microsoft.Data.SqlClient.SqlParameter("@role", role)
            };

            int count = Convert.ToInt32(
                DBHelper.ExecuteScalar(query, parameters)
            );

            lbl.Text = count.ToString();
        }

        // RAW MATERIAL STOCK COUNTS
        private void LoadRawMaterialCards()
        {
            LoadMaterial(lblCement, "Cement");
            LoadMaterial(lblSand, "Sand");
            LoadMaterial(lblCrush, "Crush");
            LoadMaterial(lblSteel, "Steel");
            LoadMaterial(lblOil, "Mold Oil");
        }

        // Generic method to fetch stock of a material
        private void LoadMaterial(System.Windows.Forms.Label lbl, string material)
        {
            string query = @"
                SELECT ISNULL(SUM(Quantity),0)
                FROM RawMaterial
                WHERE UPPER(LTRIM(RTRIM(Name))) = UPPER(LTRIM(RTRIM(@name)))
            ";

            var parameters = new Microsoft.Data.SqlClient.SqlParameter[]
            {
                new Microsoft.Data.SqlClient.SqlParameter("@name", material)
            };

            int qty = Convert.ToInt32(
                DBHelper.ExecuteScalar(query, parameters)
            );

            lbl.Text = qty.ToString();
        }
    }
}