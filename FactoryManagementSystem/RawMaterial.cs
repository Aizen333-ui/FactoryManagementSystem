using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace FactoryManagementSystem
{
    public partial class RawMaterial : UserControl
    {
        public RawMaterial()
        {
            InitializeComponent();

            this.btnAdd.Click -= btnAdd_Click;
            this.btnAdd.Click += btnAdd_Click;

            this.btnRemove.Click -= BtnRemove_Click;
            this.btnRemove.Click += BtnRemove_Click;

            this.cmbName.SelectedIndexChanged -= cmbName_SelectedIndexChanged;
            this.cmbName.SelectedIndexChanged += cmbName_SelectedIndexChanged;

            LoadMaterials();
        }

        // LOAD MATERIALS
        private void LoadMaterials()
        {
            try
            {
                DataTable dt = DBHelper.ExecuteDataTable(
                    "SELECT * FROM RawMaterial ORDER BY MaterialID DESC",
                    null
                );

                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading materials: " + ex.Message);
            }
        }

        // UNIT SET
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
            string selected = cmbName.SelectedItem?.ToString() ?? cmbName.Text ?? "";
            txtUnit.Text = GetUnit(selected);
        }

        // CHECK ID EXISTS
        private bool MaterialIdExists(string id)
        {
            string query = "SELECT COUNT(*) FROM RawMaterial WHERE MaterialID = @id";

            object result = DBHelper.ExecuteScalar(query,
                new SqlParameter[] { new SqlParameter("@id", id) });

            return result != null && Convert.ToInt32(result) > 0;
        }

        // GENERATE ID (unused)
        private string GenerateMaterialId()
        {
            try
            {
                object res = DBHelper.ExecuteScalar(
                    "SELECT MAX(CAST(MaterialID AS bigint)) FROM RawMaterial",
                    null
                );

                long maxId = 0;
                if (res != null && long.TryParse(res.ToString(), out long parsed))
                    maxId = parsed;

                string newId = (maxId + 1).ToString();

                while (MaterialIdExists(newId))
                {
                    maxId++;
                    newId = (maxId + 1).ToString();
                }

                return newId;
            }
            catch
            {
                var rnd = new Random();
                string alt;

                do
                {
                    alt = rnd.Next(100000, 999999).ToString();
                }
                while (MaterialIdExists(alt));

                return alt;
            }
        }

        // ADD MATERIAL
        private void btnAdd_Click(object sender, EventArgs e)
        {
            string name = cmbName.Text.Trim();
            string qty = txtQty.Text.Trim();
            string unit = txtUnit.Text.Trim();

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(qty) || string.IsNullOrEmpty(unit))
            {
                MessageBox.Show("Please fill all fields!");
                return;
            }

            if (!decimal.TryParse(qty, out decimal quantity) || quantity <= 0)
            {
                MessageBox.Show("Quantity must be a positive number.");
                return;
            }

            try
            {
                // ✅ Unit store hoga ab
                string query =
                    "INSERT INTO RawMaterial (Name, Quantity, Unit) " +
                    "VALUES (@name, @qty, @unit)";

                SqlParameter[] p =
                {
                    new SqlParameter("@name", name),
                    new SqlParameter("@qty", quantity),
                    new SqlParameter("@unit", unit)
                };

                DBHelper.ExecuteNonQuery(query, p);

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
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

            cmbName.SelectedItem = row.Cells["Name"].Value.ToString();
            txtQty.Text = row.Cells["Quantity"].Value.ToString();
        }

        // REMOVE MATERIAL
        private void BtnRemove_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Select a row first.");
                return;
            }

            string material = dataGridView1.CurrentRow.Cells["Name"].Value.ToString();

            int quantity = Convert.ToInt32(
                dataGridView1.CurrentRow.Cells["Quantity"].Value
            );

            DialogResult dr = MessageBox.Show(
                $"Remove ALL of {material}?",
                "Confirm",
                MessageBoxButtons.YesNo
            );

            if (dr != DialogResult.Yes)
                return;

            string query = "DELETE FROM RawMaterial WHERE Name = @name";

            SqlParameter[] p =
            {
        new SqlParameter("@name", material)
    };

            DBHelper.ExecuteNonQuery(query, p);

            MessageBox.Show("Removed successfully");
            LoadMaterials();
        }
        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadMaterials();
        }
    }
}