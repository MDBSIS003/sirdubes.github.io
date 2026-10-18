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
            new frmLogin(), new frmPassword(), new frmLearnerSessions(), new frmReports(),
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
        Field<ComboBox>(page, "cboProvince").SelectedItem = "Western Cape";
        Field<ComboBox>(page, "cboGrade").SelectedItem = "12";
        Assert.True(Field<Button>(page, "btnNext").Enabled);
        Next(main);
        Assert.IsType<frmSubjectMarks>(main.ActiveMdiChild);
        main.GoBack();
        Assert.Equal("Course UI test", Field<TextBox>(main.ActiveMdiChild!, "txtName").Text);
        Next(main);
        page = main.ActiveMdiChild!;
        int[] subjects = { 1, 3, 7, 8, 9, 13 };
        decimal[] marks = { 72, 70, 74, 79, 70, 80 };
        for (int index = 0; index < subjects.Length; index++)
        {
            Field<ComboBox>(page, "cboSubject").SelectedValue = subjects[index];
            Field<NumericUpDown>(page, "nudMark").Value = marks[index];
            Field<Button>(page, "btnAdd").PerformClick();
        }
        // Updating the same subject must not add a duplicate row.
        Field<Button>(page, "btnAdd").PerformClick();
        Assert.Equal(6, Field<ListView>(page, "lstMarks").Items.Count);
        Field<CheckBox>(page, "chkComplete").Checked = true;
        Next(main);
        Assert.IsType<frmInterestQuestionnaire>(main.ActiveMdiChild);
        foreach (QuestionRow row in Descendants(main.ActiveMdiChild!).OfType<QuestionRow>()) { row.Score = 5; }
        Next(main);
        Assert.IsType<frmDegreeEligibility>(main.ActiveMdiChild);
        Field<ComboBox>(main.ActiveMdiChild!, "cboFaculty").SelectedValue = 1;
        Field<ComboBox>(main.ActiveMdiChild!, "cboDegree").SelectedValue = 1;
        Next(main);
        Assert.IsType<frmAdvisoryResults>(main.ActiveMdiChild);
        Assert.Equal(AdvisoryOutcome.StrongMatch, main.Preview!.Selected.Outcome);
        Assert.Equal(before, db.Store.FindSessions(DateOnly.MinValue, DateOnly.MaxValue, "").Count);
        Next(main);
        Assert.IsType<frmFundingOptions>(main.ActiveMdiChild);
        Field<CheckBox>(main.ActiveMdiChild!, "chkFunding").Checked = true;
        Next(main);
        Assert.IsType<frmAdvisorySummary>(main.ActiveMdiChild);
        Assert.NotNull(main.SavedSession);
        Assert.False(Field<Button>(main.ActiveMdiChild!, "btnBack").Enabled);
        Assert.Contains("Course UI test", Field<TextBox>(main.ActiveMdiChild!, "txtSummary").Text);
        Assert.Equal(before + 1, db.Store.FindSessions(DateOnly.MinValue, DateOnly.MaxValue, "").Count);
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
        app.Authentication.Login("advisor_test", "FuturePathAdvisor2026!");
        using (frmReports reports = new frmReports(app)) { reports.CreateControl(); }
        Assert.Throws<AccessDeniedException>(() => new frmAdminMaintenance(app));
        app.Authentication.Login("admin_test", "FuturePathAdmin2026!");
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
}
