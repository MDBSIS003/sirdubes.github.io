using System.Drawing.Drawing2D;

namespace FuturePathStudentAdvisingSystem.UI
{
    /// <summary>
    /// A rounded input box with an icon on the left. Hosts a TextBox or a ComboBox.
    /// The border turns navy while the control inside has focus.
    /// </summary>
    public class FieldBox : Panel
    {
        private readonly Label lblIcon = new Label();
        private bool focused;

        public Control Inner { get; }
        public TextBox TextBox => (TextBox)Inner;
        public ComboBox ComboBox => (ComboBox)Inner;

        private FieldBox(string glyph, Control inner, int width)
        {
            DoubleBuffered = true;
            Size = new Size(width, 40);
            Inner = inner;

            lblIcon.Text = glyph;
            lblIcon.Font = Theme.Icons(11F);
            lblIcon.ForeColor = Theme.Muted;
            lblIcon.TextAlign = ContentAlignment.MiddleCenter;
            lblIcon.SetBounds(2, 1, 34, 38);

            inner.Font = Theme.Body(10.5F);
            Controls.Add(lblIcon);
            Controls.Add(inner);

            inner.Enter += (s, e) => SetFocused(true);
            inner.Leave += (s, e) => SetFocused(false);

            PositionInner();
        }

        public static FieldBox ForText(string glyph, int width, bool password = false, int maxLength = 0)
        {
            TextBox box = new TextBox { BorderStyle = BorderStyle.None };
            if (password) box.UseSystemPasswordChar = true;
            if (maxLength > 0) box.MaxLength = maxLength;
            return new FieldBox(glyph, box, width);
        }

        public static FieldBox ForCombo(string glyph, int width, IEnumerable<string> items)
        {
            ComboBox combo = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.Flat
            };
            foreach (string item in items)
                combo.Items.Add(item);
            return new FieldBox(glyph, combo, width);
        }

        private void SetFocused(bool value)
        {
            focused = value;
            Invalidate();
        }

        private void PositionInner()
        {
            Inner.SetBounds(38, (Height - Inner.Height) / 2, Width - 38 - 10, Inner.Height);
        }

        protected override void OnResize(EventArgs eventargs)
        {
            base.OnResize(eventargs);
            if (Inner != null) PositionInner();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle r = new Rectangle(1, 1, Width - 3, Height - 3);
            using (GraphicsPath path = Theme.RoundedRect(r, 6))
            using (Pen pen = new Pen(focused ? Theme.Navy : Theme.Border, focused ? 2F : 1F))
                e.Graphics.DrawPath(pen, path);
        }
    }
}
