using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FactoryManagementSystem
{
    public partial class OwnerReportsPage : UserControl
    {
        public OwnerReportsPage()
        {
            InitializeComponent();
        }

        private void btnViewReport_Click(object sender, EventArgs e)
        {


            DateTime fromDate = dtFrom.Value.Date;
            DateTime toDate = dtTo.Value.Date;

            // Map factory names to IDs and table names
            int factoryId;
            string tableName;
            string connStr = @"Data Source=TALHA\SQLEXPRESS;Initial Catalog=MSBlockDB;Integrated Security=True;";

            try
            {
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    // Dynamically use the table name
                    string query = @"

                    SELECT ReportDate, ReportText
                    FROM DailyReports1
                    WHERE ManagerID = @fid
                    AND ReportDate BETWEEN @from AND @to
                    ORDER BY ReportDate ASC";


                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {

                        cmd.Parameters.AddWithValue("@from", fromDate);
                        cmd.Parameters.AddWithValue("@to", toDate);

                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.HasRows)
                            {
                                txtReport.Text = $"No reports found between {fromDate.ToShortDateString()} and {toDate.ToShortDateString()}.";
                                return;
                            }

                            txtReport.Clear();
                            while (reader.Read())
                            {
                                DateTime reportDate = reader.GetDateTime(0);
                                string reportText = reader.GetString(1);

                                txtReport.AppendText($"--- Report Date: {reportDate.ToShortDateString()} ---\r\n");
                                txtReport.AppendText(reportText + "\r\n\r\n");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching reports:\n" + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


    }
}
