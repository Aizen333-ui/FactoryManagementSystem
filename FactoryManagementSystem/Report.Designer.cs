using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace FactoryDashboard.Pages
{
    partial class Report
    {
        private System.ComponentModel.IContainer components = null;

        private Panel panelHeader;
        private Label lblTitle;
        private Button btnReports;

        private Panel mainPanel;
       
        private DataGridView dataGridReport;
        private Button btnGenerate;
        private Button btnSendReport;

        private DateTimePicker dateFrom;
        private DateTimePicker dateTo;
        private Label lblFrom;
        private Label lblTo;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();

            base.Dispose(disposing);
        }
        private bool isHover = false;
        private bool isDown = false;

        private void ApplyModernButton(Button btn, Color baseColor, Color hoverColor, Color pressedColor)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.ForeColor = Color.White;
            btn.BackColor = Color.Transparent;
            btn.Cursor = Cursors.Hand;

            btn.MouseEnter += (s, e) => { isHover = true; btn.Invalidate(); };
            btn.MouseLeave += (s, e) => { isHover = false; isDown = false; btn.Invalidate(); };
            btn.MouseDown += (s, e) => { isDown = true; btn.Invalidate(); };
            btn.MouseUp += (s, e) => { isDown = false; btn.Invalidate(); };

            btn.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                Rectangle rect = btn.ClientRectangle;
                Color fill = baseColor;

                if (isDown)
                    fill = pressedColor;
                else if (isHover)
                    fill = hoverColor;

                int radius = 15;

                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
                    path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
                    path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
                    path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);
                    path.CloseFigure();

                    using (SolidBrush brush = new SolidBrush(fill))
                        e.Graphics.FillPath(brush, path);
                }

                TextRenderer.DrawText(
                    e.Graphics,
                    btn.Text,
                    btn.Font,
                    rect,
                    Color.White,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
                );
            };
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
            this.panelHeader = new Panel();
            this.lblTitle = new Label();
            this.btnReports = new Button();

            this.mainPanel = new Panel();
            this.dataGridReport = new DataGridView();
            this.btnGenerate = new Button();
            this.btnSendReport = new Button();
            this.dateFrom = new DateTimePicker();
            this.dateTo = new DateTimePicker();
            this.lblFrom = new Label();
            this.lblTo = new Label();

            this.SuspendLayout();

            // ================= HEADER =================
            
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Height = 60;

            lblTitle.Text = "Factory Report to Owner";
            lblTitle.ForeColor = Color.Black;
            lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitle.Location = new Point(20, 12);
            lblTitle.AutoSize = true;

            // FIXED: btnReports was missing Location


            panelHeader.Controls.Add(lblTitle);


            // ================= MAIN PANEL =================
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.BackColor = Color.White;

            

            // FROM
            lblFrom.Text = "From:";
            lblFrom.Font = new Font("Segoe UI", 14F);
            lblFrom.Location = new Point(20, 60);
            lblFrom.AutoSize = true;

            dateFrom.Location = new Point(110, 60);
            dateFrom.Size = new Size(180, 30);
            dateFrom.Font = new Font("Segoe UI", 11F);
            dateFrom.Format = DateTimePickerFormat.Custom;
            dateFrom.CustomFormat = "dd/MM/yyyy";

            // TO
            lblTo.Text = "To:";
            lblTo.Font = new Font("Segoe UI", 14F);
            lblTo.Location = new Point(450, 60);
            lblTo.AutoSize = true;

            dateTo.Location = new Point(510, 60);
            dateTo.Size = new Size(180, 30);
            dateTo.Font = new Font("Segoe UI", 11F);
            dateTo.Format = DateTimePickerFormat.Custom;
            dateTo.CustomFormat = "dd/MM/yyyy";
            // GENERATE BUTTON
            btnGenerate.Text = "Generate";
            btnGenerate.BackColor = Color.DarkBlue;
            btnGenerate.ForeColor = Color.White;
            btnGenerate.FlatStyle = FlatStyle.Flat;
            btnGenerate.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            btnGenerate.Size = new Size(250, 80);
            btnGenerate.Location = new Point(150, 120);

            // SEND BUTTON
            btnSendReport.Text = "Send to Owner";
            btnSendReport.BackColor = Color.Green;
            btnSendReport.ForeColor = Color.White;
            btnSendReport.FlatStyle = FlatStyle.Flat;
            btnSendReport.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            btnSendReport.Size = new Size(250, 80);
            btnSendReport.Location = new Point(500, 120);

            // GRID
            dataGridReport.Location = new Point(50, 230);
            dataGridReport.Size = new Size(1160, 1000);
            dataGridReport.BackgroundColor = Color.White;
            dataGridReport.BorderStyle = BorderStyle.Fixed3D;
            dataGridReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // ADD TO MAIN
            mainPanel.Controls.Add(lblFrom);
            mainPanel.Controls.Add(dateFrom);
            mainPanel.Controls.Add(lblTo);
            mainPanel.Controls.Add(dateTo);
            mainPanel.Controls.Add(btnGenerate);
            mainPanel.Controls.Add(btnSendReport);
            mainPanel.Controls.Add(dataGridReport);
            ApplyModernButton(btnGenerate,
            Color.FromArgb(37, 99, 235),      // base blue
            Color.FromArgb(59, 130, 246),     // hover
            Color.FromArgb(29, 78, 216));     // pressed

            ApplyModernButton(btnSendReport,
                Color.FromArgb(22, 163, 74),      // base green
                Color.FromArgb(34, 197, 94),      // hover
                Color.FromArgb(21, 128, 61));     // pressed
            // ================= FORM =================
            this.ClientSize = new Size(1050, 600);
            this.Controls.Add(mainPanel);
            this.Controls.Add(panelHeader);
            this.Text = "Report to Owner";

            this.ResumeLayout(false);
        }
    }
}