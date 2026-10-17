using System.Drawing.Drawing2D;
using FuturePathStudentAdvisingSystem.Security;

namespace FuturePathStudentAdvisingSystem.UI
{
    /// <summary>Colours, fonts, icons and small factory helpers shared by every screen.</summary>
    public static class Theme
    {
        // ---- Colours ----
        public static readonly Color Navy = ColorTranslator.FromHtml("#26215C");
        public static readonly Color Teal = ColorTranslator.FromHtml("#1D9E75");
        public static readonly Color TealDark = ColorTranslator.FromHtml("#0F6E56");
        public static readonly Color Blue = ColorTranslator.FromHtml("#185FA5");
        public static readonly Color BlueLight = ColorTranslator.FromHtml("#E6F1FB");
        public static readonly Color Amber = ColorTranslator.FromHtml("#854F0B");
        public static readonly Color AmberLight = ColorTranslator.FromHtml("#FAEEDA");
        public static readonly Color TealLight = ColorTranslator.FromHtml("#E1F5EE");
        public static readonly Color Coral = ColorTranslator.FromHtml("#993C1D");
        public static readonly Color CoralLight = ColorTranslator.FromHtml("#FAECE7");
        public static readonly Color Lavender = ColorTranslator.FromHtml("#EEEDFE");
        public static readonly Color SideText = ColorTranslator.FromHtml("#CECBF6");
        public static readonly Color Border = ColorTranslator.FromHtml("#C9CED8");
        public static readonly Color Text = ColorTranslator.FromHtml("#1F2937");
        public static readonly Color Muted = ColorTranslator.FromHtml("#6B7280");
        public static readonly Color Danger = ColorTranslator.FromHtml("#A32D2D");

        // ---- Icons (Segoe MDL2 Assets ships with Windows 10 and 11) ----
        // If one ever shows as an empty box, change its code here and every screen updates.
        public const string IconFontName = "Segoe MDL2 Assets";
        public const string GlyphStudent = "\uE7BE";
        public const string GlyphGuest = "\uE77B";
        public const string GlyphAdvisor = "\uE716";
        public const string GlyphAdmin = "\uE713";
        public const string GlyphLogo = "\uE707";
        public const string GlyphAddUser = "\uE8FA";
        public const string GlyphTick = "\uE73E";
        public const string GlyphMail = "\uE715";
        public const string GlyphLock = "\uE72E";
        public const string GlyphPerson = "\uE77B";
        public const string GlyphSchool = "\uE80F";
        public const string GlyphLocation = "\uE707";

        public static Font Icons(float size) => new Font(IconFontName, size);
        public static Font Body(float size = 10F, FontStyle style = FontStyle.Regular) => new Font("Segoe UI", size, style);

        // ---- Role helpers (used by the login screen) ----
        public static string RoleTitle(string role)
        {
            if (role == Roles.Advisor) return "Career Advisor";
            if (role == Roles.Admin) return "Administrator";
            return "Student";
        }

        public static Color RoleColor(string role)
        {
            if (role == Roles.Advisor) return Teal;
            if (role == Roles.Admin) return Coral;
            return Blue;
        }

        public static string RoleGlyph(string role)
        {
            if (role == Roles.Advisor) return GlyphAdvisor;
            if (role == Roles.Admin) return GlyphAdmin;
            return GlyphStudent;
        }

        public static string RoleBlurb(string role)
        {
            if (role == Roles.Advisor) return "Support learners and review advising reports.";
            if (role == Roles.Admin) return "Maintain degrees, requirements and funding.";
            return "Check your degree options, gaps and funding in one place.";
        }

        // ---- Drawing helper ----
        public static GraphicsPath RoundedRect(Rectangle r, int radius)
        {
            int d = radius * 2;
            GraphicsPath path = new GraphicsPath();
            path.AddArc(r.X, r.Y, d, d, 180, 90);
            path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        // ---- Control factories ----
        public static Button PrimaryButton(string text, Color back, int width, int height = 44)
        {
            Button b = new Button
            {
                Text = text,
                Size = new Size(width, height),
                FlatStyle = FlatStyle.Flat,
                BackColor = back,
                ForeColor = Color.White,
                Font = Body(10.5F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                UseVisualStyleBackColor = false
            };
            b.FlatAppearance.BorderSize = 0;
            b.FlatAppearance.MouseOverBackColor = ControlPaint.Dark(back, 0.1F);
            b.FlatAppearance.MouseDownBackColor = ControlPaint.Dark(back, 0.2F);
            return b;
        }

        public static Button OutlineButton(string text, Color color, int width, int height = 42)
        {
            Button b = new Button
            {
                Text = text,
                Size = new Size(width, height),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.White,
                ForeColor = color,
                Font = Body(10.5F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                UseVisualStyleBackColor = false
            };
            b.FlatAppearance.BorderSize = 1;
            b.FlatAppearance.BorderColor = color;
            b.FlatAppearance.MouseOverBackColor = Lavender;
            return b;
        }

        public static Label Caption(string text, int x, int y)
        {
            return new Label
            {
                Text = text,
                AutoSize = true,
                Location = new Point(x, y),
                Font = Body(9.5F, FontStyle.Bold),
                ForeColor = Text
            };
        }

        public static Label Heading(string text, int x, int y)
        {
            return new Label
            {
                Text = text,
                AutoSize = true,
                Location = new Point(x, y),
                Font = Body(22F, FontStyle.Bold),
                ForeColor = Navy
            };
        }

        public static Label SubHeading(string text, int x, int y)
        {
            return new Label
            {
                Text = text,
                AutoSize = true,
                Location = new Point(x, y),
                Font = Body(10F),
                ForeColor = Muted
            };
        }

        public static Label LinkLabel(string text, int x, int y)
        {
            return new Label
            {
                Text = text,
                AutoSize = true,
                Location = new Point(x, y),
                Font = Body(10F),
                ForeColor = Muted,
                Cursor = Cursors.Hand
            };
        }
    }
}
