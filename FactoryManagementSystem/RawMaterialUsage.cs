using FactoryManagementSystem;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace FactoryDashboard.Pages
{
    public partial class RawMaterialUsage : UserControl
    {
        public RawMaterialUsage()
        {
            InitializeComponent();

            
            btnClear.Click += BtnClear_Click;
            btnRemove.Click += BtnRemove_Click;

            LoadMaterialOptions();
            dateMaterial.MaxDate = DateTime.Today;
            LoadRawMaterials();
        }
        private void LoadRawMaterials()
        {
            try
            {
                string query = @"
            SELECT MaterialID, Name, Quantity
            FROM RawMaterial";

                DataTable dt = DBHelper.ExecuteDataTable(query, null);

                dataGridView.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading materials: " + ex.Message);
            }
        }
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
            cmbMaterialName.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        // GET MATERIAL ID FROM DB
        private int GetMaterialId(string name)
        {
            object res = DBHelper.ExecuteScalar(
                "SELECT MaterialID FROM RawMaterial WHERE Name = @n",
                new SqlParameter[] { new SqlParameter("@n", name) }
            );

            if (res == null)
                throw new Exception("Material not found in database");

            return Convert.ToInt32(res);
        }
       
        private void BtnClear_Click(object? sender, EventArgs e)
        {
            ClearFields();
            LoadRawMaterials();
        }

        private void ClearFields()
        {
            cmbMaterialName.SelectedIndex = -1;
            txtQuantity.Clear();
            dateMaterial.Value = DateTime.Today;
        }

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

        private void BtnRemove_Click(object? sender, EventArgs e)
        {
            string material = cmbMaterialName.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(material))
            {
                MessageBox.Show("Select a material to remove.");
                return;
            }

            if (!int.TryParse(txtQuantity.Text.Trim(), out int quantity) || quantity <= 0)
            {
                MessageBox.Show("Enter a valid quantity to remove.");
                txtQuantity.Focus();
                return;
            }

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
                int materialId = GetMaterialId(material);

                string query =
                    "UPDATE RawMaterial SET Quantity = Quantity - @q WHERE MaterialID=@id AND Quantity>=@q";

                SqlParameter[] p =
                {
                    new SqlParameter("@q", quantity),
                    new SqlParameter("@id", materialId)
                };

                int rows = DBHelper.ExecuteNonQuery(query, p);

                if (rows > 0)
                {
                    MessageBox.Show("Raw material entry removed successfully!");
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
       
    }
}