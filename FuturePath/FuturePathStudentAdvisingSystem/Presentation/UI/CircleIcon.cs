using System.Drawing.Drawing2D;

namespace FuturePathStudentAdvisingSystem.UI
{
    /// <summary>A filled circle with an icon glyph in the middle (used on the side panels).</summary>
    public class CircleIcon : Panel
    {
        private readonly Color fill;
        private readonly string glyph;
        private readonly float glyphSize;

        public CircleIcon(Color fill, string glyph, int diameter, float glyphSize)
        {
            this.fill = fill;
            this.glyph = glyph;
            this.glyphSize = glyphSize;
            DoubleBuffered = true;
            Size = new Size(diameter, diameter);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using (SolidBrush brush = new SolidBrush(fill))
                e.Graphics.FillEllipse(brush, 0, 0, Width - 1, Height - 1);

            using (Font font = Theme.Icons(glyphSize))
            using (StringFormat format = new StringFormat())
            {
                format.Alignment = StringAlignment.Center;
                format.LineAlignment = StringAlignment.Center;
                e.Graphics.DrawString(glyph, font, Brushes.White,
                    new RectangleF(0, 0, Width, Height), format);
            }
        }
    }
}
