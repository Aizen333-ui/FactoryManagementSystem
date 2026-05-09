using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace FactoryDashBoard.Pages
{
    partial class RawMaterialUsage
    {
        private System.ComponentModel.IContainer components = null;
        // ===== UI CONTROLS =====

        private Label lblTitle;
        private Label lblMaterialName;
        private Label lblQuantity;
        private Label lblDate;

        private ComboBox cmbMaterialName; // changed from TextBox
        private TextBox txtQuantity;
        private DateTimePicker dateMaterial;

        private Button btnClear;
        private Button btnRemove;
        private Label lblMessage;
        private System.Windows.Forms.DataGridView dataGridView;
        private Button btnBack;
        // ===== APPLY ROUNDED SHAPE TO BUTTON =====

        private void MakeRoundedButton(Button btn, Color color)
        {


            this.btnClear.BackColor = Color.Gray;
            this.btnClear.FlatStyle = FlatStyle.Flat;
            this.btnClear.FlatAppearance.BorderSize = 0;
            this.btnClear.ForeColor = Color.White;
            this.btnClear.Font = new Font("Segoe UI", 13F, FontStyle.Bold);

            this.btnClear.FlatAppearance.MouseOverBackColor = Color.DarkGray;
            this.btnClear.FlatAppearance.MouseDownBackColor = Color.DimGray;
            this.btnRemove.BackColor = Color.FromArgb(220, 38, 38);
            this.btnRemove.FlatStyle = FlatStyle.Flat;
            this.btnRemove.FlatAppearance.BorderSize = 0;
            this.btnRemove.ForeColor = Color.White;
            this.btnRemove.Font = new Font("Segoe UI", 13F, FontStyle.Bold);

            this.btnRemove.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 60, 60);
            this.btnRemove.FlatAppearance.MouseDownBackColor = Color.FromArgb(180, 20, 20);


            this.btnBack.FlatStyle = FlatStyle.Flat;
            this.btnBack.FlatAppearance.BorderSize = 0;
            this.btnBack.ForeColor = Color.White;
            this.btnBack.Font = new Font("Segoe UI", 13F, FontStyle.Bold);


            btn.Resize += (s, e) =>
            {
                ApplyRoundedRegion(btn, 18);
            };

            btn.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                Rectangle rect = btn.ClientRectangle;
                int radius = 18;

                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
                    path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
                    path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
                    path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);
                    path.CloseFigure();




                }
            };

            ApplyRoundedRegion(btn, 18);
        }
        // ===== CREATE ROUNDED INPUT BOX (TEXTBOX / COMBOBOX / DATEPICKER) =====

        private Panel CreateRoundedBox(Control innerControl, int height = 55)
        {
            Panel container = new Panel();

            container.Height = (innerControl.Height > 0) ? innerControl.Height + 28 : height;
            container.Width = (innerControl.Width > 0) ? innerControl.Width + 24 : 600;

            container.BackColor = Color.White;
            container.Padding = new Padding(12, 10, 12, 10);
            container.Margin = new Padding(0, 0, 0, 25);

            innerControl.Dock = DockStyle.None;
            innerControl.Location = new Point(12, 10);
            innerControl.Margin = new Padding(0);
            innerControl.BackColor = Color.White;
            innerControl.Width = container.Width - 24;

            if (innerControl is TextBox tb)
            {
                tb.BorderStyle = BorderStyle.None;
                tb.Font = new Font("Segoe UI", 13F);
                tb.Multiline = false;
            }

            if (innerControl is ComboBox cb)
            {
                cb.FlatStyle = FlatStyle.Flat;
                cb.Font = new Font("Segoe UI", 13F);
                cb.DropDownStyle = ComboBoxStyle.DropDownList;
                cb.Height = 30;

            }

            if (innerControl is DateTimePicker dt)
            {
                dt.Font = new Font("Segoe UI", 12F);
                dt.Format = DateTimePickerFormat.Custom;
                dt.CustomFormat = "dd/MM/yyyy";
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

                    using (SolidBrush brush = new SolidBrush(Color.White))
                        e.Graphics.FillPath(brush, path);

                    using (Pen pen = new Pen(Color.FromArgb(180, 190, 210), 1.5f))
                        e.Graphics.DrawPath(pen, path);
                }
            };

            return container;
        }
        // ===== APPLY ROUNDED REGION =====

        private void ApplyRoundedRegion(Button btn, int radius)
        {
            Rectangle rect = btn.ClientRectangle;

            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
                path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
                path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
                path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);
                path.CloseFigure();

                btn.Region = new Region(path);
            }
        }
        // ===== UI INITIALIZATION =====

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // MAIN LAYOUT
            FlowLayoutPanel main = new FlowLayoutPanel();
            main.Dock = DockStyle.Fill;
            main.FlowDirection = FlowDirection.TopDown;
            main.WrapContents = false;
            main.AutoScroll = true;
            main.Padding = new Padding(50, 30, 50, 30);

            // ===== TITLE =====
            Label title = new Label();
            title.Text = "Raw Material Usage";
            title.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            title.AutoSize = true;
            title.Margin = new Padding(0, 0, 0, 20);

            // ===== MATERIAL =====
            Label lbl1 = new Label();
            lbl1.Text = "Material Name:";
            lbl1.Font = new Font("Segoe UI", 14F);
            lbl1.AutoSize = true;
            lbl1.Margin = new Padding(0, 15, 0, 8);
            this.cmbMaterialName = new ComboBox();
            this.cmbMaterialName.Width = 1160;
            this.cmbMaterialName.Items.AddRange(new object[]
            {
    "Cement", "Sand", "Crush", "Steel", "Mold Oil"
            });

            // ===== QUANTITY =====
            Label lbl2 = new Label();
            lbl2.Text = "Quantity:";
            lbl2.Font = new Font("Segoe UI", 14F);
            lbl2.AutoSize = true;
            lbl2.Margin = new Padding(0, 15, 0, 8);
            this.txtQuantity = new TextBox();
            this.txtQuantity.Width = 1160;
            this.txtQuantity.Font = new Font("Segoe UI", 12F);

            // ===== DATE =====
            Label lbl3 = new Label();
            lbl3.Text = "Date:";
            lbl3.Font = new Font("Segoe UI", 14F);
            lbl3.AutoSize = true;
            lbl3.Margin = new Padding(0, 15, 0, 8);
            this.dateMaterial = new DateTimePicker();
            this.dateMaterial.Width = 1160;
            this.dateMaterial.Font = new Font("Segoe UI", 12F);
            this.dateMaterial.Format = DateTimePickerFormat.Custom;
            this.dateMaterial.CustomFormat = "dd/MM/yyyy";

            // ===== BUTTONS =====
            Panel btnPanel = new Panel();
            btnPanel.Width = 1500;
            btnPanel.Height = 80;
            btnPanel.AutoSize = true;
            btnPanel.Margin = new Padding(0, 20, 0, 20);




            // CLEAR BUTTON (gray style)
            this.btnClear = new Button();
            this.btnClear.Text = "Clear";
            this.btnClear.Width = 260;
            this.btnClear.Height = 50;
            this.btnClear.FlatStyle = FlatStyle.Flat;
            this.btnClear.ForeColor = Color.White;
            this.btnClear.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            this.btnClear.Location = new Point(700, 10);


            // REMOVE BUTTON (red like RawMaterial)
            this.btnRemove = new Button();
            this.btnRemove.Text = "Remove Entry";
            this.btnRemove.Width = 260;
            this.btnRemove.Height = 50;
            this.btnRemove.FlatStyle = FlatStyle.Flat;
            this.btnRemove.ForeColor = Color.White;
            this.btnRemove.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            this.btnRemove.Location = new Point(300, 10);

            this.btnBack = new Button();
            this.btnBack.Location = new Point(260, 750);
            this.btnBack.Width = 260;
            this.btnBack.Height = 50;
            this.btnBack.Text = "Back";
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
            btnPanel.Controls.Add(btnClear);
            btnPanel.Controls.Add(btnRemove);
            MakeRoundedButton(btnRemove, Color.FromArgb(220, 38, 38));
            MakeRoundedButton(btnClear, Color.Gray);
            MakeRoundedButton(btnBack, Color.AliceBlue);

            //Message Label
            this.lblMessage = new Label();
            this.lblMessage.AutoSize = true;
            this.lblMessage.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            this.lblMessage.Text = "Raw Materials In Stock:";

            // DATA GRID VIEW
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.dataGridView.Width = 1160;
            this.dataGridView.Height = 500;
            this.dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView.RowHeadersVisible = false;
            this.dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView.MultiSelect = false;
            this.dataGridView.ReadOnly = true; // optional but recommended


            // ===== ADD TO MAIN =====
            main.Controls.Add(title);

            main.Controls.Add(lbl1);
            main.Controls.Add(CreateRoundedBox(cmbMaterialName));

            main.Controls.Add(lbl2);
            main.Controls.Add(CreateRoundedBox(txtQuantity));

            main.Controls.Add(lbl3);
            main.Controls.Add(dateMaterial); // same as RawMaterial style

            main.Controls.Add(btnPanel);
            main.Controls.Add(lblMessage);
            main.Controls.Add(dataGridView);
            main.Controls.Add(btnBack);

            // FINAL
            this.Controls.Add(main);
            this.ResumeLayout(false);
        }
    }
}



