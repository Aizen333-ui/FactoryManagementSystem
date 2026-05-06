using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace FactoryDashboard.Pages
{
    public partial class Report : UserControl
    {
        string connStr =
            "Data Source=TALHA-SOHAIL\\SQLEXPRESS;Initial Catalog=FactoryDB;Integrated Security=True;TrustServerCertificate=True";

        public Report()
        {
            InitializeComponent();
            btnGenerate.Click += BtnGenerate_Click;
        }

        private void BtnGenerate_Click(object sender, EventArgs e)
        {
            DateTime from = dateFrom.Value.Date;
            DateTime to = dateTo.Value.Date;

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

                    // ================= MATERIAL USAGE =================
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

                    // ================= PRODUCTION =================
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

                dataGridReport.DataSource = dt;

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
    }
}