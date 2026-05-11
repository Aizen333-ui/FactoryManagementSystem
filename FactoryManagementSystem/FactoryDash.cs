// ======================================================
// FactoryDash.cs
// ======================================================

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

            LoadDashboardData();
            LoadPieChart();
            LoadProductionChart();
            LoadProductionOverview();
        }

        // ======================================================
        // LOAD KPI DATA
        // ======================================================

        private void LoadDashboardData()
        {
            try
            {
                // TOTAL PRODUCTION

                object totalProduction =
                    DBHelper.ExecuteScalar(@"
                        SELECT ISNULL(SUM(Quantity),0)
                        FROM Production
                    ", null);

                lblTotalProduction.Text =
                    totalProduction.ToString();

                // RAW MATERIAL %

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

                if (usage != DBNull.Value && usage != null)
                {
                    lblRawUsagePercent.Text =
                        usage.ToString() + "%";
                }
                else
                {
                    lblRawUsagePercent.Text = "0%";
                }
            }
            catch
            {
                lblTotalProduction.Text = "0";
                lblRawUsagePercent.Text = "0%";
            }
        }
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
                Panel card = new Panel();

                card.Size = new Size(250, 120);
                card.BackColor = Color.White;
                card.Margin = new Padding(10);

                MakeRounded(card, 12);

                Label lblName = new Label();

                lblName.Text =
                    row["ProductName"].ToString();

                lblName.Font =
                    new Font("Segoe UI", 12F, FontStyle.Bold);

                lblName.Location = new Point(15, 15);
                lblName.AutoSize = true;

                Label lblQty = new Label();

                lblQty.Text =
                    row["TotalQty"].ToString();

                lblQty.Font =
                    new Font("Segoe UI", 22F, FontStyle.Bold);

                lblQty.Location = new Point(15, 55);
                lblQty.AutoSize = true;

                card.Controls.Add(lblName);
                card.Controls.Add(lblQty);

                productionFlow.Controls.Add(card);
            }
        }
        // ======================================================
        // PIE CHART
        // ======================================================

        private void LoadPieChart()
        {
            pieChart.Series.Clear();
            pieChart.ChartAreas.Clear();
            pieChart.Legends.Clear();

            ChartArea area = new ChartArea("Main");
            area.BackColor = Color.White;

            pieChart.ChartAreas.Add(area);

            Legend lg = new Legend();
            lg.Docking = Docking.Bottom;
            lg.Font = new Font("Segoe UI", 9F);

            pieChart.Legends.Add(lg);

            Series s = new Series("Materials");

            s.ChartType = SeriesChartType.Pie;

            s.IsValueShownAsLabel = true;

            s.LabelFormat = "0.##'%'";
            s.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

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

            foreach (DataRow r in dt.Rows)
            {
                string material = r["MaterialName"].ToString();

                double used =
                    Convert.ToDouble(r["TotalUsed"]);

                double percent =
                    (used / grandTotal) * 100;

                s.Points.AddXY(material, percent);
            }

            pieChart.Series.Add(s);
        }

        // ======================================================
        // BAR CHART
        // ======================================================

        private void LoadProductionChart()
        {
            barChart.Series.Clear();
            barChart.ChartAreas.Clear();
            barChart.Legends.Clear();

            ChartArea area = new ChartArea("Main");

            area.BackColor = Color.White;

            barChart.ChartAreas.Add(area);

            Legend lg = new Legend();

            lg.Docking = Docking.Bottom;
            lg.Font = new Font("Segoe UI", 9F);

            barChart.Legends.Add(lg);

            Series s = new Series("Production");

            s.ChartType = SeriesChartType.Pie;

            s.IsValueShownAsLabel = true;

            s.LabelFormat = "0.##'%'";

            s.Font =
                new Font("Segoe UI", 9F, FontStyle.Bold);

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

            foreach (DataRow row in dt.Rows)
            {
                grandTotal +=
                    Convert.ToDouble(row["Total"]);
            }

            foreach (DataRow row in dt.Rows)
            {
                string product =
                    row["ProductName"].ToString();

                double total =
                    Convert.ToDouble(row["Total"]);

                double percent =
                    (total / grandTotal) * 100;

                s.Points.AddXY(product, percent);
            }

            barChart.Series.Add(s);
        }
    }
}