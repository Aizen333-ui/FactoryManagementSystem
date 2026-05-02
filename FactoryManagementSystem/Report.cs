using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FactoryDashboard.Pages
{
    public partial class Report : UserControl
    {
        // Your database connection string
        private readonly string connStr = @"Data Source=TALHA\SQLEXPRESS;Initial Catalog=MSBlockDB;Integrated Security=True;";


        public Report()
        {
            InitializeComponent();

            // Attach event handlers
            btnGenerate.Click += BtnGenerate_Click;
            btnSendReport.Click += BtnSendReport_Click;
        }


        // ----------------------------------------------------------
        // 1️⃣ GENERATE REPORT (SHOW IN DATAGRID)
        // ----------------------------------------------------------
        private void BtnGenerate_Click(object? sender, EventArgs e)
        {
            DateTime from = dateFrom.Value.Date;
            DateTime to = dateTo.Value.Date;

            DataTable dt = new DataTable();
            dt.Columns.Add("Type");
            dt.Columns.Add("Name");
            dt.Columns.Add("Quantity");
            dt.Columns.Add("Unit");
            dt.Columns.Add("Date");

            // ---------------- RAW MATERIAL DATA ----------------
            var rawList = GlobalStorage.RawMaterials
                .Where(x => x.Date.Date >= from && x.Date.Date <= to)
                .ToList();

            foreach (var r in rawList)
            {
                dt.Rows.Add(
                    "Raw Material",
                    r.MaterialName,
                    r.Quantity,
                    r.Unit,
                    r.Date.ToShortDateString()
                );
            }

            // ---------------- PRODUCTION DATA ----------------
            var prodList = GlobalStorage.Productions
                .Where(x => x.Date.Date >= from && x.Date.Date <= to)
                .ToList();

            foreach (var p in prodList)
            {
                dt.Rows.Add(
                    "Production",
                    p.ProductName,
                    p.Quantity,
                    p.Unit,
                    p.Date.ToShortDateString()
                );
            }

            // Display result in DataGrid
            dataGridReport.DataSource = dt;
        }



        // ----------------------------------------------------------
        // 2️⃣ SEND REPORT TO OWNER (SAVE IN SQL)
        // ----------------------------------------------------------
        private void BtnSendReport_Click(object? sender, EventArgs e)
        {
            if (dataGridReport.Rows.Count == 0)
            {
                MessageBox.Show("Generate a report before sending!", "Alert",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Convert DataGrid rows into a single text report
            string finalReport = "";

            foreach (DataGridViewRow row in dataGridReport.Rows)
            {
                if (!row.IsNewRow)
                {
                    finalReport +=
                        $"[{row.Cells["Type"].Value}]  " +
                        $"Name: {row.Cells["Name"].Value},  " +
                        $"Qty: {row.Cells["Quantity"].Value} {row.Cells["Unit"].Value},  " +
                        $"Date: {row.Cells["Date"].Value}\n";
                }
            }

            // If empty fail-safe
            if (string.IsNullOrWhiteSpace(finalReport))
            {
                MessageBox.Show("Report is empty!", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    string query =
                        @"INSERT INTO DailyReports1 (ManagerID, ReportDate, ReportText)
                      VALUES (@mid, @date, @text)";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@mid", 1); // Factory Manager ID
                        cmd.Parameters.AddWithValue("@date", DateTime.Now.Date);
                        cmd.Parameters.AddWithValue("@text", finalReport);

                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Report successfully sent to owner!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                dataGridReport.DataSource = null; // Clear after sending
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Error:\n" + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }


}
