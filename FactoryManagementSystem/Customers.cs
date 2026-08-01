using System;
using System.Data;
using FactoryManagementCore;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace SalesDashboard.Pages
{
    public partial class Customers : UserControl
    {

        // ============================================================
        // Stores currently selected customer ID.
        //
        // Nullable because no customer is selected when page loads.
        // ============================================================

        private int? selectedCustomerId = null;

        // ============================================================
        // Prevents selection events from executing while DataGridView
        // is being refreshed or populated.
        // ============================================================

        private bool isLoadingCustomers = false;

        // ============================================================
        // Constructor
        //
        // Initializes:
        // - UI components
        // - Event handlers
        // - Customer data loading
        // ============================================================

        public Customers()
        {
            InitializeComponent();

            // Connect UI events with their handlers
            btnAddCustomer.Click += btnAdd_Click;

            btnEditCustomer.Click += btnEdit_Click;

            btnBack.Click += btnBack_Click;

            txtSearchCustomer.TextChanged +=
                txtSearchCustomer_TextChanged;

            dgvCustomers.SelectionChanged +=
                dgvCustomers_SelectionChanged;

            // Load customer records
            LoadCustomers();

            // Execute initial page setup after loading
            this.Load += Customers_Load;
        }

        // ============================================================
        // Runs when Customer page is displayed.
        //
        // Clears automatic DataGridView selection and resets input
        // fields to default state.
        // ============================================================

        private void Customers_Load(
            object? sender,
            EventArgs e)
        {
            dgvCustomers.ClearSelection();
            dgvCustomers.CurrentCell = null;
            selectedCustomerId = null;
            ClearInput();
        }

        // ============================================================
        // Loads all customers from database.
        //
        // Retrieves:
        // - Customer ID
        // - Customer Name
        // - Phone
        // - Address
        //
        // Displays records inside Customers DataGridView.
        // ============================================================

        private void LoadCustomers()
        {
            try
            {
                // Prevent selection event while loading data
                isLoadingCustomers = true;

                string query =
                    @"
                    SELECT 
                        CustomerID,
                        CustomerName,
                        Phone,
                        Address
                    FROM Customers
                    ORDER BY CustomerID DESC";

                DataTable dt =
                    DBHelper.ExecuteDataTable(
                        query,
                        null);

                // Refresh DataGridView columns
                dgvCustomers.Columns.Clear();

                dgvCustomers.AutoGenerateColumns = true;

                // Bind database results
                dgvCustomers.DataSource =
                    dt;

                // Remove default first-row selection
                dgvCustomers.ClearSelection();

                dgvCustomers.CurrentCell = null;

                // Reset selected customer
                selectedCustomerId = null;

                // Clear input fields
                ClearInput();

            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error loading customers: "
                    + ex.Message,

                    "Customers",

                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                dgvCustomers.DataSource = null;
            }
            finally
            {
                // Allow selection events again
                isLoadingCustomers = false;
            }
        }

        // ============================================================
        // Searches customers based on entered text.
        //
        // Searches in:
        // - Customer Name
        // - Phone Number
        // - Address
        //
        // Triggered automatically when search textbox changes.
        // ============================================================

        private void txtSearchCustomer_TextChanged(
            object? sender,
            EventArgs e)
        {
            string searchText =
                txtSearchCustomer.Text.Trim();

            // Reload all customers if search is empty
            if (string.IsNullOrEmpty(searchText))
            {
                LoadCustomers();
                return;
            }

            try
            {
                string query =
                @"
                SELECT 
                    CustomerID,
                    CustomerName,
                    Phone,
                    Address
                FROM Customers
                WHERE CustomerName LIKE @search
                   OR Phone LIKE @search
                   OR Address LIKE @search
                ORDER BY CustomerID DESC";

                // Parameterized search prevents SQL injection
                SqlParameter parameter =
                    new SqlParameter(
                        "@search",
                        SqlDbType.NVarChar)
                    {
                        Value =
                            "%" + searchText + "%"
                    };

                DataTable dt =
                    DBHelper.ExecuteDataTable(
                        query,
                        new[] { parameter });

                // Refresh grid with search results
                dgvCustomers.Columns.Clear();

                dgvCustomers.AutoGenerateColumns =
                    true;

                dgvCustomers.DataSource =
                    dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Search error: "
                    + ex.Message,

                    "Customers",

                    MessageBoxButtons.OK,

                    MessageBoxIcon.Error);
            }
        }

        // ============================================================
        // Adds a new customer record.
        //
        // Validation:
        // - Customer name is required
        //
        // Database:
        // Inserts customer information into Customers table.
        //
        // Audit:
        // Creates log entry after successful operation.
        // ============================================================

        private void btnAdd_Click(
            object? sender,
            EventArgs e)
        {
            string name =
                txtCustomerName.Text.Trim();

            string phone =
                txtPhone.Text.Trim();

            string address =
                txtAddress.Text.Trim();

            // Validate required field
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show(
                    "Please enter customer name.",

                    "Validation",

                    MessageBoxButtons.OK,

                    MessageBoxIcon.Warning);

                return;
            }

            try
            {
                string query =
                @"
                INSERT INTO Customers
                (
                    CustomerName,
                    Phone,
                    Address
                )
                VALUES
                (
                    @name,
                    @phone,
                    @address
                )";

                SqlParameter[] parameters =
                {
                    new SqlParameter(
                        "@name",
                        SqlDbType.NVarChar)
                    {
                        Value = name
                    },


                    new SqlParameter(
                        "@phone",
                        SqlDbType.NVarChar)
                    {
                        Value = phone
                    },


                    new SqlParameter(
                        "@address",
                        SqlDbType.NVarChar)
                    {
                        Value = address
                    }
                };

                int result =
                    DBHelper.ExecuteNonQuery(
                        query,
                        parameters);

                if (result > 0)
                {
                    MessageBox.Show(
                        "Customer added successfully.",

                        "Customers",

                        MessageBoxButtons.OK,

                        MessageBoxIcon.Information);

                    ClearInput();

                    // Create successful audit record
                    try
                    {
                        Logger.AddLog(
                            Session.CurrentUser ?? "system",

                            "Customer Added",

                            "Customers",

                            $"Added customer '{name}'",

                            "Success");
                    }
                    catch
                    {
                        // Ignore logging failure
                        // so main operation continues
                    }

                    LoadCustomers();
                }
                else
                {
                    MessageBox.Show(
                        "No rows affected. Customer was not added.",

                        "Customers",

                        MessageBoxButtons.OK,

                        MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                // Record failed operation
                try
                {
                    Logger.AddLog(
                        Session.CurrentUser ?? "system",

                        "Customer Added",

                        "Customers",

                        ex.Message,

                        "Failed");
                }
                catch
                {
                    // Ignore logger errors
                }

                MessageBox.Show(
                    "Error adding customer: "
                    + ex.Message,

                    "Customers",

                    MessageBoxButtons.OK,

                    MessageBoxIcon.Error);
            }
        }

        // ============================================================
        // Updates selected customer information.
        //
        // Requirements:
        // - A customer must be selected first.
        // - Customer name is required.
        //
        // Updates:
        // - Customer Name
        // - Phone
        // - Address
        //
        // Creates an audit log after successful update.
        // ============================================================

        private void btnEdit_Click(
            object? sender,
            EventArgs e)
        {
            // Check if customer is selected
            if (selectedCustomerId == null)
            {
                MessageBox.Show(
                    "Please select a customer to edit.",

                    "Customers",

                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            string name =
                txtCustomerName.Text.Trim();

            string phone =
                txtPhone.Text.Trim();

            string address =
                txtAddress.Text.Trim();

            // Validate customer name
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show(
                    "Please enter customer name.",

                    "Validation",

                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            try
            {
                string query =
                @"
                UPDATE Customers
                SET
                    CustomerName = @name,
                    Phone = @phone,
                    Address = @address
                WHERE CustomerID = @id";

                SqlParameter[] parameters =
                {
                    new SqlParameter(
                        "@name",
                        SqlDbType.NVarChar)
                    {
                        Value = name
                    },

                    new SqlParameter(
                        "@phone",
                        SqlDbType.NVarChar)
                    {
                        Value = phone
                    },

                    new SqlParameter(
                        "@address",
                        SqlDbType.NVarChar)
                    {
                        Value = address
                    },

                    new SqlParameter(
                        "@id",
                        SqlDbType.Int)
                    {
                        Value =
                            selectedCustomerId.Value
                    }
                };

                int result =
                    DBHelper.ExecuteNonQuery(
                        query,
                        parameters);

                if (result > 0)
                {
                    MessageBox.Show(
                        "Customer updated successfully.",

                        "Customers",

                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    ClearInput();

                    // Save update activity
                    try
                    {
                        Logger.AddLog(
                            Session.CurrentUser ?? "system",

                            "Customer Updated",

                            "Customers",

                            $"Updated customer '{name}' " +
                            $"(ID={selectedCustomerId})",

                            "Success");
                    }
                    catch
                    {
                        // Ignore logging failures
                    }

                    LoadCustomers();
                }
                else
                {
                    MessageBox.Show(
                        "No rows affected. Customer was not updated.",

                        "Customers",

                        MessageBoxButtons.OK,

                        MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                try
                {
                    Logger.AddLog(
                        Session.CurrentUser ?? "system",

                        "Customer Updated",

                        "Customers",

                        ex.Message,

                        "Failed");
                }
                catch
                {
                    // Ignore logger failures
                }

                MessageBox.Show(
                    "Error updating customer: "
                    + ex.Message,

                    "Customers",

                    MessageBoxButtons.OK,

                    MessageBoxIcon.Error);
            }
        }

        // ============================================================
        // Handles DataGridView customer selection.
        //
        // When user selects a row:
        // - Stores CustomerID
        // - Loads customer details into input fields
        //
        // Used for editing existing customers.
        // ============================================================

        private void dgvCustomers_SelectionChanged(
            object? sender,
            EventArgs e)
        {
            // Ignore selection while grid is loading
            if (isLoadingCustomers)
                return;

            // Ignore when no row is selected
            if (dgvCustomers.SelectedRows.Count == 0)
                return;

            try
            {
                DataGridViewRow row =
                    dgvCustomers.SelectedRows[0];

                // Retrieve data from bound DataRow
                if (row.DataBoundItem is DataRowView drv)
                {
                    selectedCustomerId =
                        drv.Row.Field<int>(
                            "CustomerID");

                    txtCustomerName.Text =
                        drv.Row.Field<string>(
                            "CustomerName")
                        ?? "";

                    txtPhone.Text =
                        drv.Row.Field<string>(
                            "Phone")
                        ?? "";

                    txtAddress.Text =
                        drv.Row.Field<string>(
                            "Address")
                        ?? "";
                }
            }
            catch
            {
                // Ignore selection parsing errors
            }
        }

        // ============================================================
        // Clears customer input fields.
        //
        // Used after:
        // - Adding customer
        // - Updating customer
        // - Reloading customer list
        // ============================================================

        private void ClearInput()
        {
            txtCustomerName.Text =
                string.Empty;

            txtPhone.Text =
                string.Empty;

            txtAddress.Text =
                string.Empty;

            selectedCustomerId =
                null;
        }

        // ============================================================
        // Returns user back to Sales Dashboard.
        //
        // Actions:
        // - Clears sidebar selection
        // - Loads Sales Dashboard page
        // ============================================================

        private void btnBack_Click(
            object? sender,
            EventArgs e)
        {
            var dashboard =
                this.FindForm()
                as FactoryManagementSystem.SalesDashboard;

            if (dashboard != null)
            {
                dashboard.ResetSidebarSelection();
                dashboard.LoadPage(
                    new FactoryManagementSystem.SalesDash());
            }
        }
    }
}