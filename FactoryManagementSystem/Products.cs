using System.Data;

namespace SalesDashboard.Pages
{
    public partial class Products : UserControl
    {
        public Products()
        {
            InitializeComponent();

            // Prevent automatic selection
            dgvProducts.MultiSelect = false;
            dgvProducts.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            // Attach button click events
            btnBack.Click += btnBack_Click;
            btnSearch.Click += btnSearch_Click;
            btnClear.Click += btnClear_Click;

            // Load product list when page opens
            LoadProducts();
            this.Load += Products_Load;

        }
        // Ensures no product is selected when the page loads
        private void Products_Load(object? sender, EventArgs e)
        {
            // Remove automatic first-row selection
            dgvProducts.ClearSelection();
            dgvProducts.CurrentCell = null;
        }
        // Clears search and filter values and reloads complete product list
        private void btnClear_Click(object? sender, EventArgs e)
        {
            txtSearch.Clear();
            cmbFilter.SelectedIndex = 0;
            LoadProducts(); // Reload all products
        }

        // Retrieves products from database based on search and stock filters
        private void LoadProducts()
        {
            try
            {
                // Base query to fetch product details from Production table
                string query = @"
                SELECT ProductName, Quantity, Date
                FROM Production
                WHERE 1=1";

                // Add product name search condition if user entered text
                if (!string.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    query += " AND ProductName LIKE @search";
                }

                // Apply stock availability filters
                if (cmbFilter.SelectedItem != null)
                {
                    string filter = cmbFilter.SelectedItem.ToString();


                    // Products with available quantity
                    if (filter == "In Stock")
                    {
                        query += " AND Quantity > 0";
                    }

                    // Products with quantity below defined threshold
                    else if (filter == "Low Stock")
                    {
                        query += " AND Quantity <= 5000 AND Quantity > 0";
                    }

                }

                // Sort products alphabetically
                query += " ORDER BY ProductName";

                // Create SQL parameters to prevent SQL injection
                var parameters = new List<Microsoft.Data.SqlClient.SqlParameter>();

                // Add search parameter only when search is used
                if (!string.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    parameters.Add(
                        new Microsoft.Data.SqlClient.SqlParameter(
                            "@search",
                            "%" + txtSearch.Text.Trim() + "%"
                        ));
                }

                // Execute query and retrieve product data
                DataTable dt =
                    FactoryManagementCore.DBHelper.ExecuteDataTable(
                        query,
                        parameters.ToArray()
                    );

                // Display results in DataGridView
                dgvProducts.DataSource = dt;
                dgvProducts.ClearSelection();
                dgvProducts.CurrentCell = null;
            }
            catch (Exception ex)
            {
                // Handle database or loading errors
                MessageBox.Show(
                    "Error loading products: " + ex.Message
                );

                // Clear grid if loading fails
                dgvProducts.DataSource = null;
            }
        }

        // Runs product search using current search/filter values
        private void btnSearch_Click(object? sender, EventArgs e)
        {
            LoadProducts();
        }

        // Returns user to Sales Dashboard main page
        private void btnBack_Click(object? sender, EventArgs e)
        {
            var dashboard = this.FindForm() as FactoryManagementSystem.SalesDashboard;


            if (dashboard != null)
            {
                // Reset selected sidebar item
                dashboard.ResetSidebarSelection();

                // Navigate back to sales dashboard home page
                dashboard.LoadPage(
                    new FactoryManagementSystem.SalesDash()
                );
            }
        }
    }
}