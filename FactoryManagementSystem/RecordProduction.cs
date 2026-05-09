using FactoryManagementSystem;
using Microsoft.Data.SqlClient;
using System.Data;

namespace FactoryDashBoard.Pages
{
    public partial class RecordProduction : UserControl
    {
        // Constructor
        public RecordProduction()
        {
            InitializeComponent();

            // Button event bindings
            btnSave.Click += BtnSave_Click;
            btnClear.Click += BtnClear_Click;
            btnBack.Click += btnBack_Click;

            // Restrict quantity input to numbers only
            txtQuantity.KeyPress += txtQuantity_KeyPress;

            // Load dropdown data
            LoadProductOptions();
            LoadUnitOptions();

            // Prevent future production dates
            dateProduction.MaxDate = DateTime.Today;

            // Load existing production records
            LoadProduction();
        }

        // Load product list into ComboBox
        private void LoadProductOptions()
        {
            cmbProductName.Items.AddRange(new object[]
            {
                "Tuff Tile",
                "Kurbstone",
                "Paver Block",
                "Hollow Block",
                "Solid Block"
            });

            cmbProductName.SelectedIndex = -1;
        }

        // Load unit options into ComboBox
        private void LoadUnitOptions()
        {
            cmbUnit.Items.AddRange(new object[]
            {
                "Pieces",
                "Sqft"
            });

            cmbUnit.SelectedIndex = 0;
        }

        // Load production records into DataGridView
        private void LoadProduction()
        {
            try
            {
                DataTable dt = DBHelper.ExecuteDataTable(
                    "SELECT * FROM Production ORDER BY ProductionID DESC",
                    null
                );

                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading production: " + ex.Message);
            }
        }

        // Check if production already exists for same product and date
        private bool ProductionExists(string product, DateTime date)
        {
            string query = "SELECT COUNT(*) FROM Production WHERE ProductName=@p AND Date=@d";

            object result = DBHelper.ExecuteScalar(query, new SqlParameter[]
            {
                new SqlParameter("@p", product),
                new SqlParameter("@d", date)
            });

            return result != null && Convert.ToInt32(result) > 0;
        }

        // Save production record
        private void BtnSave_Click(object? sender, EventArgs e)
        {
            // Validate product selection
            if (cmbProductName.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a product");
                cmbProductName.Focus();
                return;
            }

            // Validate quantity input
            if (string.IsNullOrWhiteSpace(txtQuantity.Text))
            {
                MessageBox.Show("Please enter quantity");
                txtQuantity.Focus();
                return;
            }

            // Validate numeric quantity
            if (!double.TryParse(txtQuantity.Text, out double quantity))
            {
                MessageBox.Show("Quantity must be numeric");
                txtQuantity.Focus();
                return;
            }

            // Validate quantity range
            if (quantity <= 0 || quantity > 10000)
            {
                MessageBox.Show("Quantity must be between 1 and 10000");
                txtQuantity.Focus();
                return;
            }

            DateTime selectedDate = dateProduction.Value.Date;

            // Prevent future date entry
            if (selectedDate > DateTime.Today)
            {
                MessageBox.Show("Future date not allowed");
                return;
            }

            // Prevent duplicate entries
            if (ProductionExists(cmbProductName.Text, selectedDate))
            {
                MessageBox.Show("Production for this product already recorded for selected date.");
                return;
            }

            try
            {
                // Insert production record
                string query =
                    "INSERT INTO Production (ProductName, Quantity, Date) VALUES (@p, @q, @d)";

                SqlParameter[] p =
                {
                    new SqlParameter("@p", cmbProductName.Text),
                    new SqlParameter("@q", quantity),
                    new SqlParameter("@d", selectedDate)
                };

                DBHelper.ExecuteNonQuery(query, p);

                MessageBox.Show("Production record saved successfully!");

                // Refresh grid after insert
                LoadProduction();

                // Clear form fields
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving production: " + ex.Message);
            }
        }

        // Clear button click
        private void BtnClear_Click(object? sender, EventArgs e)
        {
            ClearFields();
        }

        // Reset form fields
        private void ClearFields()
        {
            cmbProductName.SelectedIndex = -1;
            txtQuantity.Clear();
            cmbUnit.SelectedIndex = 0;
            dateProduction.Value = DateTime.Today;
        }

        // Restrict quantity textbox to numeric input only
        private void txtQuantity_KeyPress(object? sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) &&
                !char.IsDigit(e.KeyChar) &&
                e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }

        // Back button → return to dashboard home page
        private void btnBack_Click(object sender, EventArgs e)
        {
            var dashboard = this.FindForm() as FactoryManagementSystem.FactoryDashBoard;

            if (dashboard != null)
            {
                dashboard.LoadPage(new FactoryHomePage());
            }
        }
    }
}