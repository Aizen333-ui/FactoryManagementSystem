using System;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
namespace FactoryManagementSystem
{
    public partial class OwnerDashBoard : Form
    {
        public OwnerDashBoard()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.WindowState = FormWindowState.Maximized;
        }

        // --- PAGE LOAD FUNCTION ---
        private void LoadPage(UserControl page)
        {
            // load into the inner white card so padding and rounded card visuals apply
            if (card == null)
            {
                // fallback to panelMain if card isn't initialized yet
                panelMain.Controls.Clear();
                page.Dock = DockStyle.Fill;
                panelMain.Controls.Add(page);
                page.BringToFront();
                return;
            }

            card.Controls.Clear();
            // ensure page respects inner spacing of the card
            page.Dock = DockStyle.Fill;
            page.Margin = new Padding(0);
            page.Padding = new Padding(24);
            card.Controls.Add(page);
            page.BringToFront();
        }

        // --- MENU CLICK EVENTS ---

        private void btnRawMaterial_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnRawMaterial);
            LoadPage(new RawMaterial());
        }

        private void btnPayments_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnPayments);
            LoadPage(new Payments());
        }

        private void btnManageWorkers_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnManageWorkers);
            LoadPage(new WorkersAddandView());
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnReports);
            LoadPage(new OwnerReportsPage());
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("You want to Logout?", "Confirm",
               MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Close();
            }
        }
    }
}
