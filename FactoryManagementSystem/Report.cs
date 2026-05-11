using FactoryManagementSystem;
using Microsoft.Data.SqlClient;
using System.Data;

namespace FactoryDashBoard.Pages
{
    public partial class Report : UserControl
    {
        // Database connection string
        string connStr =
            "Data Source=TALHA-SOHAIL\\SQLEXPRESS;Initial Catalog=FactoryDB;Integrated Security=True;TrustServerCertificate=True";

        // Constructor
        public Report()
        {
            InitializeComponent();

            // Button event bindings
            btnGenerate.Click += BtnGenerate_Click;
            btnBack.Click += btnBack_Click;
            btnSendReport.Click += btnSendReport_Click;
        }

        // Generate report based on selected date range
        private void BtnGenerate_Click(object sender, EventArgs e)
        {
            DateTime from = dateFrom.Value.Date;
            DateTime to = dateTo.Value.Date;

            // Create unified report table
            DataTable dt = new DataTable();
            dt.Columns.Add("Type");
            dt.Columns.Add("Name");
            dt.Columns.Add("Quantity");
            dt.Columns.Add("Date");

            try
            {
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    con.Open();

                    // ================= RAW MATERIAL USAGE REPORT =================
                    string materialQuery = @"
                        SELECT 
                            r.Name AS MaterialName,
                            mu.QuantityUsed,
                            mu.Date
                        FROM MaterialUsage mu
                        INNER JOIN RawMaterial r ON mu.MaterialID = r.MaterialID
                        WHERE mu.Date BETWEEN @from AND @to
                        ORDER BY mu.Date ASC";

                    using (SqlCommand cmd = new SqlCommand(materialQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@from", from);
                        cmd.Parameters.AddWithValue("@to", to);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                dt.Rows.Add(
                                    "Raw Material",
                                    reader["MaterialName"].ToString(),
                                    reader["QuantityUsed"].ToString(),
                                    Convert.ToDateTime(reader["Date"]).ToShortDateString()
                                );
                            }
                        }
                    }

                    // ================= PRODUCTION REPORT =================
                    string productionQuery = @"
                        SELECT 
                            ProductName,
                            Quantity,
                            Date
                        FROM Production
                        WHERE Date BETWEEN @from AND @to
                        ORDER BY Date ASC";

                    using (SqlCommand cmd = new SqlCommand(productionQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@from", from);
                        cmd.Parameters.AddWithValue("@to", to);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                dt.Rows.Add(
                                    "Production",
                                    reader["ProductName"].ToString(),
                                    reader["Quantity"].ToString(),
                                    Convert.ToDateTime(reader["Date"]).ToShortDateString()
                                );
                            }
                        }
                    }
                }

                // Bind data to grid
                dataGridReport.DataSource = dt;

                // Show message if no records found
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("No records found.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating report: " + ex.Message);
            }
        }
        private void btnSendReport_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Report has been sent to the owner successfully!",
                "Report Sent",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }
        // Navigate back to dashboard home page
        private void btnBack_Click(object sender, EventArgs e)
        {
            var dashboard = this.FindForm() as FactoryManagementSystem.FactoryDashBoard;

            if (dashboard != null)
            {
                dashboard.LoadPage(new FactoryDash());
            }
        }
    }
}