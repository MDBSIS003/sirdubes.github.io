namespace FuturePathStudentAdvisingSystem.UI
{
    /// <summary>
    /// Base layout used by the welcome, login and sign-up screens:
    /// a coloured side panel (icon, title, short text) and a white working area.
    /// Derived forms add their controls to Content (and optionally Side).
    /// </summary>
    public class BrandedForm : Form
    {
        protected Panel Side { get; }
        protected Panel Content { get; }

        public const int SideWidth = 230;
        public const int FormWidth = 800;
        public const int FormHeight = 540;

        public BrandedForm()
    : this("Future Path", Theme.Navy, Theme.Teal, Theme.GlyphLogo,
           "Future Path", "Find the degree that fits you.")
        {
        }

        public BrandedForm(string windowTitle, Color sideColor, Color circleColor,
                           string glyph, string sideTitle, string sideText)
        {
            SuspendLayout();

            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(FormWidth, FormHeight);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.White;
            Font = Theme.Body(10F);
            Text = windowTitle;

            Content = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };
            Side = new Panel { Dock = DockStyle.Left, Width = SideWidth, BackColor = sideColor };

            CircleIcon circle = new CircleIcon(circleColor, glyph, 88, 34F);
            circle.Location = new Point((SideWidth - 88) / 2, 64);
            Side.Controls.Add(circle);

            Label title = new Label
            {
                Text = sideTitle,
                AutoSize = false,
                Location = new Point(12, 168),
                Size = new Size(SideWidth - 24, 36),
                TextAlign = ContentAlignment.MiddleCenter,
                Font = Theme.Body(16F, FontStyle.Bold),
                ForeColor = Color.White
            };
            Side.Controls.Add(title);

            Label text = new Label
            {
                Text = sideText,
                AutoSize = false,
                Location = new Point(20, 210),
                Size = new Size(SideWidth - 40, 70),
                TextAlign = ContentAlignment.TopCenter,
                Font = Theme.Body(10F),
                ForeColor = Theme.SideText
            };
            Side.Controls.Add(text);

            // Fill-docked control first, edge-docked control second.
            Controls.Add(Content);
            Controls.Add(Side);

            ResumeLayout(false);
        }
    }
}
