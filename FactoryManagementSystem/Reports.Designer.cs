using System.Drawing.Drawing2D;

namespace FactoryManagementSystem
{
    partial class OwnerReportsPage
    {
        private System.ComponentModel.IContainer components = null;

        private DateTimePicker dtFrom;
        private DateTimePicker dtTo;
        private Button btnViewReport;
        private Label lblFrom;
        private Label lblTo;
        private RichTextBox txtReport;
        private Label lblTitle;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private Panel CreateRoundedBox(Control innerControl, int height = 55)
        {
            Panel container = new Panel();
            container.Height = height;
            container.Width = 1100;   // force proper width for reports box
            container.BackColor = Color.White;
            container.Padding = new Padding(12, 10, 14, 12);
            container.Margin = new Padding(0, 0, 0, 15);
            container.Size = new Size(container.Width, container.Height + 2);
            innerControl.Dock = DockStyle.Fill;
            innerControl.BackColor = Color.White;
            innerControl.Margin = new Padding(0);
            
            if (innerControl is RichTextBox rt)
            {
                rt.Font = new Font("Consolas", 10F);
                rt.BorderStyle = BorderStyle.None;
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
            btnViewReport.MouseEnter += (s, e) => btnViewReport.Invalidate();
            btnViewReport.MouseLeave += (s, e) => btnViewReport.Invalidate();
            btnViewReport.MouseDown += (s, e) => btnViewReport.Invalidate();
            btnViewReport.MouseUp += (s, e) => btnViewReport.Invalidate();
        }

        private void InitializeComponent()
        {
            this.lblTitle = new Label();
            this.lblFrom = new Label();
            this.lblTo = new Label();
            this.dtFrom = new DateTimePicker();
            this.dtTo = new DateTimePicker();
            this.btnViewReport = new Button();
            this.txtReport = new RichTextBox();

            this.SuspendLayout();

            FlowLayoutPanel main = new FlowLayoutPanel();
            main.Dock = DockStyle.Fill;
            main.FlowDirection = FlowDirection.TopDown;
            main.WrapContents = false;
            main.AutoScroll = true;
            main.Padding = new Padding(30, 20, 30, 20);

            lblTitle.Text = "Reports";
            lblTitle.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            lblTitle.AutoSize = true;
            lblTitle.Margin = new Padding(0, 0, 0, 20);

            lblFrom.Text = "From Date:";
            lblFrom.Font = new Font("Segoe UI", 14F);
            lblFrom.AutoSize = true;
            lblTo.Text = "To Date:";
            lblTo.Font = new Font("Segoe UI", 14F);
            lblTo.AutoSize = true;

            dtFrom.Width = 1000;
            dtFrom.Height = 35;
            dtFrom.Font = new Font("Segoe UI", 14F);
            dtFrom.Format = DateTimePickerFormat.Custom;
            dtFrom.CustomFormat = "dd/MM/yyyy";

            dtTo.Width = 1000;
            dtTo.Height = 35;
            dtTo.Font = new Font("Segoe UI", 14F);
            dtTo.Format = DateTimePickerFormat.Custom;
            dtTo.CustomFormat = "dd/MM/yyyy";

            btnViewReport.Text = "View Report";
            btnViewReport.Width = 200;
            btnViewReport.Height = 45;
            btnViewReport.FlatStyle = FlatStyle.Flat;
            btnViewReport.FlatAppearance.BorderSize = 0;
            btnViewReport.ForeColor = Color.White;
            btnViewReport.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            RoundControl(btnViewReport, 16);
            btnViewReport.Margin = new Padding(0, 20, 0, 20);
            btnViewReport.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                bool hover = btnViewReport.ClientRectangle.Contains(btnViewReport.PointToClient(Cursor.Position));
                bool down = (Control.MouseButtons == MouseButtons.Left) && hover;

                Color baseColor = Color.FromArgb(0, 123, 255);
                Color hoverColor = Color.FromArgb(0, 150, 255);
                Color downColor = Color.FromArgb(0, 90, 200);

                Color useColor = baseColor;

                if (down)
                    useColor = downColor;
                else if (hover)
                    useColor = hoverColor;

                using var brush = new LinearGradientBrush(
                    btnViewReport.ClientRectangle,
                    useColor,
                    Color.FromArgb(0, 90, 200),
                    45F);

                e.Graphics.FillRectangle(brush, btnViewReport.ClientRectangle);

                TextRenderer.DrawText(
                    e.Graphics,
                    btnViewReport.Text,
                    btnViewReport.Font,
                    btnViewReport.ClientRectangle,
                    btnViewReport.ForeColor,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            };
            btnViewReport.Click += new EventHandler(this.btnViewReport_Click);
            Panel reportBox = CreateRoundedBox(txtReport, 800);
            reportBox.Margin = new Padding(0, 20, 0, 20);
            txtReport.Width = 1160;
            txtReport.Height = 500;
            txtReport.Font = new Font("Consolas", 11F);
            txtReport.ReadOnly = true;

            main.Controls.Add(lblTitle);
            main.Controls.Add(lblFrom);
            // Date pickers displayed directly (no outer rounded boxes)
            main.Controls.Add(dtFrom);
            main.Controls.Add(lblTo);
            main.Controls.Add(dtTo);
            main.Controls.Add(btnViewReport);
            main.Controls.Add(reportBox);

            this.Controls.Add(main);
            this.Size = new Size(800, 700);
            this.ResumeLayout(false);
        }
    }
}