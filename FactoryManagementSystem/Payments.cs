using System.Data;
using Microsoft.Data.SqlClient;

namespace FactoryManagementSystem
{
    public partial class Payments : UserControl
    {
        // Error provider for input validation
        ErrorProvider error = new ErrorProvider();

        // Constructor
        public Payments()
        {
            InitializeComponent();

            // Add payment reasons to combo box
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

            // Prevent manual typing in combo box
            cmbReason.DropDownStyle = ComboBoxStyle.DropDownList;

            // Load payment records into DataGridView
            LoadPayments();
        }

        // Load all payments from database
        private void LoadPayments()
        {
            try
            {
                // Fetch data from database
                DataTable dt = DBHelper.ExecuteDataTable(
                    "SELECT * FROM Payments ORDER BY PaymentID DESC",
                    null
                );

                // Bind data to grid
                dataGridView1.AutoGenerateColumns = true;
                dataGridView1.DataSource = dt;

                // Format amount column with currency
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

        // Automatically format amount text with Rs prefix
        private void TxtAmount_TextChanged(object sender, EventArgs e)
        {
            string text = txtAmount.Text.Replace("Rs", "").Trim();

            if (decimal.TryParse(text, out decimal amount))
            {
                // Prevent recursive event triggering
                txtAmount.TextChanged -= TxtAmount_TextChanged;

                txtAmount.Text = "Rs " + amount.ToString();

                // Move cursor to end
                txtAmount.SelectionStart = txtAmount.Text.Length;

                // Reattach event
                txtAmount.TextChanged += TxtAmount_TextChanged;
            }
        }

        // Add new payment record
        private void btnAdd_Click(object sender, EventArgs e)
        {
            string amountText = txtAmount.Text.Replace("Rs", "").Trim();
            DateTime date = datePaid.Value;

            // Validate amount
            if (!decimal.TryParse(amountText, out decimal amount) || amount <= 0)
            {
                MessageBox.Show("Enter valid amount.");
                return;
            }

            // Prevent future dates
            if (date.Date > DateTime.Today)
            {
                MessageBox.Show("Future date not allowed.");
                return;
            }

            try
            {
                // Insert payment into database
                string query =
                    "INSERT INTO Payments (Amount, Reason, Date) VALUES (@amount, @reason, @date)";

                SqlParameter[] p =
                {
                    new SqlParameter("@amount", amount),
                    new SqlParameter("@reason", cmbReason.SelectedItem.ToString()),
                    new SqlParameter("@date", date)
                };

                DBHelper.ExecuteNonQuery(query, p);

                MessageBox.Show("Payment added!");

                // Refresh grid
                LoadPayments();

                // Clear inputs
                txtAmount.Clear();
                cmbReason.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding: " + ex.Message);
            }
        }

        // Delete selected payment
        private void btnDelete_Click(object sender, EventArgs e)
        {
            // Ensure a row is selected
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Select a row to delete!");
                return;
            }

            // Get selected payment ID
            int id = Convert.ToInt32(
                dataGridView1.CurrentRow.Cells["PaymentID"].Value
            );

            // Confirmation message
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
                // Delete payment from database
                DBHelper.ExecuteNonQuery(
                    "DELETE FROM Payments WHERE PaymentID = @id",
                    new SqlParameter[] { new SqlParameter("@id", id) }
                );

                MessageBox.Show("Payment deleted!");

                // Refresh grid
                LoadPayments();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting: " + ex.Message);
            }
        }

        // Navigate back to dashboard home page
        private void btnBack_Click(object sender, EventArgs e)
        {
            OwnerDashBoard dashboard =
                (OwnerDashBoard)this.FindForm();

            dashboard.LoadPage(new OwnerHomePage());
        }
    }
}