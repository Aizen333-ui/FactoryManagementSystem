using System;
using System.Data;
using System.Text.RegularExpressions;
using Microsoft.Data.SqlClient;
using System.Windows.Forms;

namespace FactoryManagementSystem
{
    public partial class Payments : UserControl
    {
        ErrorProvider error = new ErrorProvider();

        public Payments()
        {
            InitializeComponent();

            cmbReason.Items.AddRange(new object[]
            {
                "Cement Purchase",
                "Sand Purchase",
                "Crush Purchase",
                "Steel Purchase",
                "Worker Salary",
                "Diesel Expense",
                "Machine Maintenance",
                "Factory Rent",
                "Other Expense"
            });

            cmbReason.DropDownStyle = ComboBoxStyle.DropDownList;

            LoadPayments();
        }

        private bool PaymentIdExists(int id)
        {
            object result = DBHelper.ExecuteScalar("SELECT COUNT(*) FROM Payments WHERE PaymentID = @id",
                new SqlParameter[] { new SqlParameter("@id", id) });
            return result != null && Convert.ToInt32(result) > 0;
        }

        private int GeneratePaymentId()
        {
            try
            {
                object res = DBHelper.ExecuteScalar("SELECT MAX(CAST(PaymentID AS bigint)) FROM Payments", null);
                long maxId = 0;
                if (res != null && long.TryParse(res.ToString(), out long parsed))
                    maxId = parsed;

                int newId = (int)(maxId + 1);

                while (PaymentIdExists(newId))
                {
                    maxId++;
                    newId = (int)(maxId + 1);
                }

                return newId;
            }
            catch
            {
                var rnd = new Random();
                int alt;
                do
                {
                    alt = rnd.Next(100000, 999999);
                } while (PaymentIdExists(alt));
                return alt;
            }
        }

        private void LoadPayments()
        {
            try
            {
                DataTable dt = DBHelper.ExecuteDataTable("SELECT * FROM Payments ORDER BY PaymentID DESC", null);

                dataGridView1.AutoGenerateColumns = true;
                dataGridView1.DataSource = dt;

                if (dataGridView1.Columns.Contains("Amount"))
                {
                    dataGridView1.Columns["Amount"].DefaultCellStyle.Format = "'Rs '0.00";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading payments: " + ex.Message);
            }
        }

        private void TxtAmount_TextChanged(object sender, EventArgs e)
        {
            if (decimal.TryParse(txtAmount.Text.Replace("Rs", "").Trim(), out decimal amount))
            {
                txtAmount.Text = "Rs " + amount.ToString();
                txtAmount.SelectionStart = txtAmount.Text.Length;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string amountText = txtAmount.Text.Replace("Rs", "").Trim();
            DateTime date = datePaid.Value;

            if (!decimal.TryParse(amountText, out decimal amount) || amount <= 0)
            {
                MessageBox.Show("Enter valid amount.");
                return;
            }

            if (date.Date > DateTime.Today)
            {
                MessageBox.Show("Future date not allowed.");
                return;
            }

            try
            {
                // ✅ ID auto-generate hogi DB me (IDENTITY)
                string query =
                    "INSERT INTO Payments (Amount, Date) VALUES (@amount, @date)";

                SqlParameter[] p =
                {
                    new SqlParameter("@amount", amount),
                    new SqlParameter("@date", date)
                };

                DBHelper.ExecuteNonQuery(query, p);

                MessageBox.Show("Payment added!");

                LoadPayments();

                txtAmount.Clear();
                cmbReason.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding: " + ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select a row to delete!");
                return;
            }

            int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["PaymentID"].Value);

            DialogResult dr = MessageBox.Show(
                "Are you sure you want to delete this payment?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (dr != DialogResult.Yes)
                return;

            try
            {
                DBHelper.ExecuteNonQuery("DELETE FROM Payments WHERE PaymentID = @id",
                    new SqlParameter[] { new SqlParameter("@id", id) });

                MessageBox.Show("Payment deleted!");
                LoadPayments();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting: " + ex.Message);
            }
        }
    }
}