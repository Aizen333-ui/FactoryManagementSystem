using Microsoft.Data.SqlClient;
using System.Data;
using System.Text.RegularExpressions;

namespace FactoryManagementSystem
{
    public partial class WorkersAddandView : UserControl
    {
        // ErrorProvider for input validation feedback
        ErrorProvider error = new ErrorProvider();

        // Flag to prevent recursive text formatting in wage field
        private bool isEditing = false;

        public WorkersAddandView()
        {
            InitializeComponent();

            // Initialize worker roles dropdown
            cmbRole.Items.AddRange(new object[]
            {
                "Labor",
                "Driver",
                "Loader",
                "Machine Operator"
            });

            cmbRole.DropDownStyle = ComboBoxStyle.DropDownList;

            // Load existing workers from database
            LoadWorkers();
        }

        // ================= LOAD WORKERS =================
        private void LoadWorkers()
        {
            try
            {
                string query = "SELECT * FROM Workers ORDER BY WorkerID DESC";
                DataTable dt = DBHelper.ExecuteDataTable(query, null);

                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading workers: " + ex.Message);
            }
        }

        // ================= AUTO FORMAT WAGE =================
        // Adds "Rs" prefix while typing salary
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

        // ================= ADD WORKER =================
        private void btnAdd_Click(object sender, EventArgs e)
        {
            error.Clear();

            string name = txtName.Text.Trim();
            string role = cmbRole.Text.Trim();
            string wageTxt = txtWage.Text.Replace("Rs", "").Trim();

            // Validate name (only alphabets, 3–30 chars)
            if (!Regex.IsMatch(name, @"^[A-Za-z ]{3,30}$"))
            {
                MessageBox.Show("Worker name must be alphabets only (3–30 chars).");
                return;
            }

            // Validate role selection
            if (string.IsNullOrEmpty(role))
            {
                MessageBox.Show("Please select worker role.");
                return;
            }

            // Validate wage range
            if (!decimal.TryParse(wageTxt, out decimal wage) || wage < 200 || wage > 5000)
            {
                MessageBox.Show("Daily wage must be between 200 and 5000 PKR.");
                return;
            }

            try
            {
                // Insert worker into database
                string query = "INSERT INTO Workers (Name, Role, Salary) VALUES (@Name, @Role, @Wage)";

                SqlParameter[] p =
                {
                    new SqlParameter("@Name", name),
                    new SqlParameter("@Role", role),
                    new SqlParameter("@Wage", wage)
                };

                DBHelper.ExecuteNonQuery(query, p);

                MessageBox.Show("Worker added successfully!");

                // Refresh grid after insertion
                LoadWorkers();

                // Clear form fields
                txtName.Clear();
                txtWage.Clear();
                cmbRole.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding worker: " + ex.Message);
            }
        }

        // ================= REMOVE WORKER =================
        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Select a worker to remove.");
                return;
            }

            int id = Convert.ToInt32(
                dataGridView1.CurrentRow.Cells["WorkerID"].Value
            );

            // Confirmation before deletion
            DialogResult dr = MessageBox.Show(
                "Are you sure you want to delete this worker?",
                "Confirm",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (dr != DialogResult.Yes)
                return;

            try
            {
                string query = "DELETE FROM Workers WHERE WorkerID = @id";

                SqlParameter[] p =
                {
                    new SqlParameter("@id", id)
                };

                DBHelper.ExecuteNonQuery(query, p);

                MessageBox.Show("Worker removed!");

                // Refresh grid after deletion
                LoadWorkers();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error removing worker: " + ex.Message);
            }
        }

        // ================= BACK BUTTON =================
        private void btnBack_Click(object sender, EventArgs e)
        {
            OwnerDashBoard dashboard =
                (OwnerDashBoard)this.FindForm();
            dashboard.ResetSidebarSelection(); // 🔥 FIX ADDED

            // Navigate back to owner home page
            dashboard.LoadPage(new OwnerDash());
        }
    }
}