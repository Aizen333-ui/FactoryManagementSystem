using System.Data;
using FactoryManagementCore;
using Microsoft.Data.SqlClient;

namespace SalesDashboard.Pages
{
    public partial class NewSale : UserControl
    {
        // Stores completed sale ID for invoice printing
        private int lastSaleID = 0;


        // ============================================================
        // Constructor
        //
        // Initializes:
        // - UI events
        // - Customers
        // - Products
        // - Completed sales
        // ============================================================

        public NewSale()
        {
            InitializeComponent();

            btnAddToCart.Click += btnAddToCart_Click;
            btnRemove.Click += btnRemove_Click;
            btnRefresh.Click += btnRefresh_Click;

            btnCompleteSale.Click += btnCompleteSale_Click;
            btnPrintInvoice.Click += btnPrintInvoice_Click;
            btnBack.Click += btnBack_Click;


            txtDiscount.TextChanged +=
                (s, e) => CalculateCartTotal();

            txtTax.TextChanged +=
                (s, e) => CalculateCartTotal();

            dgvCart.SelectionMode =
            DataGridViewSelectionMode.CellSelect;

            dgvCart.MultiSelect = false;
            LoadInitialData();

        }

        // ============================================================
        // Loads all initial page data
        //
        // Loads:
        // - Customers
        // - Products
        // - Payment options
        // - Completed sales
        // ============================================================

        private void LoadInitialData()
        {
            LoadCustomers();

            LoadAvailableProducts();

            LoadPaymentOptions();

            LoadCompletedSales();

            CalculateCartTotal();
        }



        // ============================================================
        // Loads customers into ComboBox
        // ============================================================

        private void LoadCustomers()
        {
            try
            {
                DataTable dt =
                    DBHelper.ExecuteDataTable(
                    @"SELECT CustomerID, CustomerName
                      FROM Customers
                      ORDER BY CustomerName",
                    null);


                cmbCustomer.Items.Clear();

                cmbCustomer.Items.Add(
                    "Select customer...");


                foreach (DataRow row in dt.Rows)
                {
                    cmbCustomer.Items.Add(
                        new CustomerItem
                        {
                            ID =
                            Convert.ToInt32(
                                row["CustomerID"]),

                            Name =
                            row["CustomerName"]
                            .ToString()
                        });
                }


                cmbCustomer.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error loading customers: "
                    + ex.Message);
            }
        }



        // ============================================================
        // Loads available products
        //
        // Displays:
        // - Product name
        // - Available quantity
        // ============================================================

        private void LoadAvailableProducts()
        {
            try
            {
                DataTable dt =
                    DBHelper.ExecuteDataTable(
                    @"SELECT ProductName,
                      SUM(Quantity) AS Quantity
                      FROM Production
                      WHERE Quantity > 0
                      GROUP BY ProductName
                      ORDER BY ProductName",
                    null);


                cmbProduct.Items.Clear();

                cmbProduct.Items.Add(
                    "Select product...");


                lstAvailableProducts.Items.Clear();


                foreach (DataRow row in dt.Rows)
                {
                    string name =
                        row["ProductName"]
                        .ToString();


                    cmbProduct.Items.Add(name);


                    ListViewItem item =
                        new ListViewItem(name);


                    item.SubItems.Add(
                        row["Quantity"]
                        .ToString());


                    lstAvailableProducts.Items.Add(item);
                }


                cmbProduct.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error loading products: "
                    + ex.Message);
            }
        }



        // ============================================================
        // Payment dropdown values
        // ============================================================

        private void LoadPaymentOptions()
        {
            cmbPaymentMethod.Items.Clear();

            cmbPaymentMethod.Items.Add(
                "Select payment method...");

            cmbPaymentMethod.Items.AddRange(
                new object[]
                {
                    "Cash",
                    "Card",
                    "Bank Transfer",
                    "Credit"
                });


            cmbPaymentMethod.SelectedIndex = 0;



            cmbPaymentStatus.Items.Clear();

            cmbPaymentStatus.Items.Add(
                "Select payment status...");

            cmbPaymentStatus.Items.AddRange(
                new object[]
                {
                    "Paid",
                    "Pending",
                    "Partial"
                });


            cmbPaymentStatus.SelectedIndex = 0;
        }
        // ============================================================
        // Stores customer ID with displayed name
        //
        // Allows ComboBox to show name while keeping database ID.
        // ============================================================

        private class CustomerItem
        {
            public int ID { get; set; }

            public string Name { get; set; }


            public override string ToString()
            {
                return Name;
            }
        }



        // ============================================================
        // Add product to cart
        //
        // Checks:
        // - Product selected
        // - Quantity valid
        // - Stock available
        // ============================================================

        private void btnAddToCart_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                if(cmbCustomer.SelectedIndex <=0)
                {
                    MessageBox.Show(
                        "Select customer.");
                    return;
                }

                if (cmbProduct.SelectedIndex <= 0)
                {
                    MessageBox.Show(
                        "Select product.");

                    return;
                }


                if (!int.TryParse(
                    txtQuantity.Text,
                    out int quantity))
                {
                    MessageBox.Show(
                        "Enter valid quantity.");

                    return;
                }


                if (!decimal.TryParse(
                    txtUnitPrice.Text,
                    out decimal price))
                {
                    MessageBox.Show(
                        "Enter valid price.");

                    return;
                }



                string product =
                    cmbProduct.SelectedItem
                    .ToString();



                // Check available stock

                object result =
                    DBHelper.ExecuteScalar(
                    @"SELECT SUM(Quantity)
              FROM Production
              WHERE ProductName=@name",
                    new SqlParameter[]
                    {
                new SqlParameter(
                    "@name",
                    product)
                    });



                int available =
                    Convert.ToInt32(result);



                if (quantity > available)
                {
                    MessageBox.Show(
                        "Not enough stock available.");

                    return;
                }



                decimal total =
                    quantity * price;



                DataTable dt =
                    DBHelper.ExecuteDataTable(
                    @"SELECT TOP 1 ProductionID
              FROM Production
              WHERE ProductName=@name",
                    new SqlParameter[]
                    {
                new SqlParameter(
                    "@name",
                    product)
                    });



                int productionID =
                    Convert.ToInt32(
                        dt.Rows[0]["ProductionID"]);



                dgvCart.Rows.Add(
                    productionID,
                    product,
                    quantity,
                    price,
                    total);



                // Select only the ProductName cell of the newly added row

                int newRowIndex = dgvCart.Rows.Count - 1;

                dgvCart.ClearSelection();

                dgvCart.CurrentCell =
                dgvCart.Rows[newRowIndex]
                .Cells["ProductName"];

                dgvCart.Rows[newRowIndex]
                    .Cells["ProductName"]
                    .Selected = true;



                CalculateCartTotal();


                txtQuantity.Clear();

                txtUnitPrice.Clear();


                cmbProduct.SelectedIndex = 0;


            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error adding product: "
                    + ex.Message);
            }
        }



        // ============================================================
        // Remove product from cart
        //
        // Behaviour:
        //   - No qty typed → remove the entire selected row
        //   - Qty typed    → reduce that product's qty by the amount;
        //                    block if amount exceeds what is in the cart
        // ============================================================

        private void btnRemove_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                // Check selected cell

                if (dgvCart.CurrentCell == null ||
                    dgvCart.CurrentCell.OwningRow.IsNewRow)
                {
                    MessageBox.Show(
                        "Select a product from cart first.");

                    return;
                }



                // Ensure ProductName is selected

                if (dgvCart.CurrentCell.OwningColumn.Name != "ProductName")
                {
                    MessageBox.Show(
                        "Please select the Product Name.");

                    return;
                }



                DataGridViewRow selected =
                    dgvCart.CurrentCell.OwningRow;



                int currentQty =
                    Convert.ToInt32(
                        selected.Cells["Quantity"].Value);



                // =====================================================
                // No quantity entered → remove complete row
                // =====================================================

                if (string.IsNullOrWhiteSpace(txtQuantity.Text))
                {
                    dgvCart.Rows.Remove(selected);

                    CalculateCartTotal();


                    return;
                }



                // =====================================================
                // Quantity entered → partial removal
                // =====================================================

                if (!int.TryParse(
                    txtQuantity.Text,
                    out int removeQty) ||
                    removeQty <= 0)
                {
                    MessageBox.Show(
                        "Enter a valid quantity.");

                    return;
                }



                // Cannot remove more than cart quantity

                if (removeQty > currentQty)
                {
                    MessageBox.Show(
                        $"Cannot remove {removeQty}. " +
                        $"Only {currentQty} unit(s) in cart.");

                    return;
                }



                // Remove complete row if quantity matches

                if (removeQty == currentQty)
                {
                    dgvCart.Rows.Remove(selected);
                }
                else
                {
                    // Reduce only quantity

                    decimal unitPrice =
                        Convert.ToDecimal(
                            selected.Cells["UnitPrice"].Value);


                    int remainingQty =
                        currentQty - removeQty;


                    selected.Cells["Quantity"].Value =
                        remainingQty;


                    selected.Cells["TotalAmount"].Value =
                        remainingQty * unitPrice;
                }



                txtQuantity.Clear();


                CalculateCartTotal();

            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Remove failed: " + ex.Message);
            }
        }



        // ============================================================
        // Refresh stock list
        // ============================================================

        private void btnRefresh_Click(
            object sender,
            EventArgs e)
        {
            ClearSaleForm();

            LoadAvailableProducts();
        }



        // ============================================================
        // Calculates totals
        //
        // Formula:
        //
        // Grand Total =
        // Subtotal - Discount + Tax
        // ============================================================

        private void CalculateCartTotal()
        {
            decimal subtotal = 0;


            foreach (DataGridViewRow row
                in dgvCart.Rows)
            {
                if (row.IsNewRow)
                    continue;


                subtotal +=
                    Convert.ToDecimal(
                    row.Cells["TotalAmount"]
                    .Value);
            }



            decimal discount = 0;

            decimal.TryParse(
                txtDiscount.Text,
                out discount);



            decimal taxPercent = 0;

            decimal.TryParse(
                txtTax.Text,
                out taxPercent);



            decimal tax =
                subtotal *
                taxPercent /
                100;



            decimal grandTotal =
                subtotal -
                discount +
                tax;



            lblSubtotalValue.Text =
                "Rs. "
                + subtotal.ToString("N2");


            lblGrandTotalValue.Text =
                "Rs. "
                + grandTotal.ToString("N2");
        }



        // ============================================================
        // Loads completed sales
        //
        // Shows previous invoices in grid.
        // ============================================================

        private void LoadCompletedSales()
        {
            try
            {
                DataTable dt =
                    DBHelper.ExecuteDataTable(
                    @"SELECT TOP 20
                s.InvoiceNo,
                c.CustomerName,
                s.SaleDate,
                s.GrandTotal,
                s.PaymentStatus

              FROM Sales s

              INNER JOIN Customers c
              ON s.CustomerID =
                 c.CustomerID

              ORDER BY s.SaleID DESC",
                    null);



                dgvCompletedSales.Rows.Clear();



                foreach (DataRow row in dt.Rows)
                {
                    dgvCompletedSales.Rows.Add(
                        row["InvoiceNo"],
                        row["CustomerName"],
                        Convert.ToDateTime(
                            row["SaleDate"])
                            .ToShortDateString(),
                        row["GrandTotal"],
                        row["PaymentStatus"]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error loading sales: "
                    + ex.Message);
            }
        }
        // ============================================================
        // Completes sale
        //
        // Process:
        // 1. Validate sale data
        // 2. Insert Sales record
        // 3. Insert SaleItems
        // 4. Reduce Production stock
        // 5. Insert payment
        // 6. Commit transaction
        // ============================================================

        private void btnCompleteSale_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                if (cmbCustomer.SelectedIndex <= 0)
                {
                    MessageBox.Show(
                        "Select customer.");

                    return;
                }


                if (dgvCart.Rows.Count == 0)
                {
                    MessageBox.Show(
                        "Cart is empty.");

                    return;
                }


                if (cmbPaymentMethod.SelectedIndex <= 0 ||
                   cmbPaymentStatus.SelectedIndex <= 0)
                {
                    MessageBox.Show(
                        "Select payment details.");

                    return;
                }



                CustomerItem customer =
                    (CustomerItem)cmbCustomer.SelectedItem;



                decimal subtotal = 0;


                foreach (DataGridViewRow row
                    in dgvCart.Rows)
                {
                    subtotal +=
                        Convert.ToDecimal(
                        row.Cells["TotalAmount"]
                        .Value);
                }



                decimal discount = 0;

                decimal.TryParse(
                    txtDiscount.Text,
                    out discount);



                decimal taxPercent = 0;

                decimal.TryParse(
                    txtTax.Text,
                    out taxPercent);



                decimal tax =
                    subtotal *
                    taxPercent /
                    100;


                decimal grandTotal =
                    subtotal -
                    discount +
                    tax;



                using SqlConnection con =
                    new SqlConnection(
                        DBHelper.ConnectionString);


                con.Open();


                using SqlTransaction tr =
                    con.BeginTransaction();


                try
                {
                    int saleID;



                    // ====================================================
                    // Insert Sale
                    // ====================================================

                    string saleQuery =
                    @"
            INSERT INTO Sales
            (
                InvoiceNo,
                CustomerID,
                SaleDate,
                SubTotal,
                Discount,
                Tax,
                GrandTotal,
                PaymentStatus,
                PaymentMethod
            )
            VALUES
            (
                @invoice,
                @customer,
                GETDATE(),
                @subtotal,
                @discount,
                @tax,
                @total,
                @status,
                @method
            );

            SELECT SCOPE_IDENTITY();
            ";



                    using (SqlCommand cmd =
                        new SqlCommand(
                            saleQuery,
                            con,
                            tr))
                    {
                        cmd.Parameters.AddWithValue(
                            "@invoice",
                            "INV-" +
                            DateTime.Now.Ticks);


                        cmd.Parameters.AddWithValue(
                            "@customer",
                            customer.ID);


                        cmd.Parameters.AddWithValue(
                            "@subtotal",
                            subtotal);


                        cmd.Parameters.AddWithValue(
                            "@discount",
                            discount);


                        cmd.Parameters.AddWithValue(
                            "@tax",
                            tax);


                        cmd.Parameters.AddWithValue(
                            "@total",
                            grandTotal);


                        cmd.Parameters.AddWithValue(
                            "@status",
                            cmbPaymentStatus.SelectedItem
                            .ToString());


                        cmd.Parameters.AddWithValue(
                            "@method",
                            cmbPaymentMethod.SelectedItem
                            .ToString());


                        saleID =
                            Convert.ToInt32(
                                cmd.ExecuteScalar());


                        lastSaleID =
                            saleID;
                    }



                    // ====================================================
                    // Insert items and reduce stock
                    // ====================================================

                    foreach (DataGridViewRow row
                        in dgvCart.Rows)
                    {
                        int productionID =
                            Convert.ToInt32(
                            row.Cells["ProductionID"]
                            .Value);


                        string product =
                            row.Cells["ProductName"]
                            .Value
                            .ToString();



                        int quantity =
                            Convert.ToInt32(
                            row.Cells["Quantity"]
                            .Value);



                        decimal price =
                            Convert.ToDecimal(
                            row.Cells["UnitPrice"]
                            .Value);



                        decimal amount =
                            Convert.ToDecimal(
                            row.Cells["TotalAmount"]
                            .Value);



                        // Add sale item

                        string itemQuery =
                        @"
                INSERT INTO SaleItems
                (
                    SaleID,
                    ProductionID,
                    ProductName,
                    Quantity,
                    UnitPrice,
                    TotalAmount
                )
                VALUES
                (
                    @sale,
                    @production,
                    @name,
                    @qty,
                    @price,
                    @amount
                )";



                        using (SqlCommand cmd =
                            new SqlCommand(
                                itemQuery,
                                con,
                                tr))
                        {
                            cmd.Parameters.AddWithValue(
                                "@sale",
                                saleID);


                            cmd.Parameters.AddWithValue(
                                "@production",
                                productionID);


                            cmd.Parameters.AddWithValue(
                                "@name",
                                product);


                            cmd.Parameters.AddWithValue(
                                "@qty",
                                quantity);


                            cmd.Parameters.AddWithValue(
                                "@price",
                                price);


                            cmd.Parameters.AddWithValue(
                                "@amount",
                                amount);


                            cmd.ExecuteNonQuery();
                        }



                        // Reduce stock

                        string stockQuery =
                        @"
                UPDATE Production
                SET Quantity =
                    Quantity - @qty

                WHERE ProductionID =
                    @id";


                        using (SqlCommand cmd =
                            new SqlCommand(
                                stockQuery,
                                con,
                                tr))
                        {
                            cmd.Parameters.AddWithValue(
                                "@qty",
                                quantity);


                            cmd.Parameters.AddWithValue(
                                "@id",
                                productionID);


                            cmd.ExecuteNonQuery();
                        }
                    }



                    // ====================================================
                    // Insert payment
                    // ====================================================

                    string paymentQuery =
                    @"
            INSERT INTO SalesPayment
            (
                SaleID,
                AmountPaid,
                PaymentMethod,
                PaymentDate,
                PaymentStatus
            )
            VALUES
            (
                @sale,
                @amount,
                @method,
                GETDATE(),
                @status
            )";



                    using (SqlCommand cmd =
                        new SqlCommand(
                            paymentQuery,
                            con,
                            tr))
                    {
                        cmd.Parameters.AddWithValue(
                            "@sale",
                            saleID);


                        cmd.Parameters.AddWithValue(
                            "@amount",
                            grandTotal);


                        cmd.Parameters.AddWithValue(
                            "@method",
                            cmbPaymentMethod.SelectedItem
                            .ToString());


                        cmd.Parameters.AddWithValue(
                            "@status",
                            cmbPaymentStatus.SelectedItem
                            .ToString());


                        cmd.ExecuteNonQuery();
                    }



                    tr.Commit();



                    Logger.AddLog(
                        Session.CurrentUser,
                        "SALE",
                        "Sales Dashboard",
                        $"Completed sale ID {saleID}",
                        "Success");



                    MessageBox.Show(
                        "Sale completed successfully.");



                    ClearSaleForm();

                    LoadAvailableProducts();

                    LoadCompletedSales();
                }
                catch
                {
                    tr.Rollback();

                    throw;
                }
            }
            catch (Exception ex)
            {
                Logger.AddLog(
                    Session.CurrentUser,
                    "SALE",
                    "Sales Dashboard",
                    ex.Message,
                    "Failure");


                MessageBox.Show(
                    "Sale failed: "
                    + ex.Message);
            }
        }
        // ============================================================
        // Print Invoice
        //
        // Opens invoice report for the last completed sale.
        // ============================================================

        private void btnPrintInvoice_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                if (lastSaleID == 0)
                {
                    MessageBox.Show(
                        "Complete a sale first.");

                    return;
                }



                Logger.AddLog(
                    Session.CurrentUser ?? "System",
                    "PRINT INVOICE",
                    "Sales Dashboard",
                    $"Printed invoice for Sale ID {lastSaleID}",
                    "Success");



                InvoiceReport report =
                    new InvoiceReport(
                        lastSaleID);


                report.ShowDialog();
            }
            catch (Exception ex)
            {
                Logger.AddLog(
                    Session.CurrentUser ?? "System",
                    "PRINT INVOICE",
                    "Sales Dashboard",
                    ex.Message,
                    "Failure");


                MessageBox.Show(
                    "Invoice error: "
                    + ex.Message);
            }
        }



        // ============================================================
        // Back Button
        //
        // Returns user to Sales Dashboard.
        // ============================================================

        private void btnBack_Click(
            object sender,
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



        // ============================================================
        // Clears current sale form
        //
        // Used after successful sale.
        // ============================================================

        private void ClearSaleForm()
        {
            cmbCustomer.SelectedIndex = 0;

            cmbProduct.SelectedIndex = 0;


            txtQuantity.Clear();

            txtUnitPrice.Clear();

            txtDiscount.Clear();

            txtTax.Clear();



            cmbPaymentMethod.SelectedIndex = 0;

            cmbPaymentStatus.SelectedIndex = 0;



            dgvCart.Rows.Clear();



            CalculateCartTotal();


        }



        // ============================================================
        // Gets selected customer ID
        //
        // Used if needed by other pages.
        // ============================================================

        private int GetSelectedCustomerID()
        {
            if (cmbCustomer.SelectedItem
                is CustomerItem customer)
            {
                return customer.ID;
            }


            return 0;
        }



        // ============================================================
        // UserControl Dispose
        // ============================================================

        protected override void Dispose(
            bool disposing)
        {
            if (disposing &&
               components != null)
            {
                components.Dispose();
            }


            base.Dispose(disposing);
        }

    }
}