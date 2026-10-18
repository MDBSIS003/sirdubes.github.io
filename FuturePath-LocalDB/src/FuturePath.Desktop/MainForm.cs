using FuturePath.Business;

namespace FuturePath.Desktop;
internal sealed class MainForm : AppForm
{
    private readonly AppServices services;
    private Control? reportSource;
    private readonly Panel content = new()
    {
        Dock = DockStyle.Fill
    };
    private readonly FlowLayoutPanel navigation = new()
    {
        Dock = DockStyle.Left,
        Width = 215,
        FlowDirection = FlowDirection.TopDown,
        WrapContents = false,
        Padding = new Padding(16, 26, 12, 20),
        BackColor = Ui.Navy
    };
    private readonly Label title = Ui.Label("Welcome to Future Path", 25, FontStyle.Bold);
    private readonly Label subtitle = Ui.Label("Find a study pathway with confidence.");
    private readonly Label identity = Ui.Label("Learner workspace");
    public MainForm(AppServices services)
    {
        this.services = services;
        Text = "Future Path | Student Advising";
        Size = new Size(Math.Min(1280, Screen.PrimaryScreen!.WorkingArea.Width), Math.Min(850, Screen.PrimaryScreen.WorkingArea.Height));
        MinimumSize = new Size(950, 620);
        StartPosition = FormStartPosition.CenterScreen;
        var header = new FlowLayoutPanel
        {
            Dock = DockStyle.Top,
            Height = 114,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            BackColor = Color.White,
            Padding = new Padding(26, 18, 15, 10)
        };
        header.Controls.Add(title);
        header.Controls.Add(subtitle);
        var status = new StatusStrip
        {
            SizingGrip = true,
            BackColor = Color.White
        };
        status.Items.Add(new ToolStripStatusLabel("LocalDB: " + services.DatabaseName + "  |  Works offline  |  Sample information — verify before applying"));
        var main = new Panel
        {
            Dock = DockStyle.Fill
        };
        main.Controls.Add(content);
        main.Controls.Add(header);
        main.Controls.Add(status);
        Controls.Add(main);
        Controls.Add(navigation);
        FormClosing += (_, e) =>
        {
            e.Cancel = !CanLeave();
        };
        RefreshNavigation();
        Home();
    }

    private void RefreshNavigation()
    {
        navigation.Controls.Clear();
        var brand = Ui.Label("FUTURE\nPATH", 25, FontStyle.Bold);
        brand.ForeColor = Color.White;
        brand.Margin = new Padding(0, 0, 0, 15);
        navigation.Controls.Add(brand);
        var line = Ui.Label("STUDENT ADVISING", 8, FontStyle.Bold);
        line.ForeColor = Color.FromArgb(151, 183, 199);
        navigation.Controls.Add(line);
        Nav("Overview", Home);
        Nav("New advisory session", NewAdvisory);
        if (services.Access.IsStaff)
        {
            Nav("Learners & sessions", () => ShowPage(new AdvisorPage(services, ViewReport, NewFromSession), "Learners & sessions", "Review saved sessions, support learners, and compare outcomes."));
            Nav("Reports", () => ShowPage(new ReportsPage(services, ViewReport), "Advising reports", "Filter local records and export reports for guidance discussions."));
            if (services.Access.Role == UserRole.Administrator)
                Nav("Administration", () => ShowPage(new MaintenancePage(services), "Reference data", "Maintain local programmes, rules, questionnaires and funding."));
            Nav("Change password", () =>
            {
                using var dialog = new PasswordDialog(services);
                dialog.ShowDialog(this);
            });
            Nav("Sign out", Logout);
        }
        else
            Nav("Staff sign in", Login);
        identity.Text = services.Access.IsStaff ? $"{services.Access.DisplayName}\n{(services.Access.Role == UserRole.Advisor ? "Career Advisor" : "Administrator")}" : "Private learner workspace\nNo sign-in needed";
        identity.ForeColor = Color.FromArgb(174, 205, 214);
        identity.MaximumSize = new Size(178, 0);
        identity.Margin = new Padding(0, 28, 0, 10);
        navigation.Controls.Add(identity);
    }

    private void Nav(string text, Action action)
    {
        var b = Ui.Button(text, action);
        b.Width = 182;
        b.AutoSize = false;
        b.Height = 45;
        b.BackColor = Ui.Navy;
        b.ForeColor = Color.White;
        b.FlatAppearance.BorderSize = 0;
        b.TextAlign = ContentAlignment.MiddleLeft;
        navigation.Controls.Add(b);
    }

    private bool CanLeave()
    {
        bool unsaved = content.Controls.OfType<AdvisingPage>().Any(page => page.HasUnsavedInput) || content.Controls.OfType<MaintenancePage>().Any(page => page.HasUnsavedInput);
        return !unsaved || Ui.Confirm("Leave this page? Unsaved entries will be lost.");
    }

    private void ShowPage(UserControl page, string heading, string description)
    {
        if (!CanLeave())
        {
            page.Dispose();
            return;
        }

        reportSource?.Dispose();
        reportSource = null;
        foreach (Control old in content.Controls.Cast<Control>().ToArray())
        {
            content.Controls.Remove(old);
            old.Dispose();
        }

        title.Text = heading;
        subtitle.Text = description;
        page.Dock = DockStyle.Fill;
        content.Controls.Add(page);
    }

    private void Home()
    {
        var page = new UserControl();
        var stack = Ui.Stack();
        page.Controls.Add(stack);
        var hero = Ui.Card("Your next chapter starts with a clearer picture.", "Explore your subjects, interests and study options. Every recommendation explains the rules behind it.");
        hero.Controls.Add(Ui.Row(Ui.Button("Start an advisory session", NewAdvisory, true), Ui.Button(services.Access.IsStaff ? "View saved sessions" : "Staff sign in", services.Access.IsStaff ? () => ShowPage(new AdvisorPage(services, ViewReport, NewFromSession), "Learners & sessions", "Saved advisory sessions") : Login)));
        stack.Controls.Add(hero);
        foreach (var item in new[]
        {
            ("01  Capture your profile", "Choose Grade 11 or 12, then add subjects and marks. Personal and contact details are optional."),
            ("02  Explore your options", "Complete the eight-question interest questionnaire and select a faculty and degree. See eligibility, gaps and ranked alternatives."),
            ("03  Make a plan", "Opt in to funding guidance, then save or print your advisory summary to discuss with a parent, teacher or career advisor.")
        }

        )
            stack.Controls.Add(Ui.Card(item.Item1, item.Item2));
        stack.Controls.Add(Ui.Label(RecommendationEngine.Disclaimer, 9));
        stack.Controls.Add(Ui.Label("Privacy: a learner workspace cannot browse other learners. For privacy on a shared computer, end your session by closing the app. Staff can retrieve saved records after signing in.", 9));
        ShowPage(page, "Plan your future", "Step 1 of 7 • Overview. Understand your options and identify the next step.");
    }

    private void NewAdvisory() => ShowPage(new AdvisingPage(services, ViewReport, finish: Home), "Build your pathway", "Profile → Subjects → Interests → Programme → Advisory outcome");
    private void NewFromSession(int id)
    {
        var session = services.Advising.Load(id);
        ShowPage(new AdvisingPage(services, ViewReport, session, Home), "Follow-up advisory session", "Review the copied data, update it, and save a new assessment. The previous assessment is preserved.");
    }

    private void Login()
    {
        if (!CanLeave())
            return;
        using var dialog = new LoginDialog(services);
        if (dialog.ShowDialog(this) == DialogResult.OK)
        {
            RefreshNavigation();
            ClearPage();
            Home();
        }
    }

    private void Logout()
    {
        if (!CanLeave() || !Ui.Confirm("Sign out and clear this workspace from the screen? Saved sessions remain in the local database."))
            return;
        services.Access.SignOut();
        ClearPage();
        RefreshNavigation();
        Home();
    }

    private void ClearPage()
    {
        reportSource?.Dispose();
        reportSource = null;
        foreach (Control c in content.Controls.Cast<Control>().ToArray())
        {
            content.Controls.Remove(c);
            c.Dispose();
        }
    }

    private void ViewReport(ReportDocument report)
    {
        if (!CanLeave())
            return;
        string previousTitle = title.Text;
        string previousSubtitle = subtitle.Text;
        reportSource = content.Controls.Cast<Control>().FirstOrDefault();
        content.Controls.Clear();
        ReportViewer? viewer = null;
        viewer = new ReportViewer(services, report, () =>
        {
            content.Controls.Clear();
            viewer?.Dispose();
            Control? source = reportSource;
            reportSource = null;
            if (source != null)
                content.Controls.Add(source);
            title.Text = previousTitle;
            subtitle.Text = previousSubtitle;
        }, report.SessionId.HasValue ? Home : null);
        // Dispose the viewer on return while retaining the originating page's input and filters.
        title.Text = report.Title;
        subtitle.Text = report.SessionId.HasValue ? "Step 7 of 7 • Preview, export or print your saved summary." : "Preview, export or print the generated report.";
        content.Controls.Add(viewer);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
            reportSource?.Dispose();
        base.Dispose(disposing);
    }
}

internal sealed class LoginDialog : AppForm
{
    public LoginDialog(AppServices services)
    {
        Text = "Future Path | Staff sign in";
        Size = new Size(670, 530);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        var stack = Ui.Stack();
        Controls.Add(stack);
        stack.Controls.Add(Ui.Label("Welcome back", 25, FontStyle.Bold));
        stack.Controls.Add(Ui.Label("Career Advisors and Administrators sign in here.\nLearners can continue from the main window without an account."));
        var username = Ui.Input(400);
        var password = Ui.Input(400);
        password.UseSystemPasswordChar = true;
        password.MaxLength = 128;
        stack.Controls.Add(Ui.Field("Username", username));
        stack.Controls.Add(Ui.Field("Password", password));
        var show = new CheckBox
        {
            Text = "Show password",
            AutoSize = true
        };
        show.CheckedChanged += (_, _) => password.UseSystemPasswordChar = !show.Checked;
        stack.Controls.Add(show);
        var signIn = Ui.Button("Sign in", () =>
        {
            services.Authentication.Login(username.Text, password.Text);
            password.Clear();
            DialogResult = DialogResult.OK;
            Close();
        }, true);
        var cancel = Ui.Button("Cancel", Close);
        cancel.DialogResult = DialogResult.Cancel;
        stack.Controls.Add(Ui.Row(signIn, cancel));
        AcceptButton = signIn;
        CancelButton = cancel;
        stack.Controls.Add(Ui.Label("Demo staff credentials are listed in the README.\nFive failed attempts lock an account for five minutes.", 9));
    }
}

internal sealed class PasswordDialog : AppForm
{
    public PasswordDialog(AppServices services)
    {
        Text = "Change password";
        Size = new Size(660, 540);
        var stack = Ui.Stack();
        Controls.Add(stack);
        var current = Ui.Input(400);
        var next = Ui.Input(400);
        var confirm = Ui.Input(400);
        foreach (var box in new[]
        {
            current,
            next,
            confirm
        }

        )
        {
            box.UseSystemPasswordChar = true;
            box.MaxLength = 128;
        }

        stack.Controls.Add(Ui.Label("Change your staff password", 20, FontStyle.Bold));
        stack.Controls.Add(Ui.Field("Current password", current));
        stack.Controls.Add(Ui.Field("New password (12–128 characters, letter and number)", next));
        stack.Controls.Add(Ui.Field("Confirm new password", confirm));
        stack.Controls.Add(Ui.Button("Save password", () =>
        {
            if (next.Text != confirm.Text)
                throw new ValidationException("The new passwords do not match.");
            services.Authentication.ChangePassword(current.Text, next.Text);
            MessageBox.Show("Password updated.", "Future Path");
            Close();
        }, true));
    }
}
