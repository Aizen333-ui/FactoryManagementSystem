using System.Drawing.Drawing2D;

namespace FactoryManagementSystem
{
    partial class RawMaterial
    {
        // ===================== COMPONENT DECLARATIONS =====================

        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbName;

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtQty;

        private System.Windows.Forms.Label labelUnit;
        private System.Windows.Forms.TextBox txtUnit;
        private Label lblUnitPrice;
        private TextBox txtUnitPrice;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dateAdded;

        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnRemove;

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnBack;
        // ===================== ROUNDED INPUT BOX =====================
        // Creates modern card-style container for inputs
        private Panel CreateRoundedBox(Control innerControl, int height = 55)
        {
            Panel container = new Panel();
            container.Height = (innerControl.Height > 0) ? innerControl.Height + 20 : height;
            container.Width = (innerControl.Width > 0) ? innerControl.Width + 24 : 800;
            container.BackColor = Color.White;
            container.Padding = new Padding(12, 10, 14, 12);
            container.Margin = new Padding(0, 0, 0, 15);
            container.Size = new Size(container.Width, container.Height + 2);
            container.AutoSize = false;
            container.Anchor = AnchorStyles.Left;   
            innerControl.Dock = DockStyle.Fill;
            innerControl.BackColor = Color.White;
            innerControl.Margin = new Padding(0);
            if (innerControl is TextBox tb)
            {
                tb.BorderStyle = BorderStyle.None;
                tb.Multiline = false;   
                tb.Font = new Font("Segoe UI", 12F);
                tb.Height = 30;
                tb.TextAlign = HorizontalAlignment.Left;
                tb.Margin = new Padding(0);
            }
            if (innerControl is ComboBox cb)
            {
                cb.FlatStyle = FlatStyle.Flat;
                cb.Font = new Font("Segoe UI", 12F);
                cb.Dock = DockStyle.Fill;
            }

            if (innerControl is DateTimePicker dt)
            {
                dt.Font = new Font("Segoe UI", 12F);
                dt.Dock = DockStyle.Fill;
                dt.CalendarFont = new Font("Segoe UI", 12F);
                dt.Format = DateTimePickerFormat.Custom;
                dt.CustomFormat = "dd/MM/yyyy";
                dt.CalendarMonthBackground = Color.White;
                dt.CalendarForeColor = Color.Black;
                dt.BackColor = Color.White;
            }
            container.Controls.Add(innerControl);

            container.Paint += (s, e) =>
            {
                int radius = 12;

                Rectangle rect = new Rectangle(1, 1, container.Width - 2, container.Height - 2);

                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
                    path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
                    path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
                    path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);
                    path.CloseAllFigures();

                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                    // Fill
                    using (SolidBrush brush = new SolidBrush(Color.White))
                        e.Graphics.FillPath(brush, path);

                    using (Pen pen = new Pen(Color.FromArgb(180, 190, 210), 1.5f))
                        e.Graphics.DrawPath(pen, path);
                }
            };

            return container;
        }
        // ===================== ROUND CONTROL UTILITY =====================
        // Applies rounded corners to buttons/controls

        private void RoundControl(Control ctl, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddArc(0, 0, radius, radius, 180, 90);
            path.AddArc(ctl.Width - radius, 0, radius, radius, 270, 90);
            path.AddArc(ctl.Width - radius, ctl.Height - radius, radius, radius, 0, 90);
            path.AddArc(0, ctl.Height - radius, radius, radius, 90, 90);
            path.CloseAllFigures();

            ctl.Region = new Region(path);

            // Reapply on resize
            ctl.Resize += (s, e) =>
            {
                GraphicsPath p = new GraphicsPath();
                p.AddArc(0, 0, radius, radius, 180, 90);
                p.AddArc(ctl.Width - radius, 0, radius, radius, 270, 90);
                p.AddArc(ctl.Width - radius, ctl.Height - radius, radius, radius, 0, 90);
                p.AddArc(0, ctl.Height - radius, radius, radius, 90, 90);
                p.CloseAllFigures();
                ctl.Region = new Region(p);
            };
            btnAdd.MouseEnter += (s, e) => btnAdd.Invalidate();
            btnAdd.MouseLeave += (s, e) => btnAdd.Invalidate();
            btnAdd.MouseDown += (s, e) => btnAdd.Invalidate();
            btnAdd.MouseUp += (s, e) => btnAdd.Invalidate();
            btnRemove.MouseEnter += (s, e) => btnRemove.Invalidate();
            btnRemove.MouseLeave += (s, e) => btnRemove.Invalidate();
            btnRemove.MouseDown += (s, e) => btnRemove.Invalidate();
            btnRemove.MouseUp += (s, e) => btnRemove.Invalidate();
            btnBack.MouseEnter += (s, e) => btnBack.Invalidate();
            btnBack.MouseLeave += (s, e) => btnBack.Invalidate();
            btnBack.MouseDown += (s, e) => btnBack.Invalidate();
            btnBack.MouseUp += (s, e) => btnBack.Invalidate();

        }
        // ===================== INITIALIZE UI =====================

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // MAIN CONTAINER (vertical layout)
            FlowLayoutPanel main = new FlowLayoutPanel();
            main.Dock = DockStyle.Fill;
            main.FlowDirection = FlowDirection.TopDown;
            main.WrapContents = false;
            main.AutoScroll = true;
            main.Padding = new Padding(30, 20, 30, 300);

            // ===== TITLE =====
            Label label1 = new Label();
            label1.Text = "Raw Material Management";
            label1.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            label1.AutoSize = true;
            label1.Margin = new Padding(0, 0, 0, 20);

            // ===== MATERIAL =====
            Label label2 = new Label();
            label2.Text = "Material:";
            label2.Font = new Font("Segoe UI", 14F);
            label2.AutoSize = true;

            ComboBox cmbName = this.cmbName = new ComboBox();
            cmbName.Width = 1000;
            cmbName.Height = 30;
            cmbName.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbName.Items.AddRange(new object[]
            {
               "Select the Raw Material...", "Cement", "Sand", "Crush", "Steel", "Mold Oil"
            });

            // ===== QUANTITY =====
            Label label3 = new Label();
            label3.Text = "Quantity:";
            label3.Font = new Font("Segoe UI", 14F);
            label3.AutoSize = true;

            this.txtQty = new System.Windows.Forms.TextBox();
            this.txtQty.Width = 1000;
            this.txtQty.Height = 30;

            // ===== UNIT =====
            Label labelUnit = new Label();
            labelUnit.Text = "Unit:";
            labelUnit.Font = new Font("Segoe UI", 14F);
            labelUnit.AutoSize = true;

            this.txtUnit = new System.Windows.Forms.TextBox();
            this.txtUnit.Width = 1000;
            this.txtUnit.Height = 30;
            this.txtUnit.ReadOnly = true;

            //===== UNIT PRICE =====
            this.txtUnitPrice = new TextBox();
            this.lblUnitPrice = new Label();
            lblUnitPrice.Text = "Unit Price:";
            lblUnitPrice.Font = new Font("Segoe UI", 14F);
            lblUnitPrice.AutoSize = true;

            txtUnitPrice.Width = 1000;
            txtUnitPrice.Height = 30;
            // ===== DATE =====
            Label label4 = new Label();
            label4.Text = "Date Added:";
            label4.Font = new Font("Segoe UI", 14F);
            label4.AutoSize = true;

            DateTimePicker dateAdded = this.dateAdded = new DateTimePicker();
            dateAdded.Width = 1000;
            dateAdded.Height = 35;
            dateAdded.Font = new Font("Segoe UI", 14F);
            // keep UI consistent with CreateRoundedBox date formatting
            dateAdded.Format = DateTimePickerFormat.Custom;
            dateAdded.CustomFormat = "dd/MM/yyyy";

            // ===== BUTTONS =====
            Panel btnPanel = new Panel();
            btnPanel.Width = 600;
            btnPanel.Height = 80;
            btnPanel.AutoSize = true;
            btnPanel.Margin = new Padding(0, 20, 0, 20);

            this.btnAdd = new Button();
            btnAdd.Text = "Add Material";
            btnAdd.Width = 260;
            btnAdd.Height = 50;
            btnAdd.FlatStyle = FlatStyle.Flat;
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.ForeColor = Color.White;
            btnAdd.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            btnAdd.Location = new Point(300, 10);
            this.btnRemove = new Button();
            btnRemove.Text = "Remove Material";
            btnRemove.Width = 260;
            btnRemove.Height = 50;
            btnRemove.FlatStyle = FlatStyle.Flat;
            btnRemove.FlatAppearance.BorderSize = 0;
            btnRemove.ForeColor = Color.White;
            btnRemove.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            btnRemove.Location = new Point(600, 10);
            this.btnBack = new Button();
            btnBack.Text = "Back";
            btnBack.Width = 260;
            btnBack.Height = 50;
            btnBack.FlatStyle = FlatStyle.Flat;
            btnBack.FlatAppearance.BorderSize = 0;
            btnBack.ForeColor = Color.White;
            btnBack.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            btnBack.Location = new Point(260, 750);

            RoundControl(btnAdd, 16);
            RoundControl(btnRemove, 16);
            RoundControl(btnBack, 16);

            // gradient for add button
            btnAdd.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                bool hover = btnAdd.ClientRectangle.Contains(btnAdd.PointToClient(Cursor.Position));
                bool down = (Control.MouseButtons == MouseButtons.Left) && hover;

                Color baseColor = Color.FromArgb(94, 60, 255);
                Color hoverColor = Color.FromArgb(120, 85, 255);
                Color downColor = Color.FromArgb(70, 40, 200);

                Color useColor = baseColor;

                if (down)
                    useColor = downColor;
                else if (hover)
                    useColor = hoverColor;

                using var brush = new LinearGradientBrush(
                    btnAdd.ClientRectangle,
                    useColor,
                    Color.FromArgb(168, 85, 247),
                    45F);

                e.Graphics.FillRectangle(brush, btnAdd.ClientRectangle);

                TextRenderer.DrawText(
                    e.Graphics,
                    btnAdd.Text,
                    btnAdd.Font,
                    btnAdd.ClientRectangle,
                    btnAdd.ForeColor,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            };
            btnRemove.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                bool hover = btnRemove.ClientRectangle.Contains(btnRemove.PointToClient(Cursor.Position));
                bool down = (Control.MouseButtons == MouseButtons.Left) && hover;

                Color baseColor = Color.FromArgb(220, 38, 38);
                Color hoverColor = Color.FromArgb(255, 70, 70);
                Color downColor = Color.FromArgb(180, 20, 20);

                Color useColor = baseColor;

                if (down)
                    useColor = downColor;
                else if (hover)
                    useColor = hoverColor;

                using var brush = new SolidBrush(useColor);

                e.Graphics.FillRectangle(brush, btnRemove.ClientRectangle);

                TextRenderer.DrawText(
                    e.Graphics,
                    btnRemove.Text,
                    btnRemove.Font,
                    btnRemove.ClientRectangle,
                    btnRemove.ForeColor,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            };
            btnBack.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                bool hover = btnBack.ClientRectangle.Contains(btnBack.PointToClient(Cursor.Position));
                bool down = (Control.MouseButtons == MouseButtons.Left) && hover;

                Color baseColor = Color.FromArgb(100, 100, 100);
                Color hoverColor = Color.FromArgb(120, 120, 120);
                Color downColor = Color.FromArgb(70, 70, 70);

                Color useColor = baseColor;

                if (down)
                    useColor = downColor;
                else if (hover)
                    useColor = hoverColor;

                using var brush = new SolidBrush(useColor);

                e.Graphics.FillRectangle(brush, btnBack.ClientRectangle);

                TextRenderer.DrawText(
                    e.Graphics,
                    btnBack.Text,
                    btnBack.Font,
                    btnBack.ClientRectangle,
                    btnBack.ForeColor,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            };

            btnPanel.Controls.Add(btnAdd);
            btnPanel.Controls.Add(btnRemove);
            // wire click handlers
            btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            btnRemove.Click += new System.EventHandler(this.BtnRemove_Click);
            btnBack.Click += new System.EventHandler(this.btnBack_Click);

            // ===== INVENTORY BOX =====
            Panel inventoryBox = new Panel();
            inventoryBox.Width = 600;
            inventoryBox.Height = 70;
            inventoryBox.BackColor = Color.White;
            inventoryBox.Margin = new Padding(0, 0, 0, 20);

            Label invLabel = new Label();
            invLabel.Text = "Current Inventory";
            invLabel.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            invLabel.Location = new Point(15, 20);
            invLabel.AutoSize = true;

            inventoryBox.Controls.Add(invLabel);

            // ===== DATAGRID =====
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.dataGridView1.Width = 1160;
            this.dataGridView1.Height = 400;
            this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.ReadOnly = true; 

            // ===== ADD ALL TO MAIN =====
            // Title
            main.Controls.Add(label1);

            // Material
            main.Controls.Add(label2);
            main.Controls.Add(CreateRoundedBox(cmbName));

            // Quantity
            main.Controls.Add(label3);
            main.Controls.Add(CreateRoundedBox(txtQty));
            // Unit
            main.Controls.Add(labelUnit);
            main.Controls.Add(CreateRoundedBox(txtUnit));

            // Unit Price
            main.Controls.Add(lblUnitPrice);
            main.Controls.Add(CreateRoundedBox(txtUnitPrice));
            // Date
            main.Controls.Add(label4);
            // Date picker displayed 
            main.Controls.Add(dateAdded);

            // Buttons
            main.Controls.Add(btnPanel);

            // Inventory box
            main.Controls.Add(inventoryBox);

            // Data grid
            main.Controls.Add(this.dataGridView1);
            main.Controls.Add(btnBack);

            // ===== FINAL =====
            this.Controls.Add(main);
            this.ResumeLayout(false);

        }
    }
}