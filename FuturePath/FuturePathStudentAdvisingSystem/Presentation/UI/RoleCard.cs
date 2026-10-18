using System.Drawing.Drawing2D;

namespace FuturePathStudentAdvisingSystem.UI
{
    /// <summary>
    /// A clickable card (icon, title, description) with a hover highlight.
    /// Handle the normal Click event to react to it.
    /// </summary>
    public class RoleCard : Panel
    {
        private readonly Panel pnlIcon = new Panel();
        private readonly Label lblIcon = new Label();
        private readonly Label lblTitle = new Label();
        private readonly Label lblDescription = new Label();
        private readonly Color accent;
        private bool hover;

        public RoleCard(string glyph, string title, string description,
                        Color iconBack, Color iconFore, Color accent)
        {
            this.accent = accent;
            DoubleBuffered = true;
            ResizeRedraw = true;
            Size = new Size(236, 140);
            Cursor = Cursors.Hand;

            pnlIcon.Location = new Point(16, 16);
            pnlIcon.Size = new Size(44, 44);
            pnlIcon.BackColor = iconBack;
            using (GraphicsPath p = Theme.RoundedRect(new Rectangle(0, 0, 44, 44), 10))
                pnlIcon.Region = new Region(p);

            lblIcon.Dock = DockStyle.Fill;
            lblIcon.TextAlign = ContentAlignment.MiddleCenter;
            lblIcon.Font = Theme.Icons(18F);
            lblIcon.ForeColor = iconFore;
            lblIcon.Text = glyph;
            pnlIcon.Controls.Add(lblIcon);

            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(16, 70);
            lblTitle.Font = Theme.Body(11.5F, FontStyle.Bold);
            lblTitle.ForeColor = Theme.Text;
            lblTitle.BackColor = Color.Transparent;
            lblTitle.Text = title;

            lblDescription.Location = new Point(16, 96);
            lblDescription.Size = new Size(204, 38);
            lblDescription.Font = Theme.Body(9F);
            lblDescription.ForeColor = Theme.Muted;
            lblDescription.BackColor = Color.Transparent;
            lblDescription.Text = description;

            Controls.Add(pnlIcon);
            Controls.Add(lblTitle);
            Controls.Add(lblDescription);

            // Child controls swallow mouse events, so forward them to the card.
            foreach (Control child in new Control[] { pnlIcon, lblIcon, lblTitle, lblDescription })
            {
                child.Cursor = Cursors.Hand;
                child.Click += (s, e) => OnClick(e);
                child.MouseEnter += (s, e) => UpdateHover();
                child.MouseLeave += (s, e) => UpdateHover();
            }
            MouseEnter += (s, e) => UpdateHover();
            MouseLeave += (s, e) => UpdateHover();
        }

        private void UpdateHover()
        {
            bool inside = ClientRectangle.Contains(PointToClient(Cursor.Position));
            if (inside != hover)
            {
                hover = inside;
                Invalidate(true);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle r = new Rectangle(1, 1, Width - 3, Height - 3);
            using (GraphicsPath path = Theme.RoundedRect(r, 10))
            using (SolidBrush fill = new SolidBrush(hover ? Theme.Lavender : Color.White))
            using (Pen pen = new Pen(hover ? accent : Theme.Border, hover ? 2F : 1F))
            {
                e.Graphics.FillPath(fill, path);
                e.Graphics.DrawPath(pen, path);
            }
        }
    }
}
