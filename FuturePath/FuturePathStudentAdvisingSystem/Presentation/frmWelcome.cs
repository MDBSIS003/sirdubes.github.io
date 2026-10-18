using FuturePathStudentAdvisingSystem.Security;
using FuturePathStudentAdvisingSystem.UI;

namespace FuturePathStudentAdvisingSystem
{
    /// <summary>
    /// The welcome dashboard: first screen of the application.
    /// Student / Career Advisor / Administrator go to the login screen; Guest goes straight in.
    /// </summary>
    public class frmWelcome : BrandedForm
    {
        public frmWelcome()
            : base("Future Path - Welcome", Theme.Navy, Theme.Teal, Theme.GlyphLogo,
                   "Future Path", "Find the degree that fits you.")
        {
            // ---- Side panel extras ----
            string[] points =
            {
                "Check degree requirements",
                "Match your interests",
                "Discover funding options"
            };

            int y = 300;
            foreach (string point in points)
            {
                Label tick = new Label
                {
                    Text = Theme.GlyphTick,
                    Font = Theme.Icons(11F),
                    ForeColor = ColorTranslator.FromHtml("#5DCAA5"),
                    AutoSize = true,
                    Location = new Point(22, y)
                };
                Label text = new Label
                {
                    Text = point,
                    Font = Theme.Body(9.5F),
                    ForeColor = Color.White,
                    AutoSize = true,
                    Location = new Point(46, y - 2)
                };
                Side.Controls.Add(tick);
                Side.Controls.Add(text);
                y += 34;
            }

            Label disclaimer = new Label
            {
                Text = "Guidance only. This is not an official admission decision.",
                Font = Theme.Body(8.5F),
                ForeColor = ColorTranslator.FromHtml("#AFA9EC"),
                AutoSize = false,
                Location = new Point(18, FormHeight - 70),
                Size = new Size(SideWidth - 36, 48)
            };
            Side.Controls.Add(disclaimer);

            // ---- Working area ----
            Content.Controls.Add(Theme.Heading("Welcome", 28, 30));
            Content.Controls.Add(Theme.SubHeading("Who is using Future Path today?", 31, 78));

            RoleCard student = new RoleCard(Theme.GlyphStudent, "Student",
                "Log in to explore degrees and funding",
                Theme.BlueLight, Theme.Blue, Theme.Navy);
            RoleCard guest = new RoleCard(Theme.GlyphGuest, "Guest",
                "Try it anonymously, no account needed",
                Theme.AmberLight, Theme.Amber, Theme.Navy);
            RoleCard advisor = new RoleCard(Theme.GlyphAdvisor, "Career Advisor",
                "Support learners and view reports",
                Theme.TealLight, Theme.TealDark, Theme.Navy);
            RoleCard admin = new RoleCard(Theme.GlyphAdmin, "Administrator",
                "Manage degrees, requirements and funding",
                Theme.CoralLight, Theme.Coral, Theme.Navy);

            student.Location = new Point(28, 124);
            guest.Location = new Point(276, 124);
            advisor.Location = new Point(28, 276);
            admin.Location = new Point(276, 276);

            Content.Controls.Add(student);
            Content.Controls.Add(guest);
            Content.Controls.Add(advisor);
            Content.Controls.Add(admin);

            Button exit = Theme.OutlineButton("Exit", Theme.Muted, 100, 36);
            exit.Location = new Point(FormWidth - SideWidth - 28 - 100, FormHeight - 62);
            Content.Controls.Add(exit);

            student.Click += (s, e) => OpenLogin(Roles.Learner);
            advisor.Click += (s, e) => OpenLogin(Roles.Advisor);
            admin.Click += (s, e) => OpenLogin(Roles.Admin);
            guest.Click += (s, e) => StartGuest();
            exit.Click += (s, e) => Close();
        }

        private void OpenLogin(string role)
        {
            using (frmLogin login = new frmLogin(role))
            {
                Hide();
                if (login.ShowDialog() == DialogResult.OK)
                    OpenHomeScreen();
                Show();
            }
        }

        private void StartGuest()
        {
            AppSession.StartGuest();
            Hide();
            OpenHomeScreen();
            Show();
        }

        /// <summary>Sends each role to its own starting screen, then logs the user out.</summary>
        private void OpenHomeScreen()
        {
            string? route = AppSession.Role == Roles.Advisor ? "Advisor" : AppSession.Role == Roles.Admin ? "Administrator" : "Profile";
            while (route != null)
            {
                using FPPage page = route switch
                {
                    "Profile" => new FPLearnerProfile(),
                    "Subjects" => new FPSubjectsMarks(),
                    "Interests" => new FPCareerInterests(),
                    "Recommendations" => new FPDegreeRecommendations(),
                    "Funding" => new FPFunding(),
                    "Advisor" => new FPAdvisorDashboard(),
                    "Administrator" => new FPAdminDashboard(),
                    "Reports" => new FPReports(),
                    _ => throw new InvalidOperationException("Unknown route.")
                };
                page.ShowDialog();
                route = page.NextPage;
            }

            AppSession.End();
        }
    }
}
