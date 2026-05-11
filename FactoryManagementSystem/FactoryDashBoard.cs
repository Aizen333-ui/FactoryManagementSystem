using FactoryDashBoard.Pages;

namespace FactoryManagementSystem
{
    public partial class FactoryDashBoard : Form
    {
        public FactoryDashBoard()
        {
            InitializeComponent();
            // Make form resizable and start in full-screen mode

            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.WindowState = FormWindowState.Maximized;
            LoadPage(new FactoryDash());
        }

        // --- LOAD USER CONTROL INTO MAIN UI AREA ---
        // This function dynamically loads different pages (UserControls)
        // inside the dashboard without opening new forms
        public void LoadPage(UserControl page)
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
        public void ResetSidebarSelection()
        {
            foreach (Control c in panelSideMenu.Controls)
            {
                if (c is Button btn && btn.Tag?.ToString() == "nav")
                {
                    btn.ForeColor = Color.White;
                    btn.BackColor = Color.Transparent;
                }
            }
        }
        // --- NAVIGATION BUTTON: RECORD PRODUCTION PAGE ---

        private void btnRecord_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnRecord);
            LoadPage(new RecordProduction());
        }
        // --- NAVIGATION BUTTON: RAW MATERIAL PAGE ---

        private void btnRaw_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnRaw);
            LoadPage(new RawMaterialUsage());
        }
        // --- NAVIGATION BUTTON: REPORT PAGE ---

        private void btnReport_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnReport);
            LoadPage(new Report());
        }
        // --- LOGOUT BUTTON ---

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
