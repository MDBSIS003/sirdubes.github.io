using System;
using System.Drawing;
using System.Windows.Forms;

namespace FuturePathStudentAdvisingSystem.UI
{
    // GUI-only: no DAL, authentication or business logic. Add as Existing Item.
    public abstract class FPPage : Form
    {
        protected readonly FlowLayoutPanel Main;
        protected static readonly Color Navy = Color.FromArgb(20, 43, 78);
        protected static readonly Color Blue = Color.FromArgb(35, 105, 210);
        protected static readonly Color Green = Color.FromArgb(20, 155, 125);
        protected FPPage(string title, string subtitle)
        {
            Text = "Future Path - " + title;
            Size = new Size(1100, 740);
            MinimumSize = new Size(900, 600);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(245, 248, 252);
            Font = new Font("Segoe UI", 10);
            var sidebar = new FlowLayoutPanel { Dock = DockStyle.Left, Width = 205, BackColor = Navy,
                FlowDirection = FlowDirection.TopDown, WrapContents = false, Padding = new Padding(12, 22, 8, 10) };
            var brand = new Label { Text = "FUTURE PATH", Font = new Font("Segoe UI", 17, FontStyle.Bold),
                ForeColor = Color.White, Width = 185, Height = 70 };
            sidebar.Controls.Add(brand);
            string[] names = { "Learner Profile", "Subjects & Marks", "Career Interests", "Recommendations", "Funding", "Advisor", "Administrator" };
            Func<Form>[] forms = { () => new FPLearnerProfile(), () => new FPSubjectsMarks(), () => new FPCareerInterests(),
                () => new FPDegreeRecommendations(), () => new FPFunding(), () => new FPAdvisorDashboard(), () => new FPAdminDashboard() };
            for (int i = 0; i < names.Length; i++)
            {
                int idx = i;
                var nav = new Button { Text = names[i], Width = 176, Height = 42, BackColor = Navy,
                    ForeColor = Color.White, FlatStyle = FlatStyle.Flat, TextAlign = ContentAlignment.MiddleLeft };
                nav.FlatAppearance.BorderSize = 0;
                nav.Click += (s, e) => { forms[idx]().Show(); Close(); };
                sidebar.Controls.Add(nav);
            }
            var top = new Panel { Dock = DockStyle.Top, Height = 95, BackColor = Color.White };
            top.Controls.Add(new Label { Text = title, Location = new Point(28, 12), AutoSize = true,
                Font = new Font("Segoe UI", 22, FontStyle.Bold), ForeColor = Navy });
            top.Controls.Add(new Label { Text = subtitle, Location = new Point(30, 57), AutoSize = true,
                ForeColor = Color.DimGray });
            Main = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown,
                WrapContents = false, AutoScroll = true, Padding = new Padding(25) };
            Controls.Add(Main);
            Controls.Add(top);
            Controls.Add(sidebar);
        }
        protected Panel Card(string title, int height)
        {
            var p = new Panel { BackColor = Color.White, Width = 800, Height = height, Margin = new Padding(0, 0, 0, 16) };
            p.Controls.Add(new Label { Text = title, Location = new Point(20, 16), AutoSize = true,
                Font = new Font("Segoe UI", 14, FontStyle.Bold), ForeColor = Navy });
            Main.Controls.Add(p);
            return p;
        }
        protected static void Field(Panel p, string title, int x, int y, Control input)
        {
            p.Controls.Add(new Label { Text = title, Location = new Point(x, y), AutoSize = true,
                ForeColor = Navy, Font = new Font("Segoe UI", 9, FontStyle.Bold) });
            input.Location = new Point(x, y + 23);
            input.Width = 330;
            p.Controls.Add(input);
        }
        protected static TextBox Input() => new TextBox { Font = new Font("Segoe UI", 10) };
        protected static ComboBox Choices(params string[] values)
        {
            var box = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
            box.Items.AddRange(values);
            return box;
        }
        protected Button Action(string title)
        {
            var b = new Button { Text = title, BackColor = Green, ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat, Width = 180, Height = 42 };
            b.Click += (s, e) => MessageBox.Show("This screen is a GUI prototype. Connect it to the backend service before submission.", "Future Path");
            Main.Controls.Add(b);
            return b;
        }
        protected static DataGridView Grid(Panel p, params string[] columns)
        {
            var grid = new DataGridView { Location = new Point(20, 60), Size = new Size(755, p.Height - 82),
                AllowUserToAddRows = false, ReadOnly = true, RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, BackgroundColor = Color.White };
            foreach (var column in columns) grid.Columns.Add(column.Replace(" ", ""), column);
            p.Controls.Add(grid);
            return grid;
        }
    }

    public class FPLearnerProfile : FPPage
    {
        public FPLearnerProfile() : base("Learner Profile", "Personal and school information")
        {
            var a = Card("Personal details", 235);
            Field(a, "First name", 20, 65, Input()); Field(a, "Last name", 420, 65, Input());
            Field(a, "Email address", 20, 140, Input()); Field(a, "Contact number", 420, 140, Input());
            var b = Card("School details", 235);
            Field(b, "School name", 20, 65, Input());
            Field(b, "Grade", 420, 65, Choices("Grade 10", "Grade 11", "Grade 12"));
            Field(b, "Province", 20, 140, Choices("Eastern Cape", "Free State", "Gauteng", "KwaZulu-Natal", "Limpopo", "Mpumalanga", "Northern Cape", "North West", "Western Cape"));
            Action("Save profile");
        }
    }

    public class FPSubjectsMarks : FPPage
    {
        public FPSubjectsMarks() : base("Subjects & Marks", "Capture your school subjects and marks")
        {
            var a = Card("Add subject", 160);
            var subject = Choices("Mathematics", "Mathematical Literacy", "English", "Physical Sciences", "Life Sciences", "Accounting", "Business Studies", "Economics", "Geography", "History", "Information Technology", "Computer Applications Technology");
            var mark = new NumericUpDown { Minimum = 0, Maximum = 100 };
            Field(a, "Subject", 20, 65, subject); Field(a, "Mark (%)", 420, 65, mark);
            var b = Card("Your results", 320);
            var grid = Grid(b, "Subject", "Mark (%)");
            var add = new Button { Text = "Add selected subject", Width = 190, Height = 40, BackColor = Blue, ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            add.Click += (s, e) => { if (subject.SelectedItem == null) { MessageBox.Show("Choose a subject."); return; }
                string name = subject.SelectedItem.ToString() ?? "";
                foreach (DataGridViewRow row in grid.Rows) if (Convert.ToString(row.Cells[0].Value) == name) { row.Cells[1].Value = mark.Value; return; }
                grid.Rows.Add(name, mark.Value); };
            Main.Controls.Add(add);
            Action("Save results");
        }
    }

    public class FPCareerInterests : FPPage
    {
        public FPCareerInterests() : base("Career Interests", "Tell us what you enjoy")
        {
            var a = Card("Interest questionnaire", 465);
            string[] questions = { "I enjoy solving technical problems.", "I like helping other people.", "I enjoy designing and creating things.", "I am interested in business.", "I enjoy science and research." };
            for (int i = 0; i < questions.Length; i++)
            {
                int y = 65 + i * 75;
                a.Controls.Add(new Label { Text = questions[i], Location = new Point(20, y + 5), Width = 450, ForeColor = Navy });
                var answer = Choices("Strongly disagree", "Disagree", "Neutral", "Agree", "Strongly agree");
                answer.Location = new Point(485, y); answer.Width = 280; a.Controls.Add(answer);
            }
            Action("Save responses");
        }
    }

    public class FPDegreeRecommendations : FPPage
    {
        public FPDegreeRecommendations() : base("Degree Recommendations", "Discover programmes that match your profile")
        {
            var a = Card("Your matches", 130);
            a.Controls.Add(new Label { Text = "Recommendations will be calculated by the backend using your marks, interests and programme requirements.",
                Location = new Point(20, 65), Width = 740, Height = 45, ForeColor = Color.DimGray });
            var b = Card("Recommended programmes", 335);
            Grid(b, "Programme", "Eligibility", "Match", "Explanation");
            Action("Find recommendations");
        }
    }

    public class FPFunding : FPPage
    {
        public FPFunding() : base("Funding Opportunities", "Explore bursaries and other study funding")
        {
            var a = Card("Search opportunities", 165);
            Field(a, "Search by name", 20, 65, Input());
            Field(a, "Funding type", 420, 65, Choices("All", "Bursary", "Scholarship", "Loan"));
            var b = Card("Funding results", 340);
            Grid(b, "Funding name", "Type", "Eligibility", "Deadline");
            Action("Search funding");
        }
    }

    public class FPAdvisorDashboard : FPPage
    {
        public FPAdvisorDashboard() : base("Advisor Dashboard", "Find learners and review their advising progress")
        {
            var a = Card("Learner lookup", 160);
            Field(a, "Search learner by name or ID", 20, 65, Input());
            var b = Card("Learner records", 350);
            Grid(b, "Learner", "Grade", "Profile status", "Advising status");
            Action("Search learners");
        }
    }

    public class FPAdminDashboard : FPPage
    {
        public FPAdminDashboard() : base("Administrator Dashboard", "Manage programmes, funding and system records")
        {
            var a = Card("Management", 290);
            string[] items = { "Manage degrees", "Admission requirements", "Funding records", "Users and roles" };
            for (int i = 0; i < items.Length; i++)
            {
                var button = new Button { Text = items[i], Location = new Point(20 + (i % 2) * 380, 65 + (i / 2) * 85),
                    Width = 330, Height = 55, BackColor = Color.FromArgb(232, 243, 251), ForeColor = Navy, FlatStyle = FlatStyle.Flat };
                button.Click += (s, e) => MessageBox.Show("This module needs backend integration and administrator authorisation.");
                a.Controls.Add(button);
            }
            var b = Card("System reports", 175);
            b.Controls.Add(new Label { Text = "Reports will be populated from the database.", Location = new Point(20, 70), AutoSize = true });
        }
    }
}
