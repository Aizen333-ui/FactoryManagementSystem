using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FactoryManagementSystem
{
    public partial class RawMaterial : UserControl
    {
        public RawMaterial()
        {
            InitializeComponent();
            // Ensure event handlers are attached (defensive in case designer wiring missed)
            this.btnAdd.Click -= btnAdd_Click;
            this.btnAdd.Click += btnAdd_Click;

            this.btnRemove.Click -= BtnRemove_Click;
            this.btnRemove.Click += BtnRemove_Click;

            this.cmbName.SelectedIndexChanged -= cmbName_SelectedIndexChanged;
            this.cmbName.SelectedIndexChanged += cmbName_SelectedIndexChanged;

            LoadMaterials();
        }


        // ----------------------------
        // LOAD MATERIALS INTO DATAGRID
        // ----------------------------
        private void LoadMaterials()
        {
            try
            {
                DataTable dt = DbHelper.ExecuteDataTable("SELECT * FROM RawMaterials ORDER BY MaterialID DESC");
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading materials: " + ex.Message);
            }
        }

        // ----------------------------
        // GET UNIT BASED ON MATERIAL
        // ----------------------------
        private string GetUnit(string materialName)
        {
            return materialName switch
            {
                "Cement" => "Bag",
                "Sand" => "Ton",
                "Crush" => "Ton",
                "Steel" => "KG",
                "Mold Oil" => "Liters",
                _ => ""
            };
        }

        private void cmbName_SelectedIndexChanged(object sender, EventArgs e)
        {
            // prefer selected item but fall back to text in case events fire differently
            string selected = cmbName.SelectedItem?.ToString() ?? cmbName.Text ?? "";
            txtUnit.Text = GetUnit(selected);
        }

        // ----------------------------
        // CHECK DUPLICATE MATERIAL ID
        // ----------------------------
        private bool MaterialIdExists(string id)
        {
            string query = "SELECT COUNT(*) FROM RawMaterials WHERE MaterialID = @id";
            object? result = DbHelper.ExecuteScalar(query, new SqlParameter[] { new SqlParameter("@id", id) });
            return result != null && Convert.ToInt32(result) > 0;
        }

        // Generate a new unique numeric MaterialID (1..10 digits)
        private string GenerateMaterialId()
        {
            // Try to generate an ID based on max existing ID + 1
            try
            {
                object? res = DbHelper.ExecuteScalar("SELECT MAX(CAST(MaterialID AS bigint)) FROM RawMaterials");
                long maxId = 0;
                if (res != null && long.TryParse(res.ToString(), out long parsed))
                    maxId = parsed;

                string newId = (maxId + 1).ToString();

                // ensure uniqueness (in rare race) by incrementing
                while (MaterialIdExists(newId))
                {
                    maxId++;
                    newId = (maxId + 1).ToString();
                }

                return newId;
            }
            catch
            {
                // fallback random 6-digit
                var rnd = new Random();
                string alt;
                do
                {
                    alt = rnd.Next(100000, 999999).ToString();
                } while (MaterialIdExists(alt));
                return alt;
            }
        }

        // ----------------------------
        // ADD MATERIAL
        // ----------------------------
        private void btnAdd_Click(object sender, EventArgs e)
        {
            string name = cmbName.Text.Trim();
            string qty = txtQty.Text.Trim();
            string unit = txtUnit.Text.Trim();
            DateTime date = dateAdded.Value;

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(qty) || string.IsNullOrEmpty(unit))
            {
                MessageBox.Show("Please fill all fields!");
                return;
            }

            // Generate a new unique numeric MaterialID
            string id = GenerateMaterialId();

            if (!decimal.TryParse(qty, out decimal quantity) || quantity <= 0)
            {
                MessageBox.Show("Quantity must be a positive number.");
                return;
            }

            try
            {
                string query =
                    "INSERT INTO RawMaterials (MaterialID, MaterialName, Quantity, Unit, DateAdded) " +
                    "VALUES (@id, @name, CAST(@qty AS decimal(18,0)), @unit, @date)";

                SqlParameter[] p =
                {
                new SqlParameter("@id", id),
                new SqlParameter("@name", name),
                new SqlParameter("@qty", quantity),
                new SqlParameter("@unit", unit),
                new SqlParameter("@date", date)
            };

                DbHelper.ExecuteNonQuery(query, p);
                MessageBox.Show("Material added successfully!");
                LoadMaterials();

                txtQty.Clear();
                txtUnit.Clear();
                cmbName.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding material: " + ex.Message);
            }
        }

        // ----------------------------
        // REMOVE MATERIAL FROM BOTH FACTORIES
        // ----------------------------
        // ----------------------------
        // REMOVE MATERIAL FROM BOTH FACTORIES
        // ----------------------------
        private void BtnRemove_Click(object? sender, EventArgs e)
        {
            string material = cmbName.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(material))
            {
                MessageBox.Show("Select a material to remove.");
                return;
            }

            if (!int.TryParse(txtQty.Text.Trim(), out int quantity) || quantity <= 0)
            {
                MessageBox.Show("Enter a valid quantity to remove.");
                txtQty.Focus();
                return;
            }

            DialogResult dr = MessageBox.Show(
                $"Are you sure you want to remove {quantity} {GetUnit(material)} of {material} from factory?",
                "Confirm Remove",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (dr != DialogResult.Yes)
                return;

            // Remove from factory
            bool removed = RawMaterialDb.RemoveQuantity(material, quantity, "Factory");

            if (removed)
            {
                MessageBox.Show("Raw material entry removed successfully!");
                txtQty.Clear();
                cmbName.SelectedIndex = -1;
            }
            else
            {
                MessageBox.Show("No material was removed. Please check if it exists in the factories.");
            }
        }


    }


}

