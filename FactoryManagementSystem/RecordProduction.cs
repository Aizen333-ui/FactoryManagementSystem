using FactoryDashboard;
using System;
using System.Linq;
using System.Windows.Forms;

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

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            // ✅ Product
            if (cmbProductName.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a product");
                cmbProductName.Focus();
                return;
            }

            // ✅ Quantity
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

            // ✅ Date
            DateTime selectedDate = dateProduction.Value.Date;
            if (selectedDate > DateTime.Today)
            {
                MessageBox.Show("Future date not allowed");
                return;
            }

            // ✅ Duplicate Check
            bool exists = GlobalStorage.Productions.Any(p =>
                p.ProductName == cmbProductName.Text &&
                p.Date.Date == selectedDate);

            if (exists)
            {
                MessageBox.Show("Production for this product already recorded for selected date.");
                return;
            }

            // ✅ Save
            GlobalStorage.Productions.Add(new ProductionEntry
            {
                ProductName = cmbProductName.Text,
                Quantity = quantity,
                Unit = cmbUnit.SelectedItem?.ToString() ?? "",

                Date = selectedDate
            });

            MessageBox.Show("Production record saved successfully!");

            ClearFields();
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
