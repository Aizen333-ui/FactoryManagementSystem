using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace FactoryManagementSystem
{
    partial class Payments
    {
        // ===================== COMPONENT DECLARATIONS =====================

        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtAmount;

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbReason;

        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker datePaid;

        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnBack;
        // ===================== ROUNDED INPUT CONTAINER =====================
        // Creates styled container (card-like UI) around inputs
        private Panel CreateRoundedBox(Control innerControl, int height = 55)
        {
            Panel container = new Panel();
            container.Height = (innerControl.Height > 0) ? innerControl.Height + 20 : height;
            container.Width = (innerControl.Width > 0) ? innerControl.Width + 24 : 600;
            container.BackColor = Color.White;
            container.Padding = new Padding(12, 10, 14, 12);
            container.Margin = new Padding(0, 0, 0, 15);
            container.Size = new Size(container.Width, container.Height + 2);
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
            // Draw custom rounded border

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
            btnDelete.MouseEnter += (s, e) => btnDelete.Invalidate();
            btnDelete.MouseLeave += (s, e) => btnDelete.Invalidate();
            btnDelete.MouseDown += (s, e) => btnDelete.Invalidate();
            btnDelete.MouseUp += (s, e) => btnDelete.Invalidate();
            btnBack.MouseEnter += (s, e) => btnBack.Invalidate();
            btnBack.MouseLeave += (s, e) => btnBack.Invalidate();
            btnBack.MouseDown += (s, e) => btnBack.Invalidate();
            btnBack.MouseUp += (s, e) => btnBack.Invalidate();

        }
        // ===================== INITIALIZE UI =====================

        private void InitializeComponent()
        {
            this.labelTitle = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtAmount = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbReason = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.datePaid = new System.Windows.Forms.DateTimePicker();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btnBack = new System.Windows.Forms.Button();

            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // ===================== MAIN LAYOUT =====================

            FlowLayoutPanel main = new FlowLayoutPanel();
            main.Dock = DockStyle.Fill;
            main.FlowDirection = FlowDirection.TopDown;
            main.WrapContents = false;
            main.AutoScroll = true;
            main.Padding = new Padding(30, 20, 30, 20);

            // Title
            labelTitle.Text = "Payments";
            labelTitle.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            labelTitle.AutoSize = true;
            labelTitle.Margin = new Padding(0, 0, 0, 20);

            // Amount
            label2.Text = "Amount:";
            label2.Font = new Font("Segoe UI", 14F);
            label2.AutoSize = true;
            txtAmount.Width = 1000;
            txtAmount.Height = 30;
            txtAmount.TextChanged += new System.EventHandler(this.TxtAmount_TextChanged);

            // Reason
            label3.Text = "Reason:";
            label3.Font = new Font("Segoe UI", 14F);
            label3.AutoSize = true;
            cmbReason.Width = 1000;
            cmbReason.Height = 30;
            cmbReason.DropDownStyle = ComboBoxStyle.DropDownList;

            // Date
            label4.Text = "Date Paid:";
            label4.Font = new Font("Segoe UI", 14F);
            label4.AutoSize = true;
            datePaid.Width = 1000;
            datePaid.Height = 35;
            datePaid.Font = new Font("Segoe UI", 14F);
            datePaid.Format = DateTimePickerFormat.Custom;
            datePaid.CustomFormat = "dd/MM/yyyy";

            // Buttons
            Panel btnPanel = new Panel();
            btnPanel.Width = 600;
            btnPanel.Height = 80;
            btnPanel.AutoSize = true;
            btnPanel.Margin = new Padding(0, 20, 0, 20);

            btnAdd.Text = "Add Payment";
            btnAdd.Width = 260;
            btnAdd.Height = 50;
            btnAdd.FlatStyle = FlatStyle.Flat;
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.ForeColor = Color.White;
            btnAdd.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            RoundControl(btnAdd, 16);
            btnAdd.Location = new Point(300, 10);

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
            btnAdd.Click += new System.EventHandler(this.btnAdd_Click);

            btnDelete.Text = "Delete Payment";
            btnDelete.Width = 260;
            btnDelete.Height = 50;
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.ForeColor = Color.White;
            btnDelete.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            RoundControl(btnDelete, 16);
            btnDelete.Location = new Point(600, 10);

            btnDelete.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                bool hover = btnDelete.ClientRectangle.Contains(btnDelete.PointToClient(Cursor.Position));
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

                e.Graphics.FillRectangle(brush, btnDelete.ClientRectangle);

                TextRenderer.DrawText(
                    e.Graphics,
                    btnDelete.Text,
                    btnDelete.Font,
                    btnDelete.ClientRectangle,
                    btnDelete.ForeColor,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            };
            btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            Button btnBack = this.btnBack = new Button();
            btnBack.Text = "Back";
            btnBack.Width = 260;
            btnBack.Height = 50;
            btnBack.FlatStyle = FlatStyle.Flat;
            btnBack.FlatAppearance.BorderSize = 0;
            btnBack.ForeColor = Color.White;
            btnBack.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            btnBack.Location = new Point(260, 850);
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
            btnBack.Click += new System.EventHandler(this.btnBack_Click);
            RoundControl(btnBack, 16);


            btnPanel.Controls.Add(btnAdd);
            btnPanel.Controls.Add(btnDelete);

            // Label for DataGrid
            label5.Text = "Previous Payments:";
            label5.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            label5.AutoSize = true;
            label5.Margin = new Padding(0, 20, 0, 10);

            // DataGrid
            this.dataGridView1.Width = 1160;
            this.dataGridView1.Height = 500;
            this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.ReadOnly = true; // optional but recommended

            // Assemble
            main.Controls.Add(labelTitle);
            main.Controls.Add(label2);
            main.Controls.Add(CreateRoundedBox(txtAmount));
            main.Controls.Add(label3);
            main.Controls.Add(CreateRoundedBox(cmbReason));
            main.Controls.Add(label4);
            main.Controls.Add(datePaid);
            main.Controls.Add(btnPanel);
            main.Controls.Add(label5);
            main.Controls.Add(this.dataGridView1);

            this.Controls.Add(main);
            this.Size = new System.Drawing.Size(800, 700);
            main.Controls.Add(btnBack);


            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
