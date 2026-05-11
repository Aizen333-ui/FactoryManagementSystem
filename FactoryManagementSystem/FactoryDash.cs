using System.Data;
using System.Windows.Forms.DataVisualization.Charting;

namespace FactoryManagementSystem
{
    public partial class FactoryDash : UserControl
    {
        // Constructor: initializes UI and loads all dashboard data
        public FactoryDash()
        {
            InitializeComponent();

            LoadDashboardData();
            LoadPieChart();
            LoadProductionChart();
            LoadProductionOverview();
        }

        // LOAD KPI DATA (Top dashboard statistics)
        private void LoadDashboardData()
        {
            try
            {
                // Fetch total production quantity from database
                object totalProduction =
                    DBHelper.ExecuteScalar(@"
                        SELECT ISNULL(SUM(Quantity),0)
                        FROM Production
                    ", null);

                lblTotalProduction.Text = totalProduction.ToString();

                // Calculate raw material usage percentage
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

                // Display usage safely
                if (usage != DBNull.Value && usage != null)
                {
                    lblRawUsagePercent.Text = usage.ToString() + "%";
                }
                else
                {
                    lblRawUsagePercent.Text = "0%";
                }
            }
            catch
            {
                // Fallback values in case of database error
                lblTotalProduction.Text = "0";
                lblRawUsagePercent.Text = "0%";
            }
        }

        // PRODUCTION OVERVIEW CARDS (Per product summary)
        private void LoadProductionOverview()
        {
            productionFlow.Controls.Clear();

            // Get total production per product
            DataTable dt = DBHelper.ExecuteDataTable(@"
                SELECT ProductName,
                       SUM(Quantity) AS TotalQty
                FROM Production
                GROUP BY ProductName
            ", null);

            // Create a small card for each product
            foreach (DataRow row in dt.Rows)
            {
                Panel card = new Panel
                {
                    Size = new Size(250, 120),
                    BackColor = Color.White,
                    Margin = new Padding(10)
                };

                MakeRounded(card, 12);

                // Product name label
                Label lblName = new Label
                {
                    Text = row["ProductName"].ToString(),
                    Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                    Location = new Point(15, 15),
                    AutoSize = true
                };

                // Quantity label (large display)
                Label lblQty = new Label
                {
                    Text = row["TotalQty"].ToString(),
                    Font = new Font("Segoe UI", 22F, FontStyle.Bold),
                    Location = new Point(15, 55),
                    AutoSize = true
                };

                card.Controls.Add(lblName);
                card.Controls.Add(lblQty);

                productionFlow.Controls.Add(card);
            }
        }

        // PIE CHART (Material usage distribution)
        private void LoadPieChart()
        {
            pieChart.Series.Clear();
            pieChart.ChartAreas.Clear();
            pieChart.Legends.Clear();

            // Chart area setup
            ChartArea area = new ChartArea("Main");
            area.BackColor = Color.White;
            pieChart.ChartAreas.Add(area);

            // Legend configuration
            Legend lg = new Legend
            {
                Docking = Docking.Bottom,
                Font = new Font("Segoe UI", 9F)
            };
            pieChart.Legends.Add(lg);

            // Pie series setup
            Series s = new Series("Materials")
            {
                ChartType = SeriesChartType.Pie,
                IsValueShownAsLabel = true,
                LabelFormat = "0.##'%'",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            };

            // Fetch material usage for current month
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

            // Calculate total usage
            foreach (DataRow r in dt.Rows)
            {
                grandTotal += Convert.ToDouble(r["TotalUsed"]);
            }

            // Add percentage values to chart
            foreach (DataRow r in dt.Rows)
            {
                string material = r["MaterialName"].ToString();
                double used = Convert.ToDouble(r["TotalUsed"]);
                double percent = (used / grandTotal) * 100;

                s.Points.AddXY(material, percent);
            }

            pieChart.Series.Add(s);
        }

        // BAR CHART (Monthly production by product)
        private void LoadProductionChart()
        {
            barChart.Series.Clear();
            barChart.ChartAreas.Clear();
            barChart.Legends.Clear();

            // Chart area setup
            ChartArea area = new ChartArea("Main");
            area.BackColor = Color.White;
            barChart.ChartAreas.Add(area);

            // Legend setup
            Legend lg = new Legend
            {
                Docking = Docking.Bottom,
                Font = new Font("Segoe UI", 9F)
            };
            barChart.Legends.Add(lg);

            // Series setup (NOTE: currently Pie type but used for bar-like display logic)
            Series s = new Series("Production")
            {
                ChartType = SeriesChartType.Pie,
                IsValueShownAsLabel = true,
                LabelFormat = "0.##'%'",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            };

            // Fetch monthly production data
            DataTable dt = DBHelper.ExecuteDataTable(@"
                SELECT 
                    ProductName,
                    SUM(Quantity) AS Total
                FROM Production
                WHERE 
                    MONTH(Date) = MONTH(GETDATE())
                    AND YEAR(Date) = YEAR(GETDATE())
                GROUP BY ProductName
            ", null);

            double grandTotal = 0;

            // Calculate total production
            foreach (DataRow row in dt.Rows)
            {
                grandTotal += Convert.ToDouble(row["Total"]);
            }

            // Convert to percentage and add to chart
            foreach (DataRow row in dt.Rows)
            {
                string product = row["ProductName"].ToString();
                double total = Convert.ToDouble(row["Total"]);
                double percent = (total / grandTotal) * 100;

                s.Points.AddXY(product, percent);
            }

            barChart.Series.Add(s);
        }
    }
}