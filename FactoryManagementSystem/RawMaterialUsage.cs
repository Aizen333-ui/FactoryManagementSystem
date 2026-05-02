using FactoryManagementSystem;
using System;
using System.Linq;
using System.Windows.Forms;


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

            bool exists = GlobalStorage.RawMaterials.Any(r =>
                r.MaterialName == material &&
                r.Quantity == quantity &&
                r.Date.Date == selectedDate);

            if (exists)
            {
                MessageBox.Show("Entry already exists for this material, quantity and date");
                return;
            }

            GlobalStorage.RawMaterials.Add(new RawMaterialEntry
            {
                MaterialName = material,
                Quantity = quantity,
                Unit = GetUnitForMaterial(material),
                Date = selectedDate
            });

            MessageBox.Show("Raw Material Saved!");

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
                $"Remove {quantity} {GetUnitForMaterial(material)} of {material} from Factory 1 ?",
                "Confirm Remove",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (dr != DialogResult.Yes)
                return;

            bool removedFactory1 = RawMaterialDb.RemoveQuantity(material, quantity, "Factory1");

            if (removedFactory1)
            {
                var entry = GlobalStorage.RawMaterials
                    .FirstOrDefault(r => r.MaterialName == material);

                if (entry != null)
                    GlobalStorage.RawMaterials.Remove(entry);

                MessageBox.Show("Raw material entry removed successfully!");

            }
            else
            {
                MessageBox.Show("Material not found in Factory.");
            }
        }

    }


}
