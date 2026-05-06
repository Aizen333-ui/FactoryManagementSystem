using FactoryDashboard;
using System;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using System.Data;

namespace FactoryDashBoard.Pages
{
    public partial class RecordProduction : UserControl
    {
        public RecordProduction()
        {
            InitializeComponent();

            btnSave.Click += BtnSave_Click;
            btnClear.Click += BtnClear_Click;
            txtQuantity.KeyPress += txtQuantity_KeyPress;

            LoadProductOptions();
            LoadUnitOptions();

            dateProduction.MaxDate = DateTime.Today;

            // ✅ LOAD DATA ON START
            LoadProduction();
        }

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

        private void LoadUnitOptions()
        {
            cmbUnit.Items.AddRange(new object[]
            {
                "Pieces",
                "Sqft"
            });
            cmbUnit.SelectedIndex = 0;
        }

        // ✅ LOAD DATA INTO GRID
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

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            if (cmbProductName.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a product");
                cmbProductName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtQuantity.Text))
            {
                MessageBox.Show("Please enter quantity");
                txtQuantity.Focus();
                return;
            }

            if (!double.TryParse(txtQuantity.Text, out double quantity))
            {
                MessageBox.Show("Quantity must be numeric");
                txtQuantity.Focus();
                return;
            }

            if (quantity <= 0 || quantity > 10000)
            {
                MessageBox.Show("Quantity must be between 1 and 10000");
                txtQuantity.Focus();
                return;
            }

            DateTime selectedDate = dateProduction.Value.Date;
            if (selectedDate > DateTime.Today)
            {
                MessageBox.Show("Future date not allowed");
                return;
            }

            if (ProductionExists(cmbProductName.Text, selectedDate))
            {
                MessageBox.Show("Production for this product already recorded for selected date.");
                return;
            }

            try
            {
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

                // ✅ REFRESH GRID
                LoadProduction();

                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving production: " + ex.Message);
            }
        }

        private void BtnClear_Click(object? sender, EventArgs e)
        {
            ClearFields();
        }

        private void ClearFields()
        {
            cmbProductName.SelectedIndex = -1;
            txtQuantity.Clear();
            cmbUnit.SelectedIndex = 0;
            dateProduction.Value = DateTime.Today;
        }

        private void txtQuantity_KeyPress(object? sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) &&
                !char.IsDigit(e.KeyChar) &&
                e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }
        
    }
}