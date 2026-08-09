using FactoryManagementCore;
using FactoryManagementSystem;
using Microsoft.Data.SqlClient;
using System.Data;

namespace FactoryDashBoard.Pages
{
    public partial class RawMaterialUsage : UserControl
    {
        private bool isLoadingMaterials = false;
        // Constructor
        public RawMaterialUsage()
        {
            InitializeComponent();

            btnClear.Click += BtnClear_Click;
            btnRemove.Click += BtnRemove_Click;
            btnBack.Click += btnBack_Click;

            LoadMaterialOptions();

            dateMaterial.MaxDate = DateTime.Today;

            LoadRawMaterials();

            // Prevent automatic selection when the control is loaded
            this.Load += RawMaterialUsage_Load;
        }
        // Event handler to prevent automatic selection when the control is loaded
        private void RawMaterialUsage_Load(object? sender, EventArgs e)
        {
            dataGridView.ClearSelection();
            dataGridView.CurrentCell = null;
        }
        // Load raw material records from database
        private void LoadRawMaterials()
        {
            try
            {
                isLoadingMaterials = true;

                string query = @"
            SELECT *
            FROM RawMaterial";

                DataTable dt = DBHelper.ExecuteDataTable(query, null);

                dataGridView.DataSource = dt;

                // Prevent the first row/cell from being selected
                dataGridView.ClearSelection();
                dataGridView.CurrentCell = null;

                // Reset combo box
                cmbMaterialName.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading materials: " + ex.Message);
            }
            finally
            {
                isLoadingMaterials = false;
            }
        }

        // Load material names into combo box
        private void LoadMaterialOptions()
        {
            cmbMaterialName.Items.Clear();

            cmbMaterialName.Items.Add("Select Raw Material...");

            cmbMaterialName.Items.AddRange(new object[]
            {
            "Cement",
            "Sand",
            "Crush",
            "Steel",
            "Mold Oil"
                });

            cmbMaterialName.SelectedIndex = 0;

            cmbMaterialName.DropDownStyle =
                ComboBoxStyle.DropDownList;
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
            cmbMaterialName.SelectedIndex = 0;
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
                "Crush" => "Ton",
                "Steel" => "Kg",
                "Mold Oil" => "Litre",
                _ => ""
            };
        }

        private bool TryBuildDeductions(out string displayName, out List<(int MaterialId, string MaterialName, decimal Amount)> deductions, out string? errorMessage)
        {
            displayName = "";
            deductions = new List<(int MaterialId, string MaterialName, decimal Amount)>();
            errorMessage = null;

            decimal? qtySpecified = null;
            string qtyText = txtQuantity.Text.Trim();
            if (!string.IsNullOrEmpty(qtyText))
            {
                if (!decimal.TryParse(qtyText, out decimal pq) || pq <= 0)
                {
                    errorMessage = "Enter a valid quantity to remove, or leave it blank when a grid row is selected to remove that whole line.";
                    return false;
                }
                qtySpecified = pq;
            }

            DataGridViewRow? gridRow =
                dataGridView.SelectedRows.Count > 0
                    ? dataGridView.SelectedRows[0]
                    : null;

            bool hasGridRow =
                gridRow != null &&
                gridRow.Cells["MaterialID"].Value != null &&
                gridRow.Cells["Name"].Value != null &&
                gridRow.Cells["Quantity"].Value != null;

            if (hasGridRow && gridRow is not null)
            {
                int materialId = Convert.ToInt32(gridRow.Cells["MaterialID"].Value);
                string name = gridRow.Cells["Name"].Value!.ToString()!;
                decimal rowQty = Convert.ToDecimal(gridRow.Cells["Quantity"].Value);
                displayName = name;

                decimal toRemove = qtySpecified ?? rowQty;
                if (toRemove <= 0)
                {
                    errorMessage = "Nothing to remove for this row.";
                    return false;
                }
                if (toRemove > rowQty)
                {
                    errorMessage =
                        $"Quantity ({toRemove}) is more than this stock line ({rowQty}). Either select a smaller amount or use the dropdown only — removal will spread across every line that uses this material.";
                    return false;
                }

                deductions.Add((materialId, name, toRemove));
                return true;
            }

            string materialFromCombo = cmbMaterialName.SelectedItem?.ToString() ?? "";

            if (string.IsNullOrEmpty(materialFromCombo) ||
    materialFromCombo == "Select Raw Material...")
            {
                errorMessage =
                    "Select material from the dropdown, or click a grid row — leave quantity blank to clear that row, or enter an amount.";
                return false;
            }

            if (!qtySpecified.HasValue)
            {
                errorMessage = "Pick a grid row and leave quantity blank to remove that whole line, or enter how much to take from stock.";
                return false;
            }

            displayName = materialFromCombo;

            DataTable lines = DBHelper.ExecuteDataTable(
                @"SELECT MaterialID, Name, Quantity 
                  FROM RawMaterial 
                  WHERE Name = @n 
                  ORDER BY MaterialID",
                new SqlParameter[] { new SqlParameter("@n", materialFromCombo) }
            );

            if (lines.Rows.Count == 0)
            {
                errorMessage = "Material not found in database.";
                return false;
            }

            decimal remaining = qtySpecified.Value;
            foreach (DataRow lr in lines.Rows)
            {
                if (remaining <= 0)
                    break;

                int mid = Convert.ToInt32(lr["MaterialID"]);
                string lrName = lr["Name"].ToString()!;
                decimal lineQty = Convert.ToDecimal(lr["Quantity"]);

                decimal take = Math.Min(lineQty, remaining);
                if (take > 0)
                {
                    deductions.Add((mid, lrName, take));
                    remaining -= take;
                }
            }

            if (remaining > 0)
            {
                decimal totalAvail = qtySpecified.Value - remaining;
                errorMessage =
                    $"Not enough quantity in stock ({totalAvail} available for {materialFromCombo}, you asked for {qtySpecified.Value}).";
                deductions.Clear();
                return false;
            }

            return true;
        }

        // Remove quantity of selected raw material (grid row and/or dropdown + quantity).
        private void BtnRemove_Click(object? sender, EventArgs e)
        {
            if (!TryBuildDeductions(out string material, out List<(int MaterialId, string MaterialName, decimal Amount)> deductions, out string? err))
            {
                MessageBox.Show(err ?? "Could not calculate removal.");
                if (err?.Contains("quantity", StringComparison.OrdinalIgnoreCase) == true)
                    txtQuantity.Focus();
                return;
            }

            decimal total = 0;
            foreach (var d in deductions)
                total += d.Amount;

            string unitLabel = GetUnitForMaterial(material);
            DialogResult dr = MessageBox.Show(
                deductions.Count > 1
                    ? $"Remove {total:N2} total {unitLabel} of {material} (across one or more stock lines)?"
                    : $"Remove {total:N2} {unitLabel} of {material}?",
                "Confirm Remove",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (dr != DialogResult.Yes)
                return;

            DateTime usageDate = dateMaterial.Value.Date.Add(DateTime.Now.TimeOfDay);

            const string upd =
                @"UPDATE RawMaterial SET Quantity = Quantity - @q 
                  WHERE MaterialID=@id AND Quantity>=@q";

            const string ins =
                @"INSERT INTO MaterialUsage (MaterialID, MaterialName, QuantityUsed, Date) 
                  VALUES (@id, @name, @q, @d)";

            try
            {
                using (SqlConnection con = new SqlConnection(DBHelper.ConnectionString))
                {
                    con.Open();
                    using (SqlTransaction tr = con.BeginTransaction())
                    {
                        try
                        {
                            foreach (var d in deductions)
                            {
                                using (SqlCommand cmdUp = new SqlCommand(upd, con, tr))
                                {
                                    cmdUp.Parameters.AddWithValue("@q", d.Amount);
                                    cmdUp.Parameters.AddWithValue("@id", d.MaterialId);
                                    if (cmdUp.ExecuteNonQuery() != 1)
                                        throw new InvalidOperationException("Stock changed or insufficient quantity for one of the rows.");
                                }

                                using (SqlCommand cmdIn = new SqlCommand(ins, con, tr))
                                {
                                    cmdIn.Parameters.AddWithValue("@id", d.MaterialId);
                                    cmdIn.Parameters.AddWithValue("@name", d.MaterialName);
                                    cmdIn.Parameters.AddWithValue("@q", d.Amount);
                                    cmdIn.Parameters.AddWithValue("@d", usageDate);
                                    cmdIn.ExecuteNonQuery();
                                }
                            }

                            tr.Commit();
                        }
                        catch
                        {
                            tr.Rollback();
                            throw;
                        }
                    }
                }

                MessageBox.Show("Raw material deduction saved and usage recorded.");
                Logger.AddLog(
                    Session.CurrentUser,
                    "REMOVE",
                    "Raw Material Usage",
                    $"Removed {total} {unitLabel} of {material} from raw material stock",
                    "Success"
                );
                ClearFields();
                LoadRawMaterials();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error removing: " + ex.Message);
            }
            LoadRawMaterials();
        }

        // Navigate back to dashboard home page
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