using System.Data;
using FuturePathStudentAdvisingSystem.BusinessLogic;
using FuturePathStudentAdvisingSystem.DataAccess;
using FuturePathStudentAdvisingSystem.Models;
using FuturePathStudentAdvisingSystem.Security;

namespace FuturePathStudentAdvisingSystem.UI;

public abstract class FPPage : Form
{
    protected readonly FlowLayoutPanel Main;
    protected static readonly Color Navy = Theme.Navy, Blue = Theme.Blue, Green = Theme.TealDark;
    public string? NextPage { get; private set; }
    protected FPPage(string route, string title, string subtitle)
    {
        AccessPolicy.RequirePage(route);
        Text = "Future Path — " + title; ClientSize = new(1120, 760); MinimumSize = new(950, 650);
        StartPosition = FormStartPosition.CenterScreen; Font = Theme.Body(10); BackColor = Color.FromArgb(245, 248, 252);
        AutoScaleMode = AutoScaleMode.Dpi;
        var sidebar = new FlowLayoutPanel
        {
            Dock = DockStyle.Left,
            Width = 210,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            AutoScroll = true,
            BackColor = Navy,
            Padding = new(12, 20, 8, 10)
        };
        sidebar.Controls.Add(new Label { Text = "FUTURE PATH", Width = 180, Height = 45, ForeColor = Color.White, Font = Theme.Body(17, FontStyle.Bold) });
        sidebar.Controls.Add(new Label { Text = $"{AppSession.DisplayName}\n{AppSession.Role}", Width = 180, Height = 65, ForeColor = Color.White });
        var routes = AppSession.Role is Roles.Guest or Roles.Learner
            ? new[] { "Profile", "Subjects", "Interests", "Recommendations", "Funding" }
            : AppSession.Role == Roles.Advisor ? new[] { "Advisor", "Reports" } : new[] { "Administrator", "Reports" };
        foreach (var name in routes)
        {
            var b = NavButton(name); b.Enabled = name != route;
            b.Click += (_, _) => Navigate(name); sidebar.Controls.Add(b);
        }
        if (AppSession.IsLoggedIn)
        {
            var password = NavButton("Change password"); password.Click += (_, _) => Run(() => ChangePassword()); sidebar.Controls.Add(password);
        }
        var logout = NavButton(AppSession.IsLoggedIn ? "Log out" : "Back to welcome");
        logout.Click += (_, _) => { AppSession.End(); Close(); }; sidebar.Controls.Add(logout);
        var top = new Panel { Dock = DockStyle.Top, Height = 110, BackColor = Color.White };
        top.Controls.Add(new Label { Text = title, Location = new(24, 12), AutoSize = true, Font = Theme.Body(22, FontStyle.Bold), ForeColor = Navy });
        top.Controls.Add(new Label { Text = subtitle, Location = new(26, 60), Width = 800, Height = 45, ForeColor = Theme.Muted });
        Main = new() { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, WrapContents = false, AutoScroll = true, Padding = new(20) };
        Controls.Add(Main); Controls.Add(top); Controls.Add(sidebar);
        Main.ClientSizeChanged += (_, _) => { foreach (Control c in Main.Controls) c.Width = Math.Max(650, Main.ClientSize.Width - 58); };
        Shown += (_, _) => Run(() => AccessPolicy.RequirePage(route));
    }
    private Button NavButton(string text) => new() { Text = text, Width = 178, Height = 43, BackColor = Navy, ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
    protected void Navigate(string route) { AccessPolicy.RequirePage(route); NextPage = route; Close(); }
    protected static void Run(System.Action action)
    {
        try { action(); }
        catch (Microsoft.Data.SqlClient.SqlException ex) when (ex.Number is 2601 or 2627)
        { MessageBox.Show("This record already exists. Update the existing record instead.", "Duplicate record", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        catch (Microsoft.Data.SqlClient.SqlException ex) when (ex.Number == 547)
        { MessageBox.Show("This record is linked to other data or contains an invalid reference. Correct the reference or deactivate the record.", "Linked record", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        catch (Exception ex)
        { MessageBox.Show(ex.Message + "\n\nFor connection errors, check LocalDB and run the documented database setup.", "Future Path", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
    }
    protected FlowLayoutPanel Card(string title)
    {
        var p = new FlowLayoutPanel
        {
            Width = 800,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            MinimumSize = new(650, 0),
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            BackColor = Color.White,
            Padding = new(16),
            Margin = new(0, 0, 0, 16)
        };
        p.Controls.Add(new Label { Text = title, AutoSize = true, Font = Theme.Body(14, FontStyle.Bold), ForeColor = Navy, Margin = new(3, 0, 3, 15) });
        Main.Controls.Add(p); return p;
    }
    protected static void Field(FlowLayoutPanel p, string label, Control input)
    {
        p.Controls.Add(new Label { Text = label, AutoSize = true, ForeColor = Navy, Margin = new(3, 8, 3, 3) });
        input.Width = 590; input.Margin = new(3, 0, 3, 8); p.Controls.Add(input);
    }
    protected static TextBox Input(int max = 100) => new() { MaxLength = max };
    protected static ComboBox Choices(params string[] values)
    { var b = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList }; b.Items.AddRange(values); return b; }
    protected static void Bind(ComboBox box, DataTable data, string value, string display)
    { box.DisplayMember = display; box.ValueMember = value; box.DataSource = data; box.SelectedIndex = -1; }
    protected Button Action(string text, System.Action handler, FlowLayoutPanel? parent = null)
    {
        var b = Theme.PrimaryButton(text, Green, 260, 42); b.Margin = new(3, 8, 3, 8); b.Click += (_, _) => Run(handler);
        (parent ?? Main).Controls.Add(b); return b;
    }
    protected static DataGridView Grid(FlowLayoutPanel p, int height = 270)
    {
        var grid = new DataGridView
        {
            Width = 730,
            Height = height,
            ReadOnly = true,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            AutoGenerateColumns = true,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells,
            RowHeadersVisible = false,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            BackgroundColor = Color.White
        };
        p.Controls.Add(grid); return grid;
    }
    protected static void ShowText(string title, string text)
    {
        using var f = new Form { Text = title, Size = new(900, 650), StartPosition = FormStartPosition.CenterParent };
        f.Controls.Add(new TextBox
        {
            Multiline = true,
            ReadOnly = true,
            Dock = DockStyle.Fill,
            ScrollBars = ScrollBars.Both,
            WordWrap = true,
            Font = Theme.Body(10),
            Text = text.Replace("\r\n", "\n").Replace("\n", Environment.NewLine)
        }); f.ShowDialog();
    }
    protected void Export(string title, string text, int? sessionID = null)
    {
        using var save = new SaveFileDialog { Filter = "Text report (*.txt)|*.txt", FileName = title + "-" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".txt" };
        if (save.ShowDialog(this) != DialogResult.OK) return;
        File.WriteAllText(save.FileName, text, System.Text.Encoding.UTF8);
        if (sessionID is > 0) new AdvisingDAL().RecordExport(sessionID.Value);
        MessageBox.Show("Report saved.");
    }
    private void ChangePassword()
    {
        using var dialog = new Form { Text = "Change password", ClientSize = new(430, 240), StartPosition = FormStartPosition.CenterParent };
        var p = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, Padding = new(15) };
        var current = new TextBox { UseSystemPasswordChar = true, Width = 380, MaxLength = 128, PlaceholderText = "Current password" };
        var replacement = new TextBox { UseSystemPasswordChar = true, Width = 380, MaxLength = 128, PlaceholderText = "New password (10–128 characters)" };
        var confirm = new TextBox { UseSystemPasswordChar = true, Width = 380, MaxLength = 128, PlaceholderText = "Confirm new password" };
        var ok = new Button { Text = "Save", Width = 150 };
        ok.Click += (_, _) => Run(() => { if (replacement.Text != confirm.Text) throw new ArgumentException("Passwords do not match."); new UserAccountDAL().ChangeOwnPassword(current.Text, replacement.Text); dialog.Close(); });
        p.Controls.AddRange([current, replacement, confirm, ok]); dialog.Controls.Add(p); dialog.ShowDialog(this);
    }
}

public class FPLearnerProfile : FPPage
{
    public FPLearnerProfile() : base("Profile", "Learner Dashboard & Profile", "Save your profile, then start or load an advising session. Guests use anonymous, memory-only sessions.")
    {
        var account = Card("Personal and school details");
        var first = Input(50); var last = Input(50); var email = Input(50); var phone = Input(25); var school = Input(100);
        email.ReadOnly = true; var province = Choices(Validation.Provinces); var grade = Choices();
        if (AppSession.IsLoggedIn)
        {
            Field(account, "First name", first); Field(account, "Last name", last); Field(account, "Account email", email);
            Field(account, "Contact number (optional)", phone); Field(account, "School", school); Field(account, "Province", province);
        }
        Field(account, "Current grade", grade);
        var session = Card("Choose your preferred faculty and degree");
        var faculty = Choices(); var degree = Choices(); Field(session, "Faculty", faculty); Field(session, "Degree programme", degree);
        List<DegreeProgramme> programmes = [];
        faculty.SelectedValueChanged += (_, _) => { if (faculty.SelectedValue is int f) { degree.DataSource = programmes.Where(d => d.FacultyID == f).ToList(); degree.DisplayMember = "ProgrammeName"; degree.ValueMember = "DegreeID"; degree.SelectedIndex = -1; } };
        if (AppSession.IsLoggedIn) Action("Save profile", () =>
        {
            new UserAccountDAL().SaveProfile(new()
            {
                UserID = AppSession.UserID!.Value,
                FirstName = first.Text,
                LastName = last.Text,
                Email = email.Text,
                SchoolName = school.Text,
                Province = province.Text,
                Phone = phone.Text,
                GradeLevelID = grade.SelectedValue as int?
            }); MessageBox.Show("Profile saved without creating another account.");
        }, account);
        Action("Start new advising session", () =>
        {
            if (grade.SelectedValue is not int g || degree.SelectedValue is not int d) throw new ArgumentException("Select a grade, faculty and degree.");
            if (AdvisingWorkflow.Current != null && MessageBox.Show("Start a new session? Save any current edits first.", "New session", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
            AdvisingWorkflow.Begin(g, d); Navigate("Subjects");
        }, session);
        if (AppSession.IsLoggedIn)
        {
            var history = Card("Saved advising sessions"); var grid = Grid(history, 200);
            Action("Load selected session", () => { if (grid.CurrentRow?.DataBoundItem is DataRowView r) { AdvisingWorkflow.Load((int)r["SessionID"]); Navigate("Recommendations"); } else throw new ArgumentException("Select a saved session."); }, history);
            Shown += (_, _) => Run(() => grid.DataSource = new AdvisingDAL().Sessions(AppSession.UserID!.Value));
        }
        Shown += (_, _) => Run(() =>
        {
            Bind(grade, new GradeLevelDAL().GetGradeLevels(), "GradeLevelID", "GradeName");
            programmes = new DegreeProgrammeDAL().GetActiveDegreeProgrammes(); Bind(faculty, new AdvisingDAL().Faculties(), "FacultyID", "FacultyName");
            if (AppSession.UserID is int id && new UserAccountDAL().GetLearnerProfile(id) is { } p)
            { first.Text = p.FirstName; last.Text = p.LastName; email.Text = p.Email; phone.Text = p.Phone; school.Text = p.SchoolName; province.SelectedItem = p.Province; if (p.GradeLevelID is int g) grade.SelectedValue = g; }
            if (AdvisingWorkflow.Current is { } current) { grade.SelectedValue = current.GradeLevelID; faculty.SelectedValue = current.FacultyID; degree.SelectedValue = current.DegreeID; }
        });
    }
}
public class FPSubjectsMarks : FPPage
{
    public FPSubjectsMarks() : base("Subjects", "Subjects & Marks", "Add, update and remove results. Confirm that your list is complete before assessing eligibility.")
    {
        var p = Card("School results"); var subject = Choices(); var mark = new NumericUpDown { Minimum = 0, Maximum = 100, DecimalPlaces = 2 };
        Field(p, "Subject", subject); Field(p, "Mark (%)", mark); var grid = Grid(p);
        var complete = new CheckBox { Text = "I have entered my complete subject list", AutoSize = true }; p.Controls.Add(complete);
        var marks = new List<SubjectMark>();
        void RefreshGrid() { grid.DataSource = null; grid.DataSource = marks.ToList(); }
        grid.SelectionChanged += (_, _) => { if (grid.CurrentRow?.DataBoundItem is SubjectMark m) { subject.SelectedValue = m.SubjectID; mark.Value = m.Mark; } };
        Action("Add subject", () => { if (subject.SelectedValue is not int id) throw new ArgumentException("Select a subject."); if (marks.Any(x => x.SubjectID == id)) throw new ArgumentException("Subject already exists. Use Update."); marks.Add(new(id, subject.Text, mark.Value)); RefreshGrid(); }, p);
        Action("Update selected result", () => { if (grid.CurrentRow?.DataBoundItem is not SubjectMark m) throw new ArgumentException("Select a result."); int i = marks.FindIndex(x => x.SubjectID == m.SubjectID); marks[i] = m with { Mark = mark.Value }; RefreshGrid(); }, p);
        Action("Remove selected result", () => { if (grid.CurrentRow?.DataBoundItem is SubjectMark m && MessageBox.Show("Remove " + m.SubjectName + "?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes) { marks.RemoveAll(x => x.SubjectID == m.SubjectID); RefreshGrid(); } }, p);
        Action("Save results", () => { AdvisingWorkflow.SaveMarks(marks, complete.Checked); MessageBox.Show("Results saved."); }, p);
        Shown += (_, _) => Run(() => { var s = AdvisingWorkflow.RequireCurrent(); Bind(subject, new SubjectDAL().GetSubjects(), "SubjectID", "SubjectName"); marks = s.Marks.ToList(); complete.Checked = s.SubjectsComplete; RefreshGrid(); });
    }
}
public class FPCareerInterests : FPPage
{
    public FPCareerInterests() : base("Interests", "Career Interests", "Answer every question. Scores: 1 strongly disagree through 5 strongly agree.")
    {
        var p = Card("Interest questionnaire"); var inputs = new Dictionary<int, ComboBox>();
        Shown += (_, _) => Run(() =>
        {
            var s = AdvisingWorkflow.RequireCurrent();
            foreach (var q in new AdvisingDAL().Questions())
            {
                var a = Choices("1 — Strongly disagree", "2 — Disagree", "3 — Neutral", "4 — Agree", "5 — Strongly agree"); Field(p, q.QuestionText, a); inputs[q.QuestionID] = a;
                var old = s.Responses.FirstOrDefault(r => r.QuestionID == q.QuestionID); if (old != null) a.SelectedIndex = (int)old.Score - 1;
            }
            Action("Save responses", () => { AdvisingWorkflow.SaveResponses(inputs.Where(x => x.Value.SelectedIndex >= 0).Select(x => new InterestResponse(x.Key, x.Value.SelectedIndex + 1)).ToList()); MessageBox.Show("Responses saved."); }, p);
        });
    }
}
public class FPDegreeRecommendations : FPPage
{
    public FPDegreeRecommendations() : base("Recommendations", "Degree Recommendations", "Academic eligibility and interest fit are separate. Sample guidance does not guarantee admission.")
    {
        var p = Card("Assessment and alternatives"); var selected = new Label { AutoSize = true, MaximumSize = new(700, 0) }; p.Controls.Add(selected);
        var funding = new CheckBox { Text = "I would like information on relevant funding", AutoSize = true }; p.Controls.Add(funding);
        var grid = Grid(p, 320); grid.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        Action("Calculate & complete session", () =>
        { var results = AdvisingWorkflow.Complete(funding.Checked); grid.DataSource = results.OrderBy(x => x.IsAlternative).ToList(); if (funding.Checked && MessageBox.Show("View related funding now?", "Funding", MessageBoxButtons.YesNo) == DialogResult.Yes) Navigate("Funding"); }, p);
        Action("View full explanation", () => { if (grid.CurrentRow?.DataBoundItem is Recommendation r) ShowText(r.ProgrammeName, r.Explanation); else if (grid.CurrentRow?.DataBoundItem is DataRowView row) ShowText("Stored assessment", Convert.ToString(row["GapExplanation"])!); }, p);
        Action("Export advisory summary", () => { var s = AdvisingWorkflow.RequireCurrent(); if (!s.IsFinalized) throw new InvalidOperationException("Calculate and complete the session first."); Export("Learner-advisory-summary", new ReportService().Summary(s), s.SessionID); }, p);
        Shown += (_, _) => Run(() =>
        {
            var s = AdvisingWorkflow.RequireCurrent(); selected.Text = $"Session {s.SessionID} — " + (s.IsFinalized ? "Completed; start a new session to change results." : "In progress; save marks and interests before calculation."); funding.Checked = s.FundingRequested;
            if (s.IsFinalized && s.SessionID != 0) grid.DataSource = new AdvisingDAL().StoredRecommendations(s.SessionID); else grid.DataSource = AdvisingWorkflow.Calculate(s).OrderBy(x => x.IsAlternative).ToList();
        });
    }
}
public class FPFunding : FPPage
{
    public FPFunding() : base("Funding", "Funding Opportunities", "Local sample information requires verification. Inactive and expired listings are excluded.")
    {
        var p = Card("Search and filter"); var search = Input(250); var type = Choices(); var faculty = Choices();
        Field(p, "Funding name contains", search); Field(p, "Funding type (blank = all)", type); Field(p, "Faculty (blank = all)", faculty);
        var linked = new CheckBox { Text = "Only opportunities linked to my selected degree", AutoSize = true, Checked = AdvisingWorkflow.Current != null }; p.Controls.Add(linked);
        var grid = Grid(p, 300);
        void Search() => grid.DataSource = new AdvisingDAL().Funding(search.Text, type.SelectedItem as string ?? "", linked.Checked ? AdvisingWorkflow.RequireCurrent().DegreeID : null, faculty.SelectedValue as int?);
        Action("Search funding", Search, p); Action("Clear filters", () => { search.Clear(); type.SelectedIndex = -1; faculty.SelectedIndex = -1; linked.Checked = false; Search(); }, p);
        Action("View requirements & contact", () => { if (grid.CurrentRow?.DataBoundItem is FundingOpportunity f) ShowText(f.FundingName, $"{(f.IsSample ? "SAMPLE" : "REQUIRES VERIFICATION")}\n{f.FundingType}\n{f.EligibilityDescription}\nRequired documents: {f.RequiredDocuments}\nClosing date: {f.ClosingDate:yyyy-MM-dd}\nContact / application information: {f.ContactInfo}\nNo applications are submitted by this system."); }, p);
        Shown += (_, _) => Run(() => { foreach (DataRow r in new AdvisingDAL().FundingTypes().Rows) type.Items.Add((string)r[0]); Bind(faculty, new AdvisingDAL().Faculties(), "FacultyID", "FacultyName"); Search(); });
    }
}
public class FPAdvisorDashboard : FPPage
{
    public FPAdvisorDashboard() : base("Advisor", "Career Advisor Dashboard", "Only learners assigned to your account are visible. Administrators manage assignments.")
    {
        var p = Card("Find an assigned learner"); var query = Input(100); Field(p, "Name or learner ID", query); var learners = Grid(p, 180);
        var detail = Card("Academic and advising information"); var sessions = Grid(detail, 150); var notes = Input(2000); notes.Multiline = true; notes.Height = 90;
        Field(detail, "Advising note for selected session", notes);
        Action("Search learners", () => learners.DataSource = new AdvisingDAL().SearchLearners(query.Text), p);
        Action("Load learner sessions", () => { if (learners.CurrentRow?.DataBoundItem is not DataRowView r) throw new ArgumentException("Select a learner."); sessions.DataSource = new AdvisingDAL().Sessions((int)r["UserID"]); }, p);
        int SessionID() => sessions.CurrentRow?.DataBoundItem is DataRowView r ? (int)r["SessionID"] : throw new ArgumentException("Select a session.");
        Action("View academic details", () => { var s = new AdvisingDAL().Load(SessionID()); var profile = new UserAccountDAL().GetLearnerProfile(s.UserID); ShowText("Learner details", $"{profile?.FirstName} {profile?.LastName}\n{profile?.SchoolName}\n" + string.Join("\n", s.Marks.Select(m => $"{m.SubjectName}: {m.Mark}%")) + "\n\n" + ReportService.TableText(new AdvisingDAL().StoredRecommendations(s.SessionID))); }, detail);
        Action("Save advising note", () => { new AdvisingDAL().AddNote(SessionID(), notes.Text); notes.Clear(); MessageBox.Show("Advising note saved."); }, detail);
        Action("View saved notes", () => ShowText("Advising notes", ReportService.TableText(new AdvisingDAL().Notes(SessionID()))), detail);
        Action("Export learner summary", () => { var s = new AdvisingDAL().Load(SessionID()); Export("Learner-advisory-summary", new ReportService().Summary(s), s.SessionID); }, detail);
        Shown += (_, _) => Run(() => learners.DataSource = new AdvisingDAL().SearchLearners(""));
    }
}
public class FPAdminDashboard : FPPage
{
    public FPAdminDashboard() : base("Administrator", "Administrator Dashboard", "Maintain local reference data and accounts. Deactivation preserves programme, funding and user history.")
    {
        var p = Card("Management");
        foreach (var d in AdminDAL.Definitions)
        { var definition = d; Action("Manage " + d.Name, () => { using var editor = new ManagementForm(definition.Name); editor.ShowDialog(this); }, p); }
    }
}
public class FPReports : FPPage
{
    public FPReports() : base("Reports", "System Reports", "Eligibility activity and exceptions are scoped to assigned learners for advisors.")
    {
        var p = Card("Database reports"); var choice = Choices("Degree Interest and Eligibility", "Learners Not Currently Eligible / Missing Information"); Field(p, "Report", choice); choice.SelectedIndex = 0; var grid = Grid(p, 360);
        DataTable Read() => choice.SelectedIndex == 0 ? new ReportDAL().EligibilityActivity() : new ReportDAL().Exceptions();
        Action("Generate report", () => grid.DataSource = Read(), p);
        Action("Export selected report", () => Export("FuturePath-report", "FUTURE PATH — " + choice.Text + "\nGenerated: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss zzz") + "\n" + ReportService.TableText(Read()) + "\n" + ReportService.Disclaimer), p);
    }
}
