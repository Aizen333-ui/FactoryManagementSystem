using FactoryManagementSystem;
using Microsoft.Data.SqlClient;
using System.Data;

namespace FactoryDashBoard.Pages
{
    public partial class RawMaterialUsage : UserControl
    {
        // Constructor
        public RawMaterialUsage()
        {
            InitializeComponent();

            // Attach button events
            btnClear.Click += BtnClear_Click;
            btnRemove.Click += BtnRemove_Click;
            btnBack.Click += btnBack_Click;

            // Load combo box items
            LoadMaterialOptions();

            // Restrict future dates
            dateMaterial.MaxDate = DateTime.Today;

            // Load raw materials into grid
            LoadRawMaterials();
        }

        // Load raw material records from database
        private void LoadRawMaterials()
        {
            try
            {
                string query = @"
                SELECT MaterialID, Name, Quantity
                FROM RawMaterial";

                // Execute query and store data
                DataTable dt = DBHelper.ExecuteDataTable(query, null);

                // Bind data to DataGridView
                dataGridView.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading materials: " + ex.Message);
            }
        }

        // Load material names into combo box
        private void LoadMaterialOptions()
        {
            cmbMaterialName.Items.Clear();

            cmbMaterialName.Items.AddRange(new object[]
            {
                "Cement",
                "Sand",
                "Crush",
                "Steel",
                "Mold Oil"
            });

            cmbMaterialName.SelectedIndex = -1;

            // Prevent manual typing
            cmbMaterialName.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        // Get material ID from database using material name
        private int GetMaterialId(string name)
        {
            object res = DBHelper.ExecuteScalar(
                "SELECT MaterialID FROM RawMaterial WHERE Name = @n",
                new SqlParameter[] { new SqlParameter("@n", name) }
            );

            // Check if material exists
            if (res == null)
                throw new Exception("Material not found in database");

            return Convert.ToInt32(res);
        }

        // Clear all input fields
        private void BtnClear_Click(object? sender, EventArgs e)
        {
            ClearFields();

            // Reload grid data
            LoadRawMaterials();
        }

        // Reset controls to default state
        private void ClearFields()
        {
            cmbMaterialName.SelectedIndex = -1;
            txtQuantity.Clear();
            dateMaterial.Value = DateTime.Today;
        }

        // Return unit according to selected material
        private string GetUnitForMaterial(string material)
        {
            return material switch
            {
                "Cement" => "Bag",
                "Sand" => "Ton",
                "Gravel" => "Ton",
                "Steel" => "Kg",
                "Bricks" => "Pieces",
                _ => ""
            };
        }

        // Remove quantity of selected raw material
        private void BtnRemove_Click(object? sender, EventArgs e)
        {
            // Get selected material name
            string material = cmbMaterialName.SelectedItem?.ToString();

            // Validate material selection
            if (string.IsNullOrEmpty(material))
            {
                MessageBox.Show("Select a material to remove.");
                return;
            }

            // Validate quantity input
            if (!int.TryParse(txtQuantity.Text.Trim(), out int quantity) || quantity <= 0)
            {
                MessageBox.Show("Enter a valid quantity to remove.");
                txtQuantity.Focus();
                return;
            }

            // Confirmation message
            DialogResult dr = MessageBox.Show(
                $"Remove {quantity} {GetUnitForMaterial(material)} of {material}?",
                "Confirm Remove",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (dr != DialogResult.Yes)
                return;

            try
            {
                // Fetch material ID
                int materialId = GetMaterialId(material);

                // Update material quantity
                string query =
                    "UPDATE RawMaterial SET Quantity = Quantity - @q " +
                    "WHERE MaterialID=@id AND Quantity>=@q";

                SqlParameter[] p =
                {
                    new SqlParameter("@q", quantity),
                    new SqlParameter("@id", materialId)
                };

                // Execute update query
                int rows = DBHelper.ExecuteNonQuery(query, p);

                // Check if update succeeded
                if (rows > 0)
                {
                    MessageBox.Show("Raw material entry removed successfully!");

                    // Refresh grid
                    LoadRawMaterials();
                }
                else
                {
                    MessageBox.Show("Material not found or insufficient quantity.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error removing: " + ex.Message);
            }
        }

        // Navigate back to dashboard home page
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