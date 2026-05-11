using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace FactoryManagementSystem
{
    public partial class FactoryDash : UserControl
    {
        public FactoryDash()
        {
            InitializeComponent();

            LoadPieChart();
            LoadProductionChart();
            LoadRawUsagePercent();
        }

        // ================= PIE CHART =================
        private void LoadPieChart()
        {
            pieChart.Series.Clear();
            pieChart.ChartAreas.Clear();

            pieChart.ChartAreas.Add(new ChartArea("Main"));

            Series s = new Series("Usage");
            s.ChartType = SeriesChartType.Pie;

            DataTable dt = DBHelper.ExecuteDataTable(@"
                SELECT MaterialName, SUM(QuantityUsed) AS TotalUsed
                FROM MaterialUsage
                GROUP BY MaterialName
            ", null);

            foreach (DataRow r in dt.Rows)
            {
                s.Points.AddXY(r["MaterialName"], r["TotalUsed"]);
            }

            pieChart.Series.Add(s);
        }

        // ================= BAR CHART =================
        private void LoadProductionChart()
        {
            barChart.Series.Clear();
            barChart.ChartAreas.Clear();

            barChart.ChartAreas.Add(new ChartArea("Main"));

            Series s = new Series("Production");
            s.ChartType = SeriesChartType.Column;

            DataTable dt = DBHelper.ExecuteDataTable(@"
                SELECT DATENAME(MONTH, Date) AS Month,
                       SUM(Quantity) AS Total
                FROM Production
                GROUP BY DATENAME(MONTH, Date)
            ", null);

            foreach (DataRow r in dt.Rows)
            {
                s.Points.AddXY(r["Month"], r["Total"]);
            }

            barChart.Series.Add(s);
        }

        // ================= RAW MATERIAL % =================
        private void LoadRawUsagePercent()
        {
            DataTable dt = DBHelper.ExecuteDataTable(@"
                SELECT
                    rm.MaterialId,
                    rm.Name,
                    SUM(rm.Quantity) AS TotalAdded,
                    ISNULL(SUM(mu.QuantityUsed), 0) AS TotalUsed,
                    SUM(rm.Quantity) - ISNULL(SUM(mu.QuantityUsed), 0) AS Remaining
                FROM RawMaterial rm
                LEFT JOIN MaterialUsage mu
                    ON rm.MaterialId = mu.MaterialId
                GROUP BY rm.MaterialId, rm.Name
                    ", null);

            foreach (DataRow row in dt.Rows)
            {
                string material = row["Name"].ToString();

                double used = Convert.ToDouble(row["TotalUsed"]);
                double total = Convert.ToDouble(row["TotalAdded"]);
                double remaining = Convert.ToDouble(row["Remaining"]);

              
            }
        }
    }
}