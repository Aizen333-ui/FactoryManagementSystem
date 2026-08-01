using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SalesDashboard.Pages
{
    partial class NewSale
    {
        private System.ComponentModel.IContainer components = null;


        // ============================================================
        // Controls
        // ============================================================

        private ComboBox cmbCustomer;
        private ComboBox cmbProduct;
        private ComboBox cmbPaymentMethod;
        private ComboBox cmbPaymentStatus;

        private TextBox txtQuantity;
        private TextBox txtUnitPrice;
        private TextBox txtDiscount;
        private TextBox txtTax;

        private ListView lstAvailableProducts;

        private DataGridView dgvCart;
        private DataGridView dgvCompletedSales;

        private Label lblSubtotalValue;
        private Label lblGrandTotalValue;

        private Button btnAddToCart;
        private Button btnRemove;
        private Button btnRefresh;

        private Button btnCompleteSale;
        private Button btnPrintInvoice;
        private Button btnBack;



        // ============================================================
        // Rounded textbox / combobox container
        // BOX HEIGHT = 46px so the input sits inside with padding.
        // ============================================================

        private Panel CreateRoundedBox(Control inner)
        {
            Panel box =
            new Panel
            {
                Width = inner.Width + 24,
                Height = 50,
                BackColor = Color.White
            };


            if (inner is ComboBox cb)
            {
                cb.FlatStyle = FlatStyle.Flat;
                cb.Font = new Font("Segoe UI", 13F);
                cb.DropDownStyle = ComboBoxStyle.DropDownList;

                cb.Margin = Padding.Empty;

                cb.Height = 36;

                cb.Width = box.Width - 24;

                cb.Location =
                    new Point(
                        12,
                        (box.Height - cb.Height) / 2);
            }
            else if (inner is TextBox tb)
            {
                tb.BorderStyle =
                    BorderStyle.None;

                tb.Font =
                    new Font(
                        "Segoe UI",
                        13F);

                tb.Height = 36;

                tb.Width = box.Width - 24;

                tb.Location =
                    new Point(
                        12,
                        (box.Height - tb.Height) / 2);
            }
            else
            {
                inner.Location =
                    new Point(
                        12,
                        (box.Height - inner.Height) / 2);
            }


            box.Controls.Add(inner);


            box.Paint += (s, e) =>
            {
                Rectangle rect =
                    new Rectangle(
                        0,
                        0,
                        box.Width - 1,
                        box.Height - 1);

                int radius = 12;

                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddArc(
                        rect.X,
                        rect.Y,
                        radius,
                        radius,
                        180,
                        90);

                    path.AddArc(
                        rect.Right - radius,
                        rect.Y,
                        radius,
                        radius,
                        270,
                        90);

                    path.AddArc(
                        rect.Right - radius,
                        rect.Bottom - radius,
                        radius,
                        radius,
                        0,
                        90);

                    path.AddArc(
                        rect.X,
                        rect.Bottom - radius,
                        radius,
                        radius,
                        90,
                        90);


                    path.CloseFigure();


                    e.Graphics.SmoothingMode =
                        SmoothingMode.AntiAlias;


                    using Pen pen =
                        new Pen(
                            Color.FromArgb(180, 190, 210),
                            1.5f);


                    e.Graphics.DrawPath(
                        pen,
                        path);
                }
            };


            return box;
        }



        // ============================================================
        // Rounded button
        // ============================================================

        private void MakeRoundedButton(Button btn, Color color)
        {
            btn.FlatStyle                      = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize      = 0;
            btn.UseVisualStyleBackColor        = false;
            btn.BackColor                      = color;
            btn.ForeColor                      = Color.White;
            btn.FlatAppearance.MouseOverBackColor  = ControlPaint.Light(color, 0.15f);
            btn.FlatAppearance.MouseDownBackColor  = ControlPaint.Dark(color, 0.1f);
            btn.Font                           = new Font("Segoe UI", 13F, FontStyle.Bold);
            ApplyRoundedRegion(btn, 18);
        }

        private void ApplyRoundedRegion(Button btn, int radius)
        {
            Rectangle rect = btn.ClientRectangle;

            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddArc(rect.X,              rect.Y,               radius, radius, 180, 90);
                path.AddArc(rect.Right - radius, rect.Y,               radius, radius, 270, 90);
                path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius,   0, 90);
                path.AddArc(rect.X,              rect.Bottom - radius, radius, radius,  90, 90);
                path.CloseFigure();
                btn.Region = new Region(path);
            }
        }



        // ============================================================
        // Label helpers
        // ============================================================

        // Bold section heading, flow-layout safe (uses Margin)
        private Label SectionLabel(string text, int topMargin = 20)
        {
            return new Label
            {
                Text     = text,
                Font     = new Font("Segoe UI", 15F, FontStyle.Bold),
                AutoSize = true,
                Margin   = new Padding(0, topMargin, 0, 6)
            };
        }

        // Small field label placed by absolute position inside a fixed panel
        private Label MakeLabel(string text, int x, int y)
        {
            return new Label
            {
                Text      = text,
                Font      = new Font("Segoe UI", 13F),
                AutoSize  = true,
                Location  = new Point(x, y)
            };
        }



        // ============================================================
        // InitializeComponent
        // ============================================================

        private void InitializeComponent()
        {
            SuspendLayout();

            this.Size      = new Size(1300, 900);
            this.BackColor = Color.White;


            // ── Root: top-down FlowLayoutPanel ───────────────────────

            FlowLayoutPanel main = new FlowLayoutPanel
            {
                Dock          = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents  = false,
                AutoScroll    = true,
                Padding       = new Padding(40, 20, 40, 40)
            };


            // ─────────────────────────────────────────────────────────
            // TITLE
            // ─────────────────────────────────────────────────────────

            Label title = new Label
            {
                Text     = "New Sale",
                Font     = new Font("Segoe UI", 22F, FontStyle.Bold),
                AutoSize = true,
                Margin   = new Padding(0, 0, 0, 16)
            };
            main.Controls.Add(title);


            // ─────────────────────────────────────────────────────────
            // CUSTOMER  (label + box stacked in a tiny panel so the
            //            flow-layout item has a predictable height)
            // ─────────────────────────────────────────────────────────

            Panel customerPanel = new Panel
            {
                Width  = 640,
                Height = 88,           // 22 label + 4 gap + 46 box + 8 bottom
                Margin = new Padding(0, 0, 0, 18)
            };

            customerPanel.Controls.Add(MakeLabel("Customer", 0, 0));

            cmbCustomer = new ComboBox { Width = 616 };
            Panel customerBox = CreateRoundedBox(cmbCustomer);
            customerBox.Location = new Point(0, 37);
            customerPanel.Controls.Add(customerBox);

            main.Controls.Add(customerPanel);


            // ─────────────────────────────────────────────────────────
            // PRODUCT DETAILS  (left inputs | right stock list)
            // ─────────────────────────────────────────────────────────

            main.Controls.Add(SectionLabel("Product Details", 0));

            // Row heights inside the left panel:
            //   label  = 22 px
            //   gap    =  6 px
            //   box    = 46 px
            //   spacer = 18 px between fields
            // Total per field = 92 px

            // Field positions (Y)
            // Product  label=0   box=28
            // Qty      label=92  box=120
            // Price    label=184 box=212
            // Buttons  Y=280
            int lH = 400;

            Panel productRow = new Panel
            {
                Width  = 1200,
                Height = lH,
                Margin = new Padding(0, 0, 0, 18)
            };

            // ── LEFT ─────────────────────────────────────────────────

            Panel left = new Panel
            {
                Width    = 580,
                Height   = lH,
                Location = new Point(0, 0)
            };

            // Product
            left.Controls.Add(MakeLabel("Product", 0, 0));
            cmbProduct = new ComboBox { Width = 556 };
            Panel productBox = CreateRoundedBox(cmbProduct);
            productBox.Location = new Point(0, 40);
            left.Controls.Add(productBox);

            // Quantity
            left.Controls.Add(MakeLabel("Quantity", 0, 92));
            txtQuantity = new TextBox { Width = 556 };
            Panel qtyBox = CreateRoundedBox(txtQuantity);
            qtyBox.Location = new Point(0, 140);
            left.Controls.Add(qtyBox);

            // Unit Price
            left.Controls.Add(MakeLabel("Unit Price", 0, 195));
            txtUnitPrice = new TextBox { Width = 556 };
            Panel priceBox = CreateRoundedBox(txtUnitPrice);
            priceBox.Location = new Point(0, 240);
            left.Controls.Add(priceBox);

            // Buttons
            FlowLayoutPanel cartButtons = new FlowLayoutPanel
            {
                Width    = 580,
                Height   = 58,
                Location = new Point(0, 330),
                Margin   = Padding.Empty
            };

            btnAddToCart = new Button { Text = "Add To Cart", Width = 175, Height = 50 };
            btnRemove    = new Button { Text = "Remove",      Width = 155, Height = 50 };
            btnRefresh   = new Button { Text = "Refresh",     Width = 155, Height = 50 };

            MakeRoundedButton(btnAddToCart, Color.FromArgb(34, 197, 94));
            MakeRoundedButton(btnRemove,    Color.FromArgb(239, 68, 68));
            MakeRoundedButton(btnRefresh,   Color.FromArgb(59, 130, 246));

            cartButtons.Controls.Add(btnAddToCart);
            cartButtons.Controls.Add(btnRemove);
            cartButtons.Controls.Add(btnRefresh);
            left.Controls.Add(cartButtons);


            // ── RIGHT ────────────────────────────────────────────────

            Panel right = new Panel
            {
                Width    = 580,
                Height   = lH,
                Location = new Point(610, 0)
            };

            right.Controls.Add(MakeLabel("Available Products", 160, 0));

            lstAvailableProducts = new ListView
            {
                Width         = 560,
                Height        = 360,
                Location      = new Point(20, 40),
                View          = View.Details,
                FullRowSelect = true,
                GridLines     = true
            };
            lstAvailableProducts.Columns.Add("Product",  350);
            lstAvailableProducts.Columns.Add("In Stock", 140);
            right.Controls.Add(lstAvailableProducts);


            productRow.Controls.Add(left);
            productRow.Controls.Add(right);
            main.Controls.Add(productRow);


            // ─────────────────────────────────────────────────────────
            // CURRENT CART
            // ─────────────────────────────────────────────────────────

            main.Controls.Add(SectionLabel("Current Cart"));

            dgvCart = new DataGridView
            {
                Width               = 1200,
                Height              = 200,
                ReadOnly            = true,
                AllowUserToAddRows  = false,
                RowHeadersVisible   = false,
                SelectionMode       = DataGridViewSelectionMode.CellSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor     = Color.White,
                Margin              = new Padding(0, 0, 0, 18)
            };

            dgvCart.Columns.Add("ProductionID", "ID");
            dgvCart.Columns["ProductionID"].Visible = false;
            dgvCart.Columns.Add("ProductName",  "Product");
            dgvCart.Columns.Add("Quantity",     "Qty");
            dgvCart.Columns.Add("UnitPrice",    "Unit Price");
            dgvCart.Columns.Add("TotalAmount",  "Total");

            main.Controls.Add(dgvCart);


            // ─────────────────────────────────────────────────────────
            // ORDER SUMMARY
            // Layout (all inside a fixed-height panel):
            //
            //   Row 0  (Y=0)  : Subtotal label  |  Subtotal value
            //   Row 1  (Y=54) : Discount label  |  discount box  |  Tax label  |  tax box
            //   Row 2  (Y=118): Grand Total label
            //   Row 3  (Y=148): Grand Total value
            // Panel height = 200
            // ─────────────────────────────────────────────────────────

            main.Controls.Add(SectionLabel("Order Summary"));

            Panel totals = new Panel
            {
                Width  = 1200,
                Height = 200,
                Margin = new Padding(0, 0, 0, 18)
            };

            // Row 0 – Subtotal
            totals.Controls.Add(MakeLabel("Subtotal:", 0, 4));

            lblSubtotalValue = new Label
            {
                Text      = "Rs. 0.00",
                Font      = new Font("Segoe UI", 13F, FontStyle.Bold),
                AutoSize  = true,
                Location  = new Point(160, 4)
            };
            totals.Controls.Add(lblSubtotalValue);

            // Row 1 – Discount + Tax
            totals.Controls.Add(MakeLabel("Discount:", 0, 58));

            txtDiscount = new TextBox { Width = 210 };
            Panel discountBox = CreateRoundedBox(txtDiscount);
            discountBox.Location = new Point(125, 52);
            totals.Controls.Add(discountBox);

            totals.Controls.Add(MakeLabel("Tax (%):", 400, 58));

            txtTax = new TextBox { Width = 210 };
            Panel taxBox = CreateRoundedBox(txtTax);
            taxBox.Location = new Point(505, 52);
            totals.Controls.Add(taxBox);

            // Row 2 – Grand Total label
            Label lblGrand = new Label
            {
                Text      = "Grand Total:",
                Font      = new Font("Segoe UI", 14F, FontStyle.Bold),
                AutoSize  = true,
                Location  = new Point(0, 118)
            };
            totals.Controls.Add(lblGrand);

            // Row 3 – Grand Total value
            lblGrandTotalValue = new Label
            {
                Text      = "Rs. 0.00",
                Font      = new Font("Segoe UI", 20F, FontStyle.Bold),
                AutoSize  = true,
                Location  = new Point(0, 152)
            };
            totals.Controls.Add(lblGrandTotalValue);

            main.Controls.Add(totals);


            // ─────────────────────────────────────────────────────────
            // PAYMENT
            // Layout inside fixed-height panel:
            //   Y=0  : Payment Method label
            //   Y=28 : payment combo box
            //   Y=0  : Payment Status label   (X=460)
            //   Y=28 : status combo box        (X=460)
            // Panel height = 90
            // ─────────────────────────────────────────────────────────

            main.Controls.Add(SectionLabel("Payment"));

            Panel paymentRow = new Panel
            {
                Width  = 1200,
                Height = 90,
                Margin = new Padding(0, 0, 0, 20)
            };

            paymentRow.Controls.Add(MakeLabel("Payment Method", 0, 0));

            cmbPaymentMethod = new ComboBox { Width = 390 };
            Panel paymentBox = CreateRoundedBox(cmbPaymentMethod);
            paymentBox.Location = new Point(0, 40);
            paymentRow.Controls.Add(paymentBox);

            paymentRow.Controls.Add(MakeLabel("Payment Status", 460, 0));

            cmbPaymentStatus = new ComboBox { Width = 390 };
            Panel statusBox = CreateRoundedBox(cmbPaymentStatus);
            statusBox.Location = new Point(460, 40);
            paymentRow.Controls.Add(statusBox);

            main.Controls.Add(paymentRow);


            // ─────────────────────────────────────────────────────────
            // ACTION BUTTONS
            // ─────────────────────────────────────────────────────────

            FlowLayoutPanel actions = new FlowLayoutPanel
            {
                Width  = 1200,
                Height = 66,
                Margin = new Padding(0, 0, 0, 28)
            };

            btnCompleteSale = new Button { Text = "Complete Sale", Width = 220, Height = 50 };
            btnPrintInvoice = new Button { Text = "Print Invoice", Width = 200, Height = 50 };
            btnBack         = new Button { Text = "Back",          Width = 160, Height = 50 };

            MakeRoundedButton(btnCompleteSale, Color.FromArgb(94, 60, 255));
            MakeRoundedButton(btnPrintInvoice, Color.FromArgb(59, 130, 246));
            MakeRoundedButton(btnBack,         Color.Gray);

            actions.Controls.Add(btnCompleteSale);
            actions.Controls.Add(btnPrintInvoice);
            actions.Controls.Add(btnBack);

            main.Controls.Add(actions);


            // ─────────────────────────────────────────────────────────
            // COMPLETED SALES  (after buttons)
            // ─────────────────────────────────────────────────────────

            main.Controls.Add(SectionLabel("Completed Sales", 0));

            dgvCompletedSales = new DataGridView
            {
                Width               = 1200,
                Height              = 220,
                ReadOnly            = true,
                AllowUserToAddRows  = false,
                RowHeadersVisible   = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor     = Color.White,
                Margin              = new Padding(0, 0, 0, 30)
            };

            dgvCompletedSales.Columns.Add("Invoice",  "Invoice");
            dgvCompletedSales.Columns.Add("Customer", "Customer");
            dgvCompletedSales.Columns.Add("Date",     "Date");
            dgvCompletedSales.Columns.Add("Amount",   "Amount");
            dgvCompletedSales.Columns.Add("Status",   "Status");

            main.Controls.Add(dgvCompletedSales);


            // ─────────────────────────────────────────────────────────
            // Finish
            // ─────────────────────────────────────────────────────────

            this.Controls.Add(main);
            this.BackColor = Color.White;

            ResumeLayout(false);
        }
    }
}