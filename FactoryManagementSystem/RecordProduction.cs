using FactoryManagementSystem;
using Microsoft.Data.SqlClient;
using System.Data;
using FactoryManagementCore;
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

            // Prevent future production dates
            dateProduction.MaxDate = DateTime.Today;

            // Load existing production records
            LoadProduction();

            cmbProductName.SelectedIndexChanged += (s, e) =>
            {
                txtUnit.Text = GetUnitForProduct(
                    cmbProductName.SelectedItem?.ToString() ?? ""
                );
            };
        }

        // Load product list into ComboBox
        private void LoadProductOptions()
        {
            cmbProductName.Items.AddRange(new object[]
            {
                "Select the product...",
                "Tuff Tile",
                "Kurbstone",
                "Paver Block",
                "Hollow Block",
                "Solid Block"
            });

            cmbProductName.SelectedIndex = 0;
        }

        private string GetUnitForProduct(string product)
        {
            return product switch
            {
                "Tuff Tile" => "Sqft",
                "Kurbstone" => "Piece",
                "Paver Block" => "Piece",
                "Hollow Block" => "Piece",
                "Solid Block" => "Piece",
                _ => ""
            };
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
        
        // Save production record
        private void BtnSave_Click(object? sender, EventArgs e)
        {
            // Validate product selection
            if (cmbProductName.SelectedIndex <= 0)
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
                Logger.AddLog(
                    Session.CurrentUser,
                    "CREATE",
                    "Production",
                    $"Recorded production of {quantity} {txtUnit.Text} for product '{cmbProductName.Text}'",
                    "Success"
                );
                MessageBox.Show("Production record saved successfully!");

                // Refresh grid after insert
                LoadProduction();

                // Clear form fields
                ClearFields();
            }
            catch (Exception ex)
            {
                Logger.AddLog(
                    Session.CurrentUser,
                    "CREATE",
                    "Production",
                    $"Failed to record production for '{cmbProductName.Text}'. Error: {ex.Message}",
                    "Failed"
                );
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
            cmbProductName.SelectedIndex = 0;
            txtUnit.Clear();
            txtQuantity.Clear();
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
                dashboard.ResetSidebarSelection(); 

                dashboard.LoadPage(new FactoryDash());
            }
        }
    }
}