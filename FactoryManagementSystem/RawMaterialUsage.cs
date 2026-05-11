using FactoryManagementSystem;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Collections.Generic;

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

            dataGridView.SelectionChanged += (_, _) =>
            {
                if (dataGridView.CurrentRow is null ||
                    dataGridView.CurrentRow.IsNewRow ||
                    dataGridView.CurrentRow.Cells["Name"].Value is null)
                    return;

                string name = dataGridView.CurrentRow.Cells["Name"].Value.ToString()!;
                int idx = cmbMaterialName.FindStringExact(name);
                if (idx >= 0)
                    cmbMaterialName.SelectedIndex = idx;
            };
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
                "Crush" => "Ton",
                "Steel" => "Kg",
                "Mold Oil" => "Litre",
                _ => ""
            };
        }

        /// <summary>
        /// Build per-row deductions: either one grid row (by MaterialID), or spread by material name across all matching rows.
        /// </summary>
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
                    : dataGridView.CurrentRow;

            bool hasGridRow = gridRow is not null &&
                !gridRow.IsNewRow &&
                gridRow.Cells["MaterialID"].Value is object idVal &&
                int.TryParse(idVal.ToString(), out _) &&
                gridRow.Cells["Name"].Value != null &&
                decimal.TryParse(gridRow.Cells["Quantity"].Value?.ToString(), out _);

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

            if (string.IsNullOrEmpty(materialFromCombo))
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

            DateTime usageDate = dateMaterial.Value.Date;

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
                ClearFields();
                LoadRawMaterials();
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
                dashboard.LoadPage(new FactoryDash());
            }
        }
    }
}