using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace FactoryManagementSystem
{
    partial class WorkersAddandView
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtName;

        private System.Windows.Forms.Label lblRole;
        private System.Windows.Forms.ComboBox cmbRole;

        private System.Windows.Forms.Label lblWage;
        private System.Windows.Forms.TextBox txtWage;

        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Label label1;

        private System.Windows.Forms.DataGridView dataGridView1;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();

            base.Dispose(disposing);
        }

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
                tb.Font = new Font("Segoe UI", 12F);
                tb.Margin = new Padding(0);
            }
            if (innerControl is ComboBox cb)
            {
                cb.FlatStyle = FlatStyle.Flat;
                cb.Font = new Font("Segoe UI", 12F);
                cb.Dock = DockStyle.Fill;
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
            btnRemove.MouseEnter += (s, e) => btnRemove.Invalidate();
            btnRemove.MouseLeave += (s, e) => btnRemove.Invalidate();
            btnRemove.MouseDown += (s, e) => btnRemove.Invalidate();
            btnRemove.MouseUp += (s, e) => btnRemove.Invalidate();
        }

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblRole = new System.Windows.Forms.Label();
            this.cmbRole = new System.Windows.Forms.ComboBox();
            this.lblWage = new System.Windows.Forms.Label();
            this.txtWage = new System.Windows.Forms.TextBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();

            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();

            FlowLayoutPanel main = new FlowLayoutPanel();
            main.Dock = DockStyle.Fill;
            main.FlowDirection = FlowDirection.TopDown;
            main.WrapContents = false;
            main.AutoScroll = true;
            main.Padding = new Padding(30, 20, 30, 20);

            lblTitle.Text = "Workers";
            lblTitle.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            lblTitle.AutoSize = true;
            lblTitle.Margin = new Padding(0, 0, 0, 20);

            lblName.Text = "Worker Name:";
            lblName.Font = new Font("Segoe UI", 14F);
            lblName.AutoSize = true;
            txtName.Width = 1000;
            txtName.Height = 30;

            lblRole.Text = "Job / Role:";
            lblRole.Font = new Font("Segoe UI", 14F);
            lblRole.AutoSize = true;
            cmbRole.Width = 1000;
            cmbRole.Height = 30;

            lblWage.Text = "Daily Wage (PKR):";
            lblWage.Font = new Font("Segoe UI", 14F);
            lblWage.AutoSize = true;
            txtWage.Width = 1000;
            txtWage.Height = 30;
            txtWage.Font = new Font("Segoe UI", 14F);
            txtWage.TextChanged += new System.EventHandler(this.TxtWage_TextChanged);

            Panel btnPanel = new Panel();
            btnPanel.Width = 800;
            btnPanel.Height = 80;
            btnPanel.Margin = new Padding(0, 20, 0, 20);

            btnAdd.Text = "Add Worker";
            btnAdd.Width = 260;
            btnAdd.Height = 50;
            btnAdd.FlatStyle = FlatStyle.Flat;
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.ForeColor = Color.White;
            btnAdd.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            RoundControl(btnAdd, 16);
            btnAdd.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                bool hover = btnAdd.ClientRectangle.Contains(btnAdd.PointToClient(Cursor.Position));
                bool down = (Control.MouseButtons == MouseButtons.Left) && hover;

                Color baseColor = Color.FromArgb(22, 163, 74);
                Color hoverColor = Color.FromArgb(34, 197, 94);
                Color downColor = Color.FromArgb(16, 135, 57);

                Color useColor = baseColor;

                if (down)
                    useColor = downColor;
                else if (hover)
                    useColor = hoverColor;

                using var brush = new LinearGradientBrush(
                    btnAdd.ClientRectangle,
                    useColor,
                    Color.FromArgb(16, 135, 57),
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

            btnRemove.Text = "Remove Worker";
            btnRemove.Width = 260;
            btnRemove.Height = 50;
            btnRemove.FlatStyle = FlatStyle.Flat;
            btnRemove.FlatAppearance.BorderSize = 0;
            btnRemove.ForeColor = Color.White;
            btnRemove.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            RoundControl(btnRemove, 16);
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
            btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            btnAdd.Location = new Point(200, 10);
            btnRemove.Location = new Point(500, 10);
            btnPanel.Controls.Add(btnAdd);
            btnPanel.Controls.Add(btnRemove);

            label1.Text = "Existing Workers:";
            label1.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            label1.AutoSize = true;


            this.dataGridView1.Width = 1160;
            this.dataGridView1.Height = 700;
            this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.RowHeadersVisible = false;

            main.Controls.Add(lblTitle);
            main.Controls.Add(lblName);
            main.Controls.Add(CreateRoundedBox(txtName));
            main.Controls.Add(lblRole);
            main.Controls.Add(CreateRoundedBox(cmbRole));
            main.Controls.Add(lblWage);
            // Wage textbox displayed directly (no outer rounded box)
            main.Controls.Add(CreateRoundedBox(txtWage));
            main.Controls.Add(btnPanel);
            main.Controls.Add(label1);
            main.Controls.Add(this.dataGridView1);

            this.Controls.Add(main);
            this.Size = new System.Drawing.Size(800, 700);

            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
