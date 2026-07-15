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
                "Cement",
                "Sand",
                "Crush",
                "Steel",
                "Mold Oil",               
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
        private Panel workerNameBox;
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
        private void cmbReason_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbReason.Text == "Worker Salary")
            {
                lblWorkerName.Visible = true;
                cmbWorkerName.Visible = true;
                workerNameBox.Visible = true;

                LoadWorkers();
            }
            else
            {
                lblWorkerName.Visible = false;
                cmbWorkerName.Visible = false;
                workerNameBox.Visible = false;

                cmbWorkerName.SelectedIndex = -1;
            }
        }
        private void LoadWorkers()
        {
            try
            {
                string query = "SELECT Name FROM Workers";

                DataTable dt = DBHelper.ExecuteDataTable(query, null);

                cmbWorkerName.Items.Clear();

                foreach (DataRow row in dt.Rows)
                {
                    cmbWorkerName.Items.Add(row["Name"].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading workers: " + ex.Message);
            }
        }
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
                string reason = cmbReason.SelectedItem.ToString();

                if (cmbReason.Text == "Worker Salary")
                {
                    if (cmbWorkerName.SelectedIndex == -1)
                    {
                        MessageBox.Show("Please select worker name.");
                        return;
                    }

                    reason = "Worker Salary - " + cmbWorkerName.Text;
                }

                // Insert payment into database
                string query =
                    "INSERT INTO Payments (Amount, Reason, Date) VALUES (@amount, @reason, @date)";

                SqlParameter[] p =
                {
                new SqlParameter("@amount", amount),
                new SqlParameter("@reason", reason),
                new SqlParameter("@date", date)
                };

                DBHelper.ExecuteNonQuery(query, p);

                MessageBox.Show("Payment added!");

                // Refresh grid
                LoadPayments();

                // Clear inputs
                txtAmount.Clear();
                cmbReason.SelectedIndex = -1;
                cmbWorkerName.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding: " + ex.Message);
            }
        }

        // Delete selected payment
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Select a payment row first!");
                return;
            }

            int paymentId = Convert.ToInt32(
                dataGridView1.CurrentRow.Cells["PaymentID"].Value
            );

            decimal currentAmount = Convert.ToDecimal(
                dataGridView1.CurrentRow.Cells["Amount"].Value
            );


            string enteredText = txtAmount.Text.Replace("Rs", "").Trim();
            string selectedReason = cmbReason.Text.Trim();

            string gridReason =
                dataGridView1.CurrentRow.Cells["Reason"].Value.ToString().Trim();


            if (!string.IsNullOrEmpty(selectedReason))
            {
                // For worker salary, compare only the main reason
                if (selectedReason == "Worker Salary")
                {
                    if (!gridReason.StartsWith("Worker Salary"))
                    {
                        MessageBox.Show(
                            "Selected reason does not match the selected payment.",
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);

                        return;
                    }
                }
                else
                {
                    if (gridReason != selectedReason)
                    {
                        MessageBox.Show(
                            "Selected reason does not match the selected payment.",
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);

                        return;
                    }
                }
            }

            try
            {
                // CASE 1: No amount entered -> delete whole payment
                if (string.IsNullOrWhiteSpace(enteredText))
                {
                    DialogResult confirm = MessageBox.Show(
                        "Delete this entire payment?",
                        "Confirm Delete",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning
                    );

                    if (confirm != DialogResult.Yes)
                        return;


                    DBHelper.ExecuteNonQuery(
                        "DELETE FROM Payments WHERE PaymentID=@id",
                        new SqlParameter[]
                        {
                    new SqlParameter("@id", paymentId)
                        });


                    MessageBox.Show("Payment deleted!");
                }


                // CASE 2: Amount entered -> remove only that amount
                else
                {
                    if (!decimal.TryParse(
                        enteredText,
                        out decimal removeAmount))
                    {
                        MessageBox.Show("Enter a valid amount.");
                        return;
                    }


                    if (removeAmount <= 0)
                    {
                        MessageBox.Show("Amount must be greater than zero.");
                        return;
                    }


                    if (removeAmount > currentAmount)
                    {
                        MessageBox.Show(
                            "Cannot remove more than the payment amount.");
                        return;
                    }


                    if (removeAmount == currentAmount)
                    {
                        // Amount equals payment -> delete whole row
                        DBHelper.ExecuteNonQuery(
                            "DELETE FROM Payments WHERE PaymentID=@id",
                            new SqlParameter[]
                            {
                        new SqlParameter("@id", paymentId)
                            });
                    }
                    else
                    {
                        // Partial removal
                        DBHelper.ExecuteNonQuery(
                            @"UPDATE Payments
                      SET Amount = Amount - @amount
                      WHERE PaymentID=@id",
                            new SqlParameter[]
                            {
                        new SqlParameter("@amount", removeAmount),
                        new SqlParameter("@id", paymentId)
                            });
                    }


                    MessageBox.Show("Payment updated!");
                }


                LoadPayments();
                txtAmount.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error deleting payment: " + ex.Message);
            }
        }
        // Navigate back to dashboard home page
        private void btnBack_Click(object sender, EventArgs e)
        {
            OwnerDashBoard dashboard =
                (OwnerDashBoard)this.FindForm();
            dashboard.ResetSidebarSelection(); 


            dashboard.LoadPage(new OwnerDash());
        }
    }
}