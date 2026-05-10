namespace FactoryManagementSystem
{
    public partial class OwnerDashBoard : Form
    {
        public OwnerDashBoard()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.WindowState = FormWindowState.Maximized;
            LoadPage(new OwnerDash());

        }

        // ===================== DYNAMIC PAGE LOADER =====================
        // Loads different UserControls into the main dashboard area (card panel)
        public void LoadPage(UserControl page)
        {
            // If card panel is not initialized, fallback to main panel
            if (card == null)
            {

                panelMain.Controls.Clear();
                page.Dock = DockStyle.Fill;
                panelMain.Controls.Add(page);
                page.BringToFront();
                return;
            }

            card.Controls.Clear();

            page.Dock = DockStyle.Fill;
            page.Margin = new Padding(0);
            page.Padding = new Padding(24);
            card.Controls.Add(page);
            page.BringToFront();
        }

        // --- MENU BUTTON CLICK EVENTS ---
        // ===================== RAW MATERIAL PAGE =====================

        private void btnRawMaterial_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnRawMaterial);
            LoadPage(new RawMaterial());
        }
        // ===================== PAYMENTS PAGE =====================

        private void btnPayments_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnPayments);
            LoadPage(new Payments());
        }
        // ===================== WORKERS MANAGEMENT PAGE =====================

        private void btnManageWorkers_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnManageWorkers);
            LoadPage(new WorkersAddandView());
        }
        // ===================== REPORTS PAGE =====================

        private void btnReports_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnReports);
            LoadPage(new OwnerReportsPage());
        }
        // ===================== LOGOUT FUNCTION =====================

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
