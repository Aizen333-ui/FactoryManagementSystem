using System.Data;
using FactoryManagementCore;
namespace FactoryManagementSystem
{
    public partial class FactoryDash : UserControl
    {
        public FactoryDash()
        {
            InitializeComponent();

            LoadDashboardData();
            LoadPieChart();
            LoadProductionChart();
            LoadProductionOverview();
        }


        // KPI DATA
        private void LoadDashboardData()
        {
            try
            {
                object totalProduction =
                    DBHelper.ExecuteScalar(@"
                        SELECT ISNULL(SUM(Quantity),0)
                        FROM Production
                    ", null);

                lblTotalProduction.Text = totalProduction.ToString();


                object usage =
                    DBHelper.ExecuteScalar(@"
                        SELECT 
                        CAST(
                        (
                        ISNULL(SUM(mu.QuantityUsed),0) * 100.0
                        /
                        NULLIF(SUM(rm.Quantity),0)
                        )
                        AS DECIMAL(10,2)
                        )
                        FROM RawMaterial rm
                        LEFT JOIN MaterialUsage mu
                        ON rm.MaterialId = mu.MaterialId
                    ", null);


                lblRawUsagePercent.Text =
                    usage != DBNull.Value && usage != null
                    ? usage + "%"
                    : "0%";
            }
            catch
            {
                lblTotalProduction.Text = "0";
                lblRawUsagePercent.Text = "0%";
            }
        }



        // PRODUCTION CARDS
        private void LoadProductionOverview()
        {
            productionFlow.Controls.Clear();


            DataTable dt = DBHelper.ExecuteDataTable(@"
                SELECT ProductName,
                       SUM(Quantity) AS TotalQty
                FROM Production
                GROUP BY ProductName
            ", null);



            foreach (DataRow row in dt.Rows)
            {
                Panel card = new Panel
                {
                    Size = new Size(250, 120),
                    BackColor = System.Drawing.Color.White,
                    Margin = new Padding(10)
                };
                MakeRounded(card, 16);


                System.Windows.Forms.Label lblName = new System.Windows.Forms.Label
                {
                    Text = row["ProductName"].ToString(),
                    Font = new Font(
                        "Segoe UI",
                        12,
                        System.Drawing.FontStyle.Bold),
                    Location = new Point(15, 15),
                    AutoSize = true
                };


                System.Windows.Forms.Label lblQty = new System.Windows.Forms.Label
                {
                    Text = row["TotalQty"].ToString(),
                    Font = new Font(
                        "Segoe UI",
                        22,
                        System.Drawing.FontStyle.Bold),
                    Location = new Point(15, 55),
                    AutoSize = true
                };


                card.Controls.Add(lblName);
                card.Controls.Add(lblQty);

                productionFlow.Controls.Add(card);
            }
        }

        // BAR CHART - MATERIAL USAGE (replaced previous pie)
        private void LoadPieChart()
        {
            materialusageChart.Plot.Clear();

            DataTable dt = DBHelper.ExecuteDataTable(@"
                SELECT 
                    MaterialName,
                    SUM(QuantityUsed) AS TotalUsed
                FROM MaterialUsage
                WHERE 
                    MONTH(Date)=MONTH(GETDATE())
                    AND YEAR(Date)=YEAR(GETDATE())
                GROUP BY MaterialName
            ", null);

            List<double> values = new();
            List<string> labels = new();

            foreach (DataRow row in dt.Rows)
            {
                labels.Add(row["MaterialName"].ToString());
                values.Add(Convert.ToDouble(row["TotalUsed"]));
            }

            // Add bars and set up category ticks
            materialusageChart.Plot.Add.Bars(values.ToArray());

            var tickGen = new ScottPlot.TickGenerators.NumericManual();
            for (int i = 0; i < labels.Count; i++)
            {
                tickGen.AddMajor(i, labels[i]);
            }

            if (labels.Count > 0)
                materialusageChart.Plot.Axes.Bottom.TickGenerator = tickGen;

            materialusageChart.Plot.Title(
                "Material Usage"
            );

            materialusageChart.Refresh();
        }

        // BAR CHART - MONTHLY PRODUCTION
        private void LoadProductionChart()
        {
            monthlyproductionChart.Plot.Clear();

            DataTable dt = DBHelper.ExecuteDataTable(@"
                SELECT 
                    ProductName,
                    SUM(Quantity) AS Total
                FROM Production
                WHERE 
                    MONTH(Date)=MONTH(GETDATE())
                    AND YEAR(Date)=YEAR(GETDATE())
                GROUP BY ProductName
            ", null);

            List<double> values = new();
            List<string> labels = new();

            foreach (DataRow row in dt.Rows)
            {
                labels.Add(
                    row["ProductName"].ToString()
                );

                values.Add(
                    Convert.ToDouble(row["Total"])
                );
            }

            monthlyproductionChart.Plot.Add.Bars(
                values.ToArray()
            );

            monthlyproductionChart.Plot.Title(
                "Monthly Production"
            );

            var tickGen = new ScottPlot.TickGenerators.NumericManual();

            for (int i = 0; i < labels.Count; i++)
            {
                tickGen.AddMajor(i, labels[i]);
            }

            monthlyproductionChart.Plot.Axes.Bottom.TickGenerator = tickGen;

            monthlyproductionChart.Refresh();
        }
    }
}