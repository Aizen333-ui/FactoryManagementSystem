using SalesDashboard.Pages;
using FactoryManagementCore;

namespace FactoryManagementSystem
{
    public partial class SalesDashboard : Form
    {

        public SalesDashboard()
        {
            InitializeComponent();

            // Sales dashboard opens as a full-screen application
            this.FormBorderStyle =
                FormBorderStyle.Sizable;

            this.WindowState =
                FormWindowState.Maximized;

            // Load default dashboard page
            LoadPage(new SalesDash());
        }

        // ==================================================
        // LOAD USER CONTROL PAGE
        // ==================================================
        // Handles switching between dashboard modules.
        //
        // Pages loaded:
        // - Dashboard
        // - New Sale
        // - Products
        // - Customers
        // - Sales History
        // - Returns
        //
        // Uses card panel if available, otherwise falls back
        // to main panel.
        // ==================================================

        public void LoadPage(UserControl page)
        {

            // Fallback loading when card container
            // is not available
            if (card == null)
            {
                panelMain.Controls.Clear();

                page.Dock =
                    DockStyle.Fill;

                panelMain.Controls.Add(page);

                page.BringToFront();

                return;
            }

            // Remove previous page
            card.Controls.Clear();

            // Configure page layout
            page.Dock =
                DockStyle.Fill;

            page.Margin =
                new Padding(0);

            page.Padding =
                new Padding(24);

            // Display selected module
            card.Controls.Add(page);

            page.BringToFront();
        }

        // ==================================================
        // RESET SIDEBAR BUTTONS
        // ==================================================
        // Removes active highlighting from navigation buttons.
        //
        // Called when returning back to dashboard or
        // switching pages programmatically.
        // ==================================================

        public void ResetSidebarSelection()
        {
            foreach (Control c in panelSideMenu.Controls)
            {

                if (c is Button btn &&
                    btn.Tag?.ToString() == "nav")
                {

                    btn.ForeColor =
                        Color.White;

                    btn.BackColor =
                        Color.Transparent;
                }
            }

            activeButton = null;
        }

        // ==================================================
        // CREATE NEW SALE PAGE
        // ==================================================

        private void btnNewSale_Click(
            object sender,
            EventArgs e)
        {
            SetActiveButton(btnNewSale);

            LoadPage(
                new NewSale());
        }

        // ==================================================
        // PRODUCTS PAGE
        // ==================================================

        private void btnProducts_Click(
            object sender,
            EventArgs e)
        {
            SetActiveButton(btnProducts);

            LoadPage(
                new Products());
        }

        // ==================================================
        // CUSTOMERS PAGE
        // ==================================================

        private void btnCustomers_Click(
            object sender,
            EventArgs e)
        {
            SetActiveButton(btnCustomers);

            LoadPage(
                new Customers());
        }

        // ==================================================
        // SALES HISTORY PAGE
        // ==================================================
        private void btnSalesHistory_Click(
            object sender,
            EventArgs e)
        {
            SetActiveButton(btnSalesHistory);

            LoadPage(
                new SalesHistory());
        }

        // ==================================================
        // RETURNS PAGE
        // ==================================================
        private void btnReturns_Click(
            object sender,
            EventArgs e)
        {
            SetActiveButton(btnReturns);

            LoadPage(
                new Returns());
        }

        // ==================================================
        // LOGOUT
        // ==================================================
        // Confirms logout, creates audit log entry,
        // and closes sales dashboard.
        // ==================================================
        private void btnLogout_Click(
            object sender,
            EventArgs e)
        {

            DialogResult result =
                MessageBox.Show(
                    "You want to Logout?",
                    "Confirm",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {

                // Save logout activity
                Logger.AddLog(
                    Session.CurrentUser ?? "Unknown",
                    "LOGOUT",
                    "Security",
                    "Sales user logged out from the system",
                    "Success"
                );

                // Close dashboard
                this.Close();
            }
        }
    }
}