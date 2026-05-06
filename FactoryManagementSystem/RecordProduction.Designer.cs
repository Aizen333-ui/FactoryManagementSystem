using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace FactoryDashBoard.Pages
{
    partial class RecordProduction
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblTitle;
        private Label lblProductName;
        private Label lblQuantity;
        private Label lblUnit;
        private Label lblDate;

        private ComboBox cmbProductName;
        private TextBox txtQuantity;
        private ComboBox cmbUnit;
        private DateTimePicker dateProduction;

        private Button btnSave;
        private Button btnClear;
        private Label lblMessage;
        private System.Windows.Forms.DataGridView dataGridView1;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();

            base.Dispose(disposing);
        }

        // ===================== ROUND BUTTON =====================
        private void MakeRoundedButton(Button btn, Color color)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;

            btn.BackColor = color;
            btn.ForeColor = Color.White;
            btn.Font = new Font("Segoe UI", 13F, FontStyle.Bold);

            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(
                Math.Min(color.R + 20, 255),
                Math.Min(color.G + 20, 255),
                Math.Min(color.B + 20, 255)
            );

            btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(
                Math.Max(color.R - 20, 0),
                Math.Max(color.G - 20, 0),
                Math.Max(color.B - 20, 0)
            );

            ApplyRoundedRegion(btn, 18);
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

        // ===================== ROUNDED INPUT BOX =====================
        private Panel CreateRoundedBox(Control inner, int height = 55)
        {
            Panel container = new Panel();
            container.Height = inner.Height > 0 ? inner.Height + 28 : height;
            container.Width = inner.Width > 0 ? inner.Width + 24 : 600;

            container.BackColor = Color.White;
            container.Padding = new Padding(12, 12, 14, 14);
            container.Margin = new Padding(0, 0, 0, 25);

            inner.Dock = DockStyle.Fill;
            inner.Location = new Point(0, 2);
            inner.Margin = new Padding(0);
            inner.BackColor = Color.White;
            inner.Height = 30;

            if (inner is TextBox tb)
            {
                tb.BorderStyle = BorderStyle.None;
                tb.Font = new Font("Segoe UI", 13F);
                tb.Multiline = false;
            }

            if (inner is ComboBox cb)
            {
                cb.FlatStyle = FlatStyle.Flat;
                cb.Font = new Font("Segoe UI", 13F);
                cb.DropDownStyle = ComboBoxStyle.DropDownList;
            }

            if (inner is DateTimePicker dt)
            {
                dt.Font = new Font("Segoe UI", 12F);
                dt.Format = DateTimePickerFormat.Custom;
                dt.CustomFormat = "dd/MM/yyyy";
            }

            container.Controls.Add(inner);

            container.Paint += (s, e) =>
            {
                int radius = 12;
                Rectangle rect = new Rectangle(0, 0, container.Width - 1, container.Height - 1);

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

        // ===================== UI =====================
        private void InitializeComponent()
        {
            this.SuspendLayout();

            FlowLayoutPanel main = new FlowLayoutPanel();
            main.Dock = DockStyle.Fill;
            main.FlowDirection = FlowDirection.TopDown;
            main.WrapContents = false;
            main.AutoScroll = true;
            main.Padding = new Padding(50, 30, 50, 30);

            // ================= TITLE =================
            Label title = new Label();
            title.Text = "Record Production";
            title.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            title.AutoSize = true;
            title.Margin = new Padding(0, 0, 0, 20);

            main.Controls.Add(title);

            // ================= PRODUCT NAME =================
            lblProductName = new Label();
            lblProductName.Text = "Product Name:";
            lblProductName.Font = new Font("Segoe UI", 14F);
            lblProductName.AutoSize = true;
            lblProductName.Margin = new Padding(0, 15, 0, 8);

            cmbProductName = new ComboBox();
            cmbProductName.Width = 1000;
            cmbProductName.Font = new Font("Segoe UI", 13F);
            cmbProductName.DropDownStyle = ComboBoxStyle.DropDownList;

            main.Controls.Add(lblProductName);
            main.Controls.Add(CreateRoundedBox(cmbProductName));

            // ================= QUANTITY =================
            lblQuantity = new Label();
            lblQuantity.Text = "Quantity Produced:";
            lblQuantity.Font = new Font("Segoe UI", 14F);
            lblQuantity.AutoSize = true;
            lblQuantity.Margin = new Padding(0, 15, 0, 8);

            txtQuantity = new TextBox();
            txtQuantity.Width = 1000;
            txtQuantity.Font = new Font("Segoe UI", 13F);

            main.Controls.Add(lblQuantity);
            main.Controls.Add(CreateRoundedBox(txtQuantity));

            // ================= UNIT =================
            lblUnit = new Label();
            lblUnit.Text = "Unit:";
            lblUnit.Font = new Font("Segoe UI", 14F);
            lblUnit.AutoSize = true;
            lblUnit.Margin = new Padding(0, 15, 0, 8);

            cmbUnit = new ComboBox();
            cmbUnit.Width = 1000;
            cmbUnit.Font = new Font("Segoe UI", 13F);
            cmbUnit.DropDownStyle = ComboBoxStyle.DropDownList;

            main.Controls.Add(lblUnit);
            main.Controls.Add(CreateRoundedBox(cmbUnit));

            // ================= DATE =================
            lblDate = new Label();
            lblDate.Text = "Production Date:";
            lblDate.Font = new Font("Segoe UI", 14F);
            lblDate.AutoSize = true;
            lblDate.Margin = new Padding(0, 15, 0, 8);

            dateProduction = new DateTimePicker();
            dateProduction.Width = 1000;
            dateProduction.Font = new Font("Segoe UI", 12F);
            dateProduction.Format = DateTimePickerFormat.Custom;
            dateProduction.CustomFormat = "dd/MM/yyyy";

            main.Controls.Add(lblDate);
            main.Controls.Add(dateProduction);

            // ================= BUTTONS =================
            Panel btnPanel = new Panel();
            btnPanel.Width = 600;
            btnPanel.Height = 80;
            btnPanel.AutoSize = true;
            btnPanel.Margin = new Padding(0, 20, 0, 20);

            btnSave = new Button();
            btnSave.Text = "Save Entry";
            btnSave.Width = 260;
            btnSave.Height = 50;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.ForeColor = Color.White;
            btnSave.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnSave.Location = new Point(300, 10);
            
            btnClear = new Button();
            btnClear.Text = "Clear";
            btnClear.Width = 260;
            btnClear.Height = 50;
            btnClear.FlatStyle = FlatStyle.Flat;
            btnClear.ForeColor = Color.White;
            btnClear.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnClear.Location = new Point(600, 10);
            btnPanel.Controls.Add(btnSave);
            btnPanel.Controls.Add(btnClear);

           

            MakeRoundedButton(btnSave, Color.FromArgb(94, 60, 255));
            MakeRoundedButton(btnClear, Color.Gray);
            lblMessage = new Label();
            lblMessage.Text ="Previously Produced Materials:";
            lblMessage.Font = new Font("Segoe UI", 18F);
            lblMessage.AutoSize = true; 
            lblMessage.Margin = new Padding(0, 30, 0, 15);

            // ===== DATAGRID =====
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.dataGridView1.Width = 1160;
            this.dataGridView1.Height = 500;
            this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.RowHeadersVisible = false;

            main.Controls.Add(btnPanel);
            main.Controls.Add(lblMessage);
            main.Controls.Add(this.dataGridView1);

            // FINAL
            this.Controls.Add(main);
            this.ResumeLayout(false);
        }
    }
}