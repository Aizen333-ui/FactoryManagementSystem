using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Linq;

namespace FactoryManagementSystem
{
    public partial class WorkersAddandView : UserControl
    {
        ErrorProvider error = new ErrorProvider();
        private bool isEditing = false;

        public WorkersAddandView()
        {
            InitializeComponent();

            // Roles
            cmbRole.Items.AddRange(new object[]
            {
                "Labor",
                "Driver",
                "Loader",
                "Machine Operator"
            });

            cmbRole.DropDownStyle = ComboBoxStyle.DropDownList;

            LoadWorkers();
        }

        // Load Workers from DB
        private void LoadWorkers()
        {
            try
            {
                string query = "SELECT WorkerID, Name, Role, Wage FROM Workers ORDER BY WorkerID DESC";
                DataTable dt = DbHelper.ExecuteDataTable(query, null);
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading workers: " + ex.Message);
            }
        }

        // Auto "Rs " in wage
        private void TxtWage_TextChanged(object sender, EventArgs e)
        {
            if (isEditing) return;

            isEditing = true;
            string txt = txtWage.Text.Replace("Rs", "").Replace(" ", "");

            if (decimal.TryParse(txt, out decimal wage))
            {
                txtWage.Text = "Rs " + wage.ToString();
                txtWage.SelectionStart = txtWage.Text.Length;
            }

            isEditing = false;
        }

        // ADD WORKER
        private void btnAdd_Click(object sender, EventArgs e)
        {
            error.Clear();

            string name = txtName.Text.Trim();
            string role = cmbRole.Text.Trim();
            string wageTxt = txtWage.Text.Replace("Rs", "").Trim();

            // NAME VALIDATION
            if (!Regex.IsMatch(name, @"^[A-Za-z ]{3,30}$"))
            {
                MessageBox.Show("Worker name must be alphabets only (3–30 chars).");
                return;
            }

            // ROLE VALIDATION
            if (string.IsNullOrEmpty(role))
            {
                MessageBox.Show("Please select worker role.");
                return;
            }

            // WAGE VALIDATION
            if (!decimal.TryParse(wageTxt, out decimal wage) || wage < 200 || wage > 5000)
            {
                MessageBox.Show("Daily wage must be between 200 and 5000 PKR.");
                return;
            }

            // INSERT INTO DATABASE
            try
            {
                string query = "INSERT INTO Workers (Name, Role, Wage) VALUES (@Name, @Role, @Wage)";

                SqlParameter[] p =
                {
                    new SqlParameter("@Name", name),
                    new SqlParameter("@Role", role),
                    new SqlParameter("@Wage", wage)
                };

                DbHelper.ExecuteNonQuery(query, p);

                MessageBox.Show("Worker added successfully!");
                LoadWorkers();

                txtName.Clear();
                txtWage.Clear();
                cmbRole.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding worker: " + ex.Message);
            }
        }

        // REMOVE WORKER
        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select a worker to remove.");
                return;
            }

            int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["WorkerID"].Value);

            try
            {
                string query = "DELETE FROM Workers WHERE WorkerID = @id";

                SqlParameter[] p =
                {
                    new SqlParameter("@id", id)
                };

                DbHelper.ExecuteNonQuery(query, p);

                MessageBox.Show("Worker removed!");
                LoadWorkers();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error removing worker: " + ex.Message);
            }
        }
    }
}
