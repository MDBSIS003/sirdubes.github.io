using System.Runtime.ExceptionServices;
using FuturePath.Business;
using FuturePath.Data;
using FuturePath.Desktop;

namespace FuturePath.Desktop.Tests;
// Compile on Linux; execute with Windows, WinForms and SQL Server LocalDB.
public class DesktopTests
{
    private static void Sta(Action action)
    {
        Exception? failure = null;
        Thread thread = new Thread(() =>
        {
            try { action(); }
            catch (Exception ex) { failure = ex; }
        });
        thread.IsBackground = true;
        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
        Assert.True(thread.Join(TimeSpan.FromSeconds(90)), "Windows Forms test timed out.");
        if (failure != null) { ExceptionDispatchInfo.Capture(failure).Throw(); }
    }
    private static IEnumerable<Control> Descendants(Control parent)
    {
        foreach (Control child in parent.Controls)
        {
            yield return child;
            foreach (Control nested in Descendants(child)) { yield return nested; }
        }
    }
    private static T Field<T>(Control parent, string name) where T : Control
    {
        return Descendants(parent).OfType<T>().Single(control => control.Name == name);
    }
    private static void WaitForOverview(frmMain main)
    {
        DateTime deadline = DateTime.UtcNow.AddSeconds(30);
        while (main.MdiChildren.OfType<frmWelcome>().Count() == 0 && DateTime.UtcNow < deadline)
        {
            Application.DoEvents();
            Thread.Sleep(20);
        }
        Assert.Single(main.MdiChildren.OfType<frmWelcome>());
    }
    private static void Next(frmMain main)
    {
        Assert.NotNull(main.ActiveMdiChild);
        Field<Button>(main.ActiveMdiChild!, "btnNext").PerformClick();
        Application.DoEvents();
    }
    [Fact]
    public void DesignerConstructorsDoNotConnectToDatabase() => Sta(() =>
    {
        Form[] forms = new Form[]
        {
            new frmMain(), new frmWelcome(), new frmDatabaseSetup(), new frmLearnerProfile(),
            new frmSubjectMarks(), new frmInterestQuestionnaire(), new frmDegreeEligibility(),
            new frmAdvisoryResults(), new frmFundingOptions(), new frmAdvisorySummary(),
            new frmLogin(), new frmSecurity(), new frmLearnerSessions(), new frmReports(),
            new frmReportPreview(), new frmAdminMaintenance(), new frmEditDegreeProgramme(),
            new frmEditProgrammeRequirement(), new frmEditInterestQuestion(), new frmEditDegreeInterestMapping(),
            new frmEditFundingOption(), new frmEditFundingFaculty(), new frmEditFundingProgramme(),
            new frmEditFaculty(), new frmEditSubject(), new frmEditInterestCategory(), new frmEditUserAccount()
        };
        foreach (Form form in forms)
        {
            using (form)
            {
                form.CreateControl();
                Assert.Equal("Segoe UI", form.Font.Name);
                Assert.NotEmpty(form.Controls.Cast<Control>());
            }
        }
    });
    [Fact]
    public void RadioQuestionKeepsOneAnswerAndConvertsEveryLikertValue() => Sta(() =>
    {
        InterestQuestion question = new InterestQuestion();
        question.Id = 7;
        question.Text = "A course questionnaire question";
        using QuestionRow row = new QuestionRow(question);
        Assert.Equal(7, row.QuestionId);
        Assert.Equal(0, row.Score);
        for (int score = 1; score <= 5; score++)
        {
            row.Score = score;
            Assert.Equal(score, row.Score);
            Assert.Single(Descendants(row).OfType<RadioButton>(), radio => radio.Checked);
        }
        row.Score = 0;
        Assert.DoesNotContain(Descendants(row).OfType<RadioButton>(), radio => radio.Checked);
    });
    [Theory]
    [InlineData(760, 520)]
    [InlineData(980, 640)]
    [InlineData(1280, 720)]
    public void StepFooterStaysVisibleWhenContentNeedsScrolling(int width, int height) => Sta(() =>
    {
        using Form host = new Form();
        host.ClientSize = new Size(width, height);
        using frmLearnerProfile page = new frmLearnerProfile();
        page.TopLevel = false;
        page.Dock = DockStyle.Fill;
        host.Controls.Add(page);
        host.Show();
        page.Show();
        Application.DoEvents();
        Button next = Field<Button>(page, "btnNext");
        Rectangle nextBounds = host.RectangleToClient(next.RectangleToScreen(next.ClientRectangle));
        Assert.True(host.ClientRectangle.Contains(nextBounds), "Next must remain inside the viewport.");
        Panel body = Field<Panel>(page, "pnlBody");
        Assert.True(body.AutoScroll);
        host.Close();
    });
    [Fact]
    public void SevenStepJourneyRetainsInputPreviewsThenSavesExactlyOnce() => Sta(() =>
    {
        using LocalDbDatabase db = new LocalDbDatabase();
        AppController app = new AppController(db.Store);
        using frmMain main = new frmMain(app);
        main.Show();
        WaitForOverview(main);
        int before = db.Store.FindSessions(DateOnly.MinValue, DateOnly.MaxValue, "").Count;
        main.StartAdvisory();
        Form page = main.ActiveMdiChild!;
        Field<TextBox>(page, "txtName").Text = "Course UI test";
        Field<TextBox>(page, "txtSchool").Text = "Sample school";
        Assert.False(Field<Button>(page, "btnNext").Enabled);
        Field<TextBox>(page, "txtEmail").Text = "learner@example.com";
        Field<TextBox>(page, "txtPhone").Text = "0712345678";
        Field<ComboBox>(page, "cboProvince").SelectedItem = "Western Cape";
        Field<ComboBox>(page, "cboGrade").SelectedItem = "12";
        Assert.True(Field<Button>(page, "btnNext").Enabled);
        Next(main);
        Assert.IsType<frmSubjectMarks>(main.ActiveMdiChild);
        main.GoBack();
        Assert.Equal("Course UI test", Field<TextBox>(main.ActiveMdiChild!, "txtName").Text);
        Next(main);
        page = main.ActiveMdiChild!;
        int[] subjects = { 1, 3, 7, 8, 9, 13, 2 };
        decimal[] marks = { 72, 70, 74, 79, 70, 80, 70 };
        for (int index = 0; index < subjects.Length; index++)
        {
            Field<ComboBox>(page, "cboSubject").SelectedValue = subjects[index];
            Field<NumericUpDown>(page, "nudMark").Value = marks[index];
            Field<Button>(page, "btnAdd").PerformClick();
        }
        // Updating the same subject must not add a duplicate row.
        Field<Button>(page, "btnAdd").PerformClick();
        Assert.Equal(7, Field<ListView>(page, "lstMarks").Items.Count);
        Assert.False(Field<Button>(page, "btnNext").Enabled);
        Field<CheckBox>(page, "chkComplete").Checked = true;
        Next(main);
        Assert.IsType<frmInterestQuestionnaire>(main.ActiveMdiChild);
        Assert.DoesNotContain(Descendants(main.ActiveMdiChild!), control => control.Name == "chkIncomplete");
        Assert.False(Field<Button>(main.ActiveMdiChild!, "btnNext").Enabled);
        foreach (QuestionRow row in Descendants(main.ActiveMdiChild!).OfType<QuestionRow>()) { row.Score = 5; }
        Assert.True(Field<Button>(main.ActiveMdiChild!, "btnNext").Enabled);
        Next(main);
        Assert.IsType<frmDegreeEligibility>(main.ActiveMdiChild);
        Field<ComboBox>(main.ActiveMdiChild!, "cboFaculty").SelectedValue = 1;
        Field<ComboBox>(main.ActiveMdiChild!, "cboDegree").SelectedValue = 1;
        Assert.IsType<Label>(Field<Label>(main.ActiveMdiChild!, "lblRequirements"));
        Assert.DoesNotContain(Descendants(main.ActiveMdiChild!).OfType<TextBox>(), control => control.Name == "txtRequirements");
        Next(main);
        Assert.IsType<frmAdvisoryResults>(main.ActiveMdiChild);
        Assert.NotEmpty(Field<Label>(main.ActiveMdiChild!, "lblOutcomeDetails").Text);
        Assert.Equal(AdvisoryOutcome.StrongMatch, main.Preview!.Selected.Outcome);
        Assert.Equal(before, db.Store.FindSessions(DateOnly.MinValue, DateOnly.MaxValue, "").Count);
        Next(main);
        Assert.IsType<frmFundingOptions>(main.ActiveMdiChild);
        Field<CheckBox>(main.ActiveMdiChild!, "chkFunding").Checked = true;
        Next(main);
        Assert.IsType<frmAdvisorySummary>(main.ActiveMdiChild);
        Assert.NotNull(main.SavedSession);
        Assert.False(Field<Button>(main.ActiveMdiChild!, "btnBack").Enabled);
        Assert.Contains("Course UI test", Field<Label>(main.ActiveMdiChild!, "lblSummary").Text);
        Assert.Equal(before + 1, db.Store.FindSessions(DateOnly.MinValue, DateOnly.MaxValue, "").Count);
        Assert.False(Field<Button>(main.ActiveMdiChild!, "btnEnd").Visible);
        Assert.DoesNotContain("Rules:", Field<Label>(main.ActiveMdiChild!, "lblSummary").Text);
        Assert.Equal(1, Application.OpenForms.Cast<Form>().Count(form => !form.IsMdiChild));
        Next(main);
        Assert.IsType<frmWelcome>(main.ActiveMdiChild);
        main.Close();
    });
    [Fact]
    public void StaffEditorsBindRealRecordsAndRoleChecksRemainInBusinessTier() => Sta(() =>
    {
        using LocalDbDatabase db = new LocalDbDatabase();
        AppController app = new AppController(db.Store);
        Assert.Throws<AccessDeniedException>(() => app.Maintenance.Rows("Subject"));
        app.Authentication.Login("SIRDUBES", "MDBSI003");
        using (frmReports reports = new frmReports(app)) { reports.CreateControl(); }
        Assert.Throws<AccessDeniedException>(() => new frmAdminMaintenance(app));
        app.Authentication.Login("Adonias", "NKSADO001");
        using (frmAdminMaintenance admin = new frmAdminMaintenance(app)) { admin.CreateControl(); }
        ReferenceRow user = app.Maintenance.Rows("UserAccount").First();
        using (frmEditUserAccount editor = new frmEditUserAccount(app, user))
        {
            Assert.NotNull(Field<ComboBox>(editor, "cboRole").SelectedItem);
            Assert.Equal("", Field<TextBox>(editor, "txtPassword").Text);
        }
        using (frmEditDegreeProgramme editor = new frmEditDegreeProgramme(app, app.Maintenance.Rows("DegreeProgramme").First()))
        { Assert.NotNull(Field<ComboBox>(editor, "cboFacultyId").SelectedItem); }
        using (frmEditFundingOption editor = new frmEditFundingOption(app, app.Maintenance.Rows("FundingOption").First()))
        { Assert.NotNull(Field<ComboBox>(editor, "cboType").SelectedItem); }
    });
    [Theory]
    [InlineData(760, 520)]
    [InlineData(980, 640)]
    [InlineData(1280, 720)]
    public void RequirementsAndResultsUseCompleteWrappingDisplays(int width, int height) => Sta(() =>
    {
        foreach (frmAdvisoryStep page in new frmAdvisoryStep[] { new frmDegreeEligibility(), new frmAdvisoryResults(), new frmAdvisorySummary(), new frmFundingOptions() })
        {
            using (page)
            using (Form host = new Form())
            {
                host.ClientSize = new Size(width, height);
                page.TopLevel = false;
                page.Dock = DockStyle.Fill;
                host.Controls.Add(page);
                host.Show(); page.Show(); Application.DoEvents();
                string name = page is frmDegreeEligibility ? "lblRequirements" : page is frmAdvisoryResults ? "lblOutcomeDetails" : page is frmFundingOptions ? "lblFunding" : "lblSummary";
                Label display = Field<Label>(page, name);
                display.Text = string.Join(Environment.NewLine + Environment.NewLine, Enumerable.Repeat("Required: Mathematics at 60%. Improve marks and review the full programme requirements.", 20));
                Application.DoEvents();
                Assert.True(display.AutoSize);
                Assert.True(display.MaximumSize.Width > 0);
                Assert.True(display.Height >= display.GetPreferredSize(new Size(display.MaximumSize.Width,0)).Height);
                Assert.Empty(Descendants(page).OfType<TextBox>());
                Button next = Field<Button>(page,"btnNext");
                Assert.True(host.ClientRectangle.Contains(host.RectangleToClient(next.RectangleToScreen(next.ClientRectangle))));
                host.Close();
            }
        }
    });
    [Fact]
    public void StaffLoginExposesCorrectWorkspaceActions() => Sta(() =>
    {
        using LocalDbDatabase db = new();
        AppController app = new(db.Store);
        using frmLogin login = new(app);
        login.Show();
        Assert.Single(Descendants(login).OfType<Button>());
        Assert.Equal("Sign in", Descendants(login).OfType<Button>().Single().Text);
        Field<TextBox>(login,"txtUsername").Text = "SIRDUBES";
        Field<TextBox>(login,"txtPassword").Text = "MDBSI003";
        Field<Button>(login,"btnLogin").PerformClick();
        Assert.Equal(DialogResult.OK,login.DialogResult);
        Assert.Equal(UserRole.Advisor,app.Access.Role);
        login.Close();
        using frmMain main = new(app);
        main.Show(); WaitForOverview(main);
        Form welcome = main.MdiChildren.OfType<frmWelcome>().Single();
        Assert.True(Field<Button>(welcome,"btnReports").Visible);
        Assert.False(Field<Button>(welcome,"btnAdministration").Visible);
        app.Authentication.Login("Adonias","NKSADO001");
        main.ShowOverview();
        welcome = main.MdiChildren.OfType<frmWelcome>().Single();
        Assert.True(Field<Button>(welcome,"btnAdministration").Visible);
        main.Close();
    });

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void SecurityFormSavesUsernameAndOptionalPassword(bool replacePassword) => Sta(() =>
    {
        using LocalDbDatabase db = new();
        AppController app = new(db.Store);
        app.Authentication.Login("SIRDUBES","MDBSI003");
        using frmSecurity form = new(app);
        form.Show();
        Assert.Equal("SIRDUBES",Field<TextBox>(form,"txtUsername").Text);
        Field<TextBox>(form,"txtUsername").Text="qa.advisor";
        Field<TextBox>(form,"txtCurrent").Text="MDBSI003";
        if (replacePassword)
        {
            Field<TextBox>(form,"txtNew").Text="ReplacementPass2026!";
            Field<TextBox>(form,"txtConfirm").Text="ReplacementPass2026!";
        }
        Field<Button>(form,"btnSave").PerformClick();
        Assert.Equal(DialogResult.OK,form.DialogResult);
        Assert.Equal("qa.advisor",app.Access.DisplayName);
        app.Access.SignOut();
        app.Authentication.Login("qa.advisor",replacePassword ? "ReplacementPass2026!" : "MDBSI003");
        Assert.Equal(UserRole.Advisor,app.Access.Role);
        form.Close();
    });

    [Theory]
    [InlineData(UserRole.Advisor)]
    [InlineData(UserRole.Administrator)]
    public void StaffAccountEditorCreatesUsableMarkerCredentials(UserRole role) => Sta(() =>
    {
        using LocalDbDatabase db = new();
        AppController app = new(db.Store);
        app.Authentication.Login("Adonias","NKSADO001");
        using frmEditUserAccount editor = new(app,null);
        editor.Show();
        Field<TextBox>(editor,"txtUsername").Text="marker.account";
        Field<ComboBox>(editor,"cboRole").SelectedItem=role;
        Field<TextBox>(editor,"txtPassword").Text="MarkerPassword2026!";
        Field<TextBox>(editor,"txtConfirm").Text="MarkerPassword2026!";
        Field<Button>(editor,"btnSave").PerformClick();
        Assert.Equal(DialogResult.OK,editor.DialogResult);
        Assert.Equal("marker.account",editor.AccountUsername);
        app.Access.SignOut();
        app.Authentication.Login("marker.account","MarkerPassword2026!");
        Assert.Equal(role,app.Access.Role);
        editor.Close();
    });

}
