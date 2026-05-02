using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace FactoryDashboard.Pages
{
    partial class RawMaterialUsage
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblTitle;
        private Label lblMaterialName;
        private Label lblQuantity;
        private Label lblDate;

        private ComboBox cmbMaterialName; // changed from TextBox
        private TextBox txtQuantity;
        private DateTimePicker dateMaterial;

        private Button btnSave;
        private Button btnClear;
        private Button btnRemove;
        private Label lblMessage;
        private System.Windows.Forms.DataGridView dataGridView;
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();

            base.Dispose(disposing);
        }

        private void MakeRoundedButton(Button btn, Color color)
        {
           
            this.btnSave.BackColor = Color.FromArgb(94, 60, 255);
            this.btnSave.FlatStyle = FlatStyle.Flat;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.ForeColor = Color.White;
            this.btnSave.Font = new Font("Segoe UI", 13F, FontStyle.Bold);

            this.btnSave.FlatAppearance.MouseOverBackColor = Color.FromArgb(114, 80, 255);
            this.btnSave.FlatAppearance.MouseDownBackColor = Color.FromArgb(74, 40, 235);
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
            btnPanel.Width = 1000;
            btnPanel.Height = 80;
            btnPanel.AutoSize = true;
            btnPanel.Margin = new Padding(0, 20, 0, 20);

            // SAVE BUTTON (purple like RawMaterial Add)
            this.btnSave = new Button();
            this.btnSave.Text = "Save Entry";
            this.btnSave.Width = 250;
            this.btnSave.Height = 80;
            this.btnSave.FlatStyle = FlatStyle.Flat;
            this.btnSave.ForeColor = Color.White;
            this.btnSave.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            this.btnSave.Location = new Point(100, 10);


            // CLEAR BUTTON (gray style)
            this.btnClear = new Button();
            this.btnClear.Text = "Clear";
            this.btnClear.Width = 250;
            this.btnClear.Height = 80;
            this.btnClear.FlatStyle = FlatStyle.Flat;
            this.btnClear.ForeColor = Color.White;
            this.btnClear.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            this.btnClear.Location = new Point(400, 10);


            // REMOVE BUTTON (red like RawMaterial)
            this.btnRemove = new Button();
            this.btnRemove.Text = "Remove Entry";
            this.btnRemove.Width = 250;
            this.btnRemove.Height = 80;
            this.btnRemove.FlatStyle = FlatStyle.Flat;
            this.btnRemove.ForeColor = Color.White;
            this.btnRemove.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            this.btnRemove.Location = new Point(700, 10);


            btnPanel.Controls.Add(btnSave);
            btnPanel.Controls.Add(btnClear);
            btnPanel.Controls.Add(btnRemove);
            MakeRoundedButton(btnSave, Color.FromArgb(94, 60, 255));
            MakeRoundedButton(btnRemove, Color.FromArgb(220, 38, 38));
            MakeRoundedButton(btnClear, Color.Gray);

            //Message Label
            this.lblMessage = new Label();
            this.lblMessage.AutoSize = true;
            this.lblMessage.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            this.lblMessage.Text = "Previously Used Raw Materials:";

            // DATA GRID VIEW
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.dataGridView.Width = 1160;
            this.dataGridView.Height = 700;
            this.dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView.ReadOnly = true;
            

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

            // FINAL
            this.Controls.Add(main);
            this.ResumeLayout(false);
        }
    }
}



