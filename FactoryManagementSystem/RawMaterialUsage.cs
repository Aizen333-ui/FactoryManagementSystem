using FactoryManagementSystem;
using System;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace FactoryDashboard.Pages
{
    public partial class RawMaterialUsage : UserControl
    {
        public RawMaterialUsage()
        {
            InitializeComponent();

            btnSave.Click += BtnSave_Click;
            btnClear.Click += BtnClear_Click;
            btnRemove.Click += BtnRemove_Click;

            LoadMaterialOptions();
            dateMaterial.MaxDate = DateTime.Today;
        }

        private void LoadMaterialOptions()
        {
            cmbMaterialName.Items.Clear();
            cmbMaterialName.Items.AddRange(new object[]
            {
                "Cement",
                "Sand",
                "Gravel",
                "Steel",
                "Bricks"
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

        // CHECK DUPLICATE
        private bool UsageExists(int materialId, int qty, DateTime date)
        {
            object res = DBHelper.ExecuteScalar(
                "SELECT COUNT(*) FROM MaterialUsage WHERE MaterialID=@id AND QuantityUsed=@q AND Date=@d",
                new SqlParameter[]
                {
                    new SqlParameter("@id", materialId),
                    new SqlParameter("@q", qty),
                    new SqlParameter("@d", date)
                });

            return res != null && Convert.ToInt32(res) > 0;
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            string material = cmbMaterialName.SelectedItem?.ToString();
            string quantityText = txtQuantity.Text.Trim();
            DateTime selectedDate = dateMaterial.Value.Date;

            if (string.IsNullOrEmpty(material))
            {
                MessageBox.Show("Please select a Material Name");
                cmbMaterialName.Focus();
                return;
            }

            if (!int.TryParse(quantityText, out int quantity) || quantity <= 0)
            {
                MessageBox.Show("Quantity must be a positive integer");
                txtQuantity.Focus();
                return;
            }

            if (selectedDate > DateTime.Today)
            {
                MessageBox.Show("Future date not allowed");
                return;
            }

            try
            {
                int materialId = GetMaterialId(material);

                if (UsageExists(materialId, quantity, selectedDate))
                {
                    MessageBox.Show("Entry already exists for this material, quantity and date");
                    return;
                }

                // ❌ UsageID hata diya (IDENTITY column)
                string query =
                    "INSERT INTO MaterialUsage (MaterialID, QuantityUsed, Date) VALUES (@mid,@q,@d)";

                SqlParameter[] p =
                {
                    new SqlParameter("@mid", materialId),
                    new SqlParameter("@q", quantity),
                    new SqlParameter("@d", selectedDate)
                };

                DBHelper.ExecuteNonQuery(query, p);

                MessageBox.Show("Raw Material Saved!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving: " + ex.Message);
            }
        }

        private void BtnClear_Click(object? sender, EventArgs e)
        {
            ClearFields();
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