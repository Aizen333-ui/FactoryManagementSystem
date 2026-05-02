using FactoryDashBoard.Pages;
using System;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using FactoryDashboard.Pages;

namespace FactoryManagementSystem
{
    public partial class FactoryDashBoard : Form
    {
        public FactoryDashBoard()
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

        private void btnRecord_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnRecord);
            LoadPage(new RecordProduction());
        }

        private void btnRaw_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnRaw);
            LoadPage(new RawMaterialUsage());
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnReport);
            LoadPage(new Report());
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
