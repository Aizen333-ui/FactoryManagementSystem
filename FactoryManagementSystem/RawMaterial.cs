using Microsoft.Data.SqlClient;
using System.Data;
using FactoryManagementCore;

namespace FactoryManagementSystem
{
    public partial class RawMaterial : UserControl
    {
        // Constructor
        public RawMaterial()
        {
            InitializeComponent();

            // Attach button click events
            this.btnAdd.Click -= btnAdd_Click;
            this.btnAdd.Click += btnAdd_Click;

            this.btnRemove.Click -= BtnRemove_Click;
            this.btnRemove.Click += BtnRemove_Click;

            // Attach combo box selection event
            this.cmbName.SelectedIndexChanged -= cmbName_SelectedIndexChanged;
            this.cmbName.SelectedIndexChanged += cmbName_SelectedIndexChanged;

            // Load materials into DataGridView
            LoadMaterials();
        }

        // Load all raw materials from database
        private void LoadMaterials()
        {
            try
            {
                DataTable dt = DBHelper.ExecuteDataTable(
                    "SELECT * FROM RawMaterial ORDER BY MaterialID DESC",
                    null
                );

                // Bind data to grid
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading materials: " + ex.Message);
            }
        }

        // Return unit according to material name
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

        // Automatically set unit when material changes
        private void cmbName_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = cmbName.SelectedItem?.ToString() ?? cmbName.Text ?? "";

            txtUnit.Text = GetUnit(selected);
        }

        // Add new material into database
        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Validate empty fields
            if (string.IsNullOrWhiteSpace(cmbName.Text) ||
                string.IsNullOrWhiteSpace(txtQty.Text) ||
                string.IsNullOrWhiteSpace(txtUnit.Text) ||
                string.IsNullOrWhiteSpace(txtUnitPrice.Text))
            {
                MessageBox.Show("Please fill all fields!");
                return;
            }

            // Validate quantity
            if (!decimal.TryParse(txtQty.Text, out decimal quantity) || quantity <= 0)
            {
                MessageBox.Show("Quantity must be a positive number.");
                return;
            }

            // Validate unit price
            if (!decimal.TryParse(txtUnitPrice.Text, out decimal unitPrice) || unitPrice <= 0)
            {
                MessageBox.Show("Unit price must be a positive number.");
                return;
            }

            string name = cmbName.Text;
            string unit = txtUnit.Text;

            try
            {
                string query = @"
            INSERT INTO RawMaterial
            (Name, Quantity, Unit, UnitPrice)
            VALUES
            (@name, @qty, @unit, @unitprice)";

                SqlParameter[] p =
                {
            new SqlParameter("@name", name),
            new SqlParameter("@qty", quantity),
            new SqlParameter("@unit", unit),
            new SqlParameter("@unitprice", unitPrice)
        };

                DBHelper.ExecuteNonQuery(query, p);

                // Add payment record automatically
                decimal totalAmount = quantity * unitPrice;

                string paymentQuery = @"
            INSERT INTO Payments
            (Reason, Amount, Date)
            VALUES
            (@reason, @amount, @date)";

                SqlParameter[] paymentParams =
                {
            new SqlParameter("@reason", name),
            new SqlParameter("@amount", totalAmount),
            new SqlParameter("@date", DateTime.Now)
        };

                DBHelper.ExecuteNonQuery(paymentQuery, paymentParams);
                Logger.AddLog(
                    Session.CurrentUser,
                    "CREATE",
                    "Raw Material",
                    $"Added material '{name}' quantity {quantity} {unit} at unit price Rs {unitPrice}. Total expense Rs {totalAmount}",
                    "Success"
                );

                MessageBox.Show("Material added successfully!");

                LoadMaterials();

                // Clear fields
                cmbName.SelectedIndex = -1;
                txtQty.Clear();
                txtUnitPrice.Clear();
                txtUnit.Clear();
            }
            catch (Exception ex)
            {
                Logger.AddLog(
                    Session.CurrentUser,
                    "CREATE",
                    "Raw Material",
                    $"Failed to add material: {ex.Message}",
                    "Failed"
                );
                MessageBox.Show("Error adding material: " + ex.Message);
            }
        }

        // Remove selected material from database
        private void BtnRemove_Click(object sender, EventArgs e)
        {
            string name = cmbName.Text.Trim();
            string qtyText = txtQty.Text.Trim();

            // =========================
            // CASE 1: ROW SELECTED → DELETE BY ID
            // =========================
            if (dataGridView1.CurrentRow != null && string.IsNullOrEmpty(name))
            {
                int id = Convert.ToInt32(
                    dataGridView1.CurrentRow.Cells["MaterialID"].Value
                );

                DialogResult dr = MessageBox.Show(
                    "Delete selected material?",
                    "Confirm",
                    MessageBoxButtons.YesNo
                );

                if (dr != DialogResult.Yes)
                    return;

                DBHelper.ExecuteNonQuery(
                    "DELETE FROM RawMaterial WHERE MaterialID = @id",
                    new SqlParameter[]
                    {
                new SqlParameter("@id", id)
                    }
                );
                Logger.AddLog(
                    Session.CurrentUser,
                    "DELETE",
                    "Raw Material",
                    $"Deleted material record ID '{id}'",
                    "Success"
                );
                MessageBox.Show("Deleted successfully!");
                LoadMaterials();
                return;
            }

            // =========================
            // CASE 2: NAME + QUANTITY → REDUCE STOCK
            // =========================
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Enter material name.");
                return;
            }

            if (!decimal.TryParse(qtyText, out decimal removeQty) || removeQty <= 0)
            {
                MessageBox.Show("Enter valid quantity.");
                return;
            }

            // Get existing stock
            DataTable dt = DBHelper.ExecuteDataTable(
                "SELECT MaterialID, Quantity FROM RawMaterial WHERE Name = @name",
                new SqlParameter[]
                {
            new SqlParameter("@name", name)
                }
            );

            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Material not found.");
                return;
            }

            int id2 = Convert.ToInt32(dt.Rows[0]["MaterialID"]);
            decimal currentQty = Convert.ToDecimal(dt.Rows[0]["Quantity"]);

            // Check stock availability
            if (removeQty > currentQty)
            {
                MessageBox.Show("Not enough stock available.");
                return;
            }

            decimal newQty = currentQty - removeQty;

            DialogResult dr2 = MessageBox.Show(
                $"Reduce {removeQty} from {name}?",
                "Confirm",
                MessageBoxButtons.YesNo
            );

            if (dr2 != DialogResult.Yes)
                return;

            // =========================
            // UPDATE OR DELETE IF ZERO
            // =========================
            if (newQty == 0)
            {
                DBHelper.ExecuteNonQuery(
                    "DELETE FROM RawMaterial WHERE MaterialID = @id",
                    new SqlParameter[]
                    {
                new SqlParameter("@id", id2)
                    }
                );
            }
            else
            {
                DBHelper.ExecuteNonQuery(
                    "UPDATE RawMaterial SET Quantity = @qty WHERE MaterialID = @id",
                    new SqlParameter[]
                    {
                new SqlParameter("@qty", newQty),
                new SqlParameter("@id", id2)
                    }
                );
            }
            Logger.AddLog(
                Session.CurrentUser,
                "UPDATE",
                "Raw Material",
                $"Reduced {removeQty} from '{name}'. Remaining quantity: {newQty}",
                "Success"
            );
            MessageBox.Show("Stock updated successfully!");
            LoadMaterials();
        }

        // Navigate back to owner dashboard home page
        private void btnBack_Click(object sender, EventArgs e)
        {
            OwnerDashBoard dashboard =
                (OwnerDashBoard)this.FindForm();
            dashboard.ResetSidebarSelection(); 

            dashboard.LoadPage(new OwnerDash());
        }
    }
}