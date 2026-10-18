using System.Runtime.ExceptionServices;
using FuturePath.Business;
using FuturePath.Data;
using FuturePath.Desktop;

namespace FuturePath.Desktop.Tests;
/// <summary>These tests require Windows; they are compiled on Linux and executed by the Windows CI workflow.</summary>
public sealed class DesktopTests
{
    private static void Sta(Action action)
    {
        Exception? failure = null;
        var thread = new Thread(() =>
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                failure = ex;
            }
        })
        {
            IsBackground = true
        };
        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
        Assert.True(thread.Join(TimeSpan.FromSeconds(60)), "Windows Forms test timed out.");
        if (failure != null)
            ExceptionDispatchInfo.Capture(failure).Throw();
    }

    private static IEnumerable<Control> Descendants(Control control)
    {
        foreach (Control child in control.Controls)
        {
            yield return child;
            foreach (var nested in Descendants(child))
                yield return nested;
        }
    }

    private static T Field<T>(Control parent, string name)
        where T : Control => Descendants(parent).OfType<T>().Single(c => c.AccessibleName == name);
    private static void WithApp(Action<AppServices> action)
    {
        using var database = new LocalDbDatabase();
        action(new AppServices(database.Store));
    }

    [Theory]
    [InlineData(960)]
    [InlineData(1280)]
    [InlineData(1600)]
    public void OverviewFitsTheViewportAndKeepsNavigationReadableAfterResize(int logicalWidth) => Sta(() =>
    {
        // The overview has no database dependency. Exercise real WinForms layout at the host's DPI.
        using var main = new MainForm(new AppServices(new SqlServerStore()));
        main.Show();
        main.MinimumSize = Size.Empty;
        double scale = main.DeviceDpi / 96d;
        main.ClientSize = new Size((int)(logicalWidth * scale), (int)(720 * scale));
        Application.DoEvents();

        var brand = Descendants(main).OfType<Label>().Single(l => l.Text == "FUTURE\nPATH");
        var navigation = (FlowLayoutPanel)brand.Parent!;
        int lineHeight = TextRenderer.MeasureText("FUTURE", brand.Font).Height;
        Assert.True(brand.Height <= lineHeight * 2 + (int)(8 * scale), "The two-line brand wrapped into additional lines.");
        Assert.True(brand.Right <= navigation.ClientSize.Width - navigation.Padding.Right);
        foreach (var button in navigation.Controls.OfType<Button>())
        {
            Assert.True(button.Right <= navigation.ClientSize.Width - navigation.Padding.Right);
            Assert.True(button.Height >= button.GetPreferredSize(new Size(button.Width, 0)).Height);
        }

        var cards = Descendants(main).OfType<ContentCard>().ToList();
        Assert.Equal(4, cards.Count);
        foreach (var card in cards)
        {
            var stack = (PageStack)card.Parent!;
            Assert.True(card.Width <= stack.ClientSize.Width - stack.Padding.Horizontal, "A card overflowed its viewport.");
            Assert.False(stack.HorizontalScroll.Visible);
        }
        main.Close();
    });

    [Fact]
    public void FormsConstructWithRealBindingsForEachRole() => Sta(() => WithApp(app =>
    {
        using var main = new MainForm(app);
        main.CreateControl();
        using var learner = new AdvisingPage(app, _ =>
        {
        });
        learner.CreateControl();
        Assert.Equal(5, learner.StepCount);
        Assert.Empty(Descendants(learner).OfType<TabControl>());
        app.Authentication.Login("advisor_test", "FuturePathAdvisor2026!");
        using var advisor = new AdvisorPage(app, _ =>
        {
        }, _ =>
        {
        });
        advisor.CreateControl();
        using var reports = new ReportsPage(app, _ =>
        {
        });
        reports.CreateControl();
        app.Authentication.Login("admin_test", "FuturePathAdmin2026!");
        using var admin = new MaintenancePage(app);
        admin.CreateControl();
        foreach (var definition in ReferenceCatalog.Definitions)
        {
            using var editor = new ReferenceEditor(app, definition, null, () =>
            {
            }, () =>
            {
            });
            editor.CreateControl();
            Assert.Contains(Descendants(editor).OfType<Button>(), b => b.Text == "Save changes");
        }
    }));
    [Fact]
    public void LearnerFormCapturesDataSavesAssessmentAndBuildsSummary() => Sta(() => WithApp(app =>
    {
        using var page = new AdvisingPage(app, _ =>
        {
        });
        using var host = new Form();
        host.Controls.Add(page);
        host.Show();
        Application.DoEvents();
        page.GoNext();
        Assert.Equal(1, page.CurrentStep);
        var subjects = Field<ComboBox>(page, "Subject *");
        var mark = Field<NumericUpDown>(page, "Mark (%) *");
        foreach (var(id, value)in new[]
        {
            (1, 72),
            (3, 70),
            (7, 74),
            (8, 79),
            (9, 70),
            (13, 80)
        }

        )
        {
            subjects.SelectedValue = id;
            mark.Value = value;
            Descendants(page).OfType<Button>().Single(b => b.Text == "Add subject").PerformClick();
        }

        Descendants(page).OfType<CheckBox>().Single(c => c.Text.StartsWith("I confirm")).Checked = true;
        page.GoNext();
        foreach (var combo in Descendants(page).OfType<ComboBox>().Where(c => c.AccessibleName?.StartsWith("I enjoy") == true))
            combo.SelectedValue = 5;
        page.GoNext();
        Field<ComboBox>(page, "Preferred faculty *").SelectedValue = 1;
        var degrees = Field<ComboBox>(page, "Degree programme *");
        degrees.SelectedItem = degrees.Items.Cast<DegreeProgramme>().Single(p => p.Id == 1);
        page.SubmitAssessment();
        Assert.False(page.HasUnsavedInput);
        Assert.Equal(4, page.CurrentStep);
        Assert.Contains(Descendants(page).OfType<Label>(), l => l.Text == "Strong Match");
        page.GoBack();
        Assert.Equal(4, page.CurrentStep);
        host.Close();
    }));
    [Fact]
    public void BackNextRetainInputValidateAndDoNotSaveBeforeConfirmation() => Sta(() => WithApp(app =>
    {
        using var host = new Form();
        using var page = new AdvisingPage(app, _ =>
        {
        });
        host.Controls.Add(page);
        host.Show();
        Application.DoEvents();
        int windows = Application.OpenForms.Count;
        Field<TextBox>(page, "Name / alias (optional)").Text = "Retained learner";
        page.GoNext();
        page.GoBack();
        Assert.Equal("Retained learner", Field<TextBox>(page, "Name / alias (optional)").Text);
        page.GoNext();
        Assert.Throws<ValidationException>(() => page.GoNext());
        Assert.Equal(1, page.CurrentStep);
        Assert.Equal(windows, Application.OpenForms.Count);
        app.Authentication.Login("advisor_test", "FuturePathAdvisor2026!");
        Assert.Equal(10, app.Advising.Search(DateOnly.MinValue, DateOnly.MaxValue).Count);
        page.Dispose();
        host.Close();
    }));
    [Fact]
    public void ReportAndMaintenanceEditorsAreControlsInsideTheSameWindow() => Sta(() => WithApp(app =>
    {
        app.Authentication.Login("admin_test", "FuturePathAdmin2026!");
        using var host = new Form();
        host.Show();
        int windows = Application.OpenForms.Count;
        using var viewer = new ReportViewer(app, app.Reports.Summary(1), () =>
        {
        });
        host.Controls.Add(viewer);
        Application.DoEvents();
        Assert.Equal(windows, Application.OpenForms.Count);
        host.Controls.Remove(viewer);
        using var editor = new ReferenceEditor(app, ReferenceCatalog.Get("Subject"), null, () =>
        {
        }, () =>
        {
        });
        host.Controls.Add(editor);
        Application.DoEvents();
        Assert.Equal(windows, Application.OpenForms.Count);
        host.Close();
    }));
}
