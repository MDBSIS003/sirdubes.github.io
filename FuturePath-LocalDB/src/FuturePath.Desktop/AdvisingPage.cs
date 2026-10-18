using System.ComponentModel;
using FuturePath.Business;

namespace FuturePath.Desktop;
internal sealed class AdvisingPage : AppPage
{
    private readonly AppServices services;
    private readonly Action<ReportDocument> showReport;
    private readonly Panel stepContent = new()
    {
        Dock = DockStyle.Fill
    };
    private readonly List<FlowLayoutPanel> steps = new();
    private readonly Dictionary<int, AppPage> stepHosts = new();
    private readonly Label progress = Ui.Label("", 11, FontStyle.Bold);
    private readonly Button back;
    private readonly Button next;
    private readonly Action finish;
    private int currentStep;
    private AdvisorySession? savedSession;
    private readonly BindingList<SubjectMark> marks = [];
    private readonly List<(InterestQuestion Question, ComboBox Answer)> answers = [];
    private readonly TextBox name = Ui.Input();
    private readonly TextBox school = Ui.Input();
    private readonly TextBox email = Ui.Input();
    private readonly TextBox phone = Ui.Input();
    private readonly ComboBox grade = Ui.Combo([new() { Id = 11, Name = "Grade 11 — current / projected marks" }, new() { Id = 12, Name = "Grade 12 — latest / final marks" }], 12, 340);
    private readonly ComboBox province = Ui.Choice(new[] { "" }.Concat(ReferenceCatalog.Provinces), 340);
    private readonly ComboBox faculty;
    private readonly ComboBox degree = new()
    {
        DropDownStyle = ComboBoxStyle.DropDownList,
        Width = 730,
        DisplayMember = "Name",
        Font = Ui.Body(11)
    };
    private readonly TextBox requirementText = new()
    {
        Multiline = true,
        ReadOnly = true,
        Width = 760,
        Height = 220,
        ScrollBars = ScrollBars.Vertical,
        BackColor = Color.White,
        BorderStyle = BorderStyle.FixedSingle
    };
    private readonly CheckBox complete = new()
    {
        Text = "I confirm that ALL my current subjects and marks are listed.",
        AutoSize = true
    };
    private readonly CheckBox funding = new()
    {
        Text = "Yes, show funding information linked to my selected programme / faculty.",
        AutoSize = true
    };
    private readonly FlowLayoutPanel resultStack = Ui.Stack();
    private readonly int existingLearnerId;
    private bool isSaved;
    public bool HasUnsavedInput { get; private set; }

    public AdvisingPage(AppServices services, Action<ReportDocument> showReport, AdvisorySession? followUp = null, Action? finish = null)
    {
        this.services = services;
        this.showReport = showReport;
        this.finish = finish ?? (() =>
        {
        });
        BackColor = Ui.Background;
        back = Ui.Button("← Back", GoBack);
        next = Ui.Button("Next →", GoNext, true);
        var footer = new FlowLayoutPanel
        {
            Dock = DockStyle.Bottom,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            Padding = new Padding(20, 10, 20, 10),
            BackColor = Color.White
        };
        footer.Controls.AddRange([progress, back, next]);
        Controls.Add(stepContent);
        Controls.Add(footer);
        faculty = Ui.Combo(services.Advising.Faculties(), width: 380);
        AddProfile();
        AddSubjects();
        AddInterests();
        AddSelection();
        resultStack.Name = "Your outcome";
        steps.Add(resultStack);
        resultStack.Controls.Add(Ui.Label("Your assessment will appear here", 20, FontStyle.Bold));
        resultStack.Controls.Add(Ui.Label("Complete the earlier steps, then select ‘Check eligibility & save’. Missing information produces an explanation rather than an admission decision."));
        faculty.SelectedIndexChanged += (_, _) => RefreshDegrees();
        degree.SelectedIndexChanged += (_, _) => RefreshRequirements();
        RefreshDegrees();
        if (followUp != null)
        {
            var input = followUp.Input;
            var l = input.Learner;
            existingLearnerId = l.Id;
            name.Text = l.Name;
            school.Text = l.School;
            email.Text = l.Email;
            phone.Text = l.Phone;
            province.SelectedItem = l.Province;
            grade.SelectedValue = l.Grade;
            // A follow-up preserves the learner's identity and creates a new immutable assessment.
            foreach (var box in new[]
            {
                name,
                school,
                email,
                phone
            }

            )
                box.ReadOnly = true;
            province.Enabled = false;
            var currentSubjects = services.Advising.Subjects().Select(s => s.Id).ToHashSet();
            foreach (var mark in input.Marks.Where(m => currentSubjects.Contains(m.SubjectId)))
                marks.Add(new SubjectMark { SubjectId = mark.SubjectId, SubjectName = mark.SubjectName, Mark = mark.Mark });
            foreach (var answer in answers)
            {
                var response = input.Responses.FirstOrDefault(r => r.QuestionId == answer.Question.Id);
                if (response != null)
                    answer.Answer.SelectedValue = response.Score;
            }

            faculty.SelectedValue = input.PreferredFacultyId;
            if (degree.DataSource is List<DegreeProgramme> list && list.Any(p => p.Id == input.DegreeId))
                degree.SelectedItem = list.Single(p => p.Id == input.DegreeId);
            complete.Checked = input.SubjectsComplete && marks.Count == input.Marks.Count;
            funding.Checked = input.FundingRequested;
        }

        foreach (var box in new[]
        {
            name,
            school,
            email,
            phone
        }

        )
            box.TextChanged += (_, _) => Dirty();
        foreach (var combo in new[]
        {
            grade,
            province,
            faculty,
            degree
        }.Concat(answers.Select(a => a.Answer)))
            combo.SelectedIndexChanged += (_, _) => Dirty();
        complete.CheckedChanged += (_, _) => Dirty();
        funding.CheckedChanged += (_, _) => Dirty();
        HasUnsavedInput = followUp != null;
        ShowStep(0);
        FinishLayout();
    }

    private void Dirty()
    {
        if (!isSaved)
            HasUnsavedInput = true;
    }

    private FlowLayoutPanel Tab(string label)
    {
        var stack = Ui.Stack();
        stack.Name = label;
        steps.Add(stack);
        return stack;
    }

    internal int CurrentStep => currentStep;
    internal int StepCount => steps.Count;

    private void ShowStep(int index)
    {
        currentStep = index;
        // Remove, do not dispose: the same controls retain unsaved input when Back is clicked.
        stepContent.Controls.Clear();
        if (!stepHosts.TryGetValue(index, out var host))
        {
            host = new AppPage { Dock = DockStyle.Fill };
            host.Controls.Add(steps[index]);
            host.FinishLayout();
            stepHosts.Add(index, host);
        }
        stepContent.Controls.Add(host);
        progress.Text = $"Step {index + 2} of 7  •  {steps[index].Name}     ";
        back.Enabled = index > 0 && !isSaved;
        next.Text = index == 3 ? "Check eligibility & save" : index == 4 ? "View advisory summary →" : "Next →";
    }

    internal void GoBack()
    {
        if (!isSaved && currentStep > 0)
            ShowStep(currentStep - 1);
    }

    internal void GoNext()
    {
        if (currentStep == 0)
            AdvisingValidation.Profile(ReadProfile());
        if (currentStep == 1)
            AdvisingValidation.Marks(marks.ToList(), services.Advising.Subjects());
        if (currentStep == 2)
            AdvisingValidation.Interests(ReadResponses(), services.Advising.Questions(), requireComplete: true);
        if (currentStep < 3)
            ShowStep(currentStep + 1);
        else if (currentStep == 3)
            SubmitAssessment();
        else if (savedSession != null)
            showReport(services.Reports.Summary(savedSession.Id));
    }

    private LearnerProfile ReadProfile() => new LearnerProfile
    {
        Id = existingLearnerId,
        Name = name.Text,
        Grade = Ui.SelectedId(grade),
        School = school.Text,
        Province = province.Text,
        Email = email.Text,
        Phone = phone.Text
    };
    private List<InterestResponse> ReadResponses()
    {
        var responses = new List<InterestResponse>();
        foreach (var answer in answers)
        {
            int score = Ui.SelectedId(answer.Answer);
            if (score > 0)
                responses.Add(new InterestResponse { QuestionId = answer.Question.Id, Score = score });
        }

        return responses;
    }

    private void AddProfile()
    {
        var stack = Tab("1   Profile");
        stack.Controls.Add(Ui.Label("Let’s start with you", 23, FontStyle.Bold));
        stack.Controls.Add(Ui.Label("Grade is required. Name, school and contact details are optional.\nAvoid entering ID numbers or other sensitive information. Entries are saved when you check eligibility."));
        name.MaxLength = 120;
        school.MaxLength = 120;
        phone.MaxLength = 20;
        var card = Ui.Card("Learner profile", "Use an alias or leave your name blank if you prefer.");
        card.Controls.Add(Ui.Row(Ui.Field("Name / alias (optional)", name), Ui.Field("Current grade *", grade)));
        card.Controls.Add(Ui.Row(Ui.Field("School (optional)", school), Ui.Field("Province (optional)", province)));
        card.Controls.Add(Ui.Row(Ui.Field("Email (optional)", email), Ui.Field("Phone (optional)", phone)));
        stack.Controls.Add(card);
    }

    private void AddSubjects()
    {
        var stack = Tab("2   Subjects & marks");
        stack.Controls.Add(Ui.Label("Build your academic picture", 23, FontStyle.Bold));
        stack.Controls.Add(Ui.Label("Enter every subject on your current school report. Marks range from 0 to 100; decimals are accepted.\nSelect a row to update or remove an existing entry. Duplicate subjects are rejected."));
        var subject = Ui.Combo(services.Advising.Subjects(), width: 430);
        var mark = new NumericUpDown
        {
            Minimum = 0,
            Maximum = 100,
            DecimalPlaces = 2,
            Width = 150
        };
        stack.Controls.Add(Ui.Row(Ui.Field("Subject *", subject), Ui.Field("Mark (%) *", mark)));
        var grid = Ui.Grid();
        grid.Dock = DockStyle.None;
        grid.Width = 800;
        grid.Height = 235;
        grid.AutoGenerateColumns = false;
        grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(SubjectMark.SubjectName), HeaderText = "Subject", FillWeight = 75 });
        grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(SubjectMark.Mark), HeaderText = "Mark (%)", FillWeight = 25 });
        grid.DataSource = marks;
        grid.SelectionChanged += (_, _) =>
        {
            if (grid.CurrentRow?.DataBoundItem is SubjectMark row)
            {
                subject.SelectedValue = row.SubjectId;
                mark.Value = row.Mark;
            }
        };
        stack.Controls.Add(Ui.Row(Ui.Button("Add subject", () =>
        {
            var id = Ui.SelectedId(subject);
            if (id == 0)
                throw new ValidationException("Choose a subject.");
            if (marks.Any(m => m.SubjectId == id))
                throw new ValidationException("That subject is already listed. Select its row and choose Update selected.");
            marks.Add(new SubjectMark { SubjectId = id, SubjectName = subject.Text, Mark = mark.Value });
            Dirty();
        }, true), Ui.Button("Update selected", () =>
        {
            if (grid.CurrentRow?.DataBoundItem is not SubjectMark row)
                throw new ValidationException("Select a subject row first.");
            if (marks.Any(m => m != row && m.SubjectId == Ui.SelectedId(subject)))
                throw new ValidationException("That subject already exists in another row.");
            row.SubjectId = Ui.SelectedId(subject);
            row.SubjectName = subject.Text;
            row.Mark = mark.Value;
            marks.ResetBindings();
            Dirty();
        }), Ui.Button("Remove selected", () =>
        {
            if (grid.CurrentRow?.DataBoundItem is SubjectMark row && Ui.Confirm("Remove " + row.SubjectName + " from this subject list?"))
            {
                marks.Remove(row);
                Dirty();
            }
        })));
        stack.Controls.Add(grid);
        stack.Controls.Add(complete);
        stack.Controls.Add(Ui.Label("Leave the confirmation unticked if you still need to enter marks. Your result will explain what information is missing.", 9));
    }

    private void AddInterests()
    {
        var stack = Tab("3   Interests");
        stack.Controls.Add(Ui.Label("What do you enjoy?", 23, FontStyle.Bold));
        stack.Controls.Add(Ui.Label("Choose one response for each statement. There are no right or wrong answers.\nScores are calculated locally from 0% (strongly disagree) to 100% (strongly agree)."));
        var options = new[]
        {
            new LookupItem
            {
                Id = 0,
                Name = "Choose a response…"
            },
            new()
            {
                Id = 1,
                Name = "1 — Strongly disagree"
            },
            new()
            {
                Id = 2,
                Name = "2 — Disagree"
            },
            new()
            {
                Id = 3,
                Name = "3 — Neutral"
            },
            new()
            {
                Id = 4,
                Name = "4 — Agree"
            },
            new()
            {
                Id = 5,
                Name = "5 — Strongly agree"
            }
        };
        foreach (var question in services.Advising.Questions())
        {
            var combo = Ui.Combo(options, 0, 240);
            var label = Ui.Label(question.Text);
            label.Width = 490;
            label.AutoSize = false;
            label.Height = 42;
            label.AccessibleName = question.Category;
            combo.AccessibleName = question.Text;
            stack.Controls.Add(Ui.Row(label, combo));
            answers.Add((question, combo));
        }
    }

    private void AddSelection()
    {
        var stack = Tab("4   Programme");
        stack.Controls.Add(Ui.Label("Explore your preferred pathway", 23, FontStyle.Bold));
        stack.Controls.Add(Ui.Field("Preferred faculty *", faculty));
        stack.Controls.Add(Ui.Field("Degree programme *", degree));
        stack.Controls.Add(requirementText);
        stack.Controls.Add(funding);
        stack.Controls.Add(Ui.Label("Only active degrees are selectable. These are sample rules, not current official university admission criteria.", 9));
        stack.Controls.Add(Ui.Label("Use Check eligibility & save below. No assessment is written until you confirm this action.", 9));
    }

    private void RefreshDegrees()
    {
        degree.DataSource = services.Advising.Programmes().Where(d => d.FacultyId == Ui.SelectedId(faculty)).ToList();
        RefreshRequirements();
    }

    private void RefreshRequirements()
    {
        if (degree.SelectedItem is not DegreeProgramme p)
        {
            requirementText.Text = "No active programmes are available in this faculty.";
            return;
        }

        var lines = new List<string>
        {
            p.Description,
            "",
            "SAMPLE ADMISSION REQUIREMENTS"
        };
        foreach (var r in p.Requirements.Where(r => r.AlternativeGroup == 0))
            lines.Add($"{(r.Mandatory ? "Required" : "Helpful")}: {r.Subject} ≥ {r.MinimumMark:0.##}%");
        foreach (var g in p.Requirements.Where(r => r.AlternativeGroup > 0).GroupBy(r => r.AlternativeGroup))
            lines.Add("Required choice: " + string.Join(" OR ", g.Select(r => $"{r.Subject} ≥ {r.MinimumMark:0.##}%")));
        if (p.MinimumAverage > 0)
            lines.Add($"Overall average ≥ {p.MinimumAverage:0.##}% (all captured subjects).");
        lines.Add("Strong match also requires interest alignment of at least 60%. A mandatory mark gap of up to 5 percentage points is a Possible Match; it does not mean requirements are met.");
        requirementText.Lines = lines.ToArray();
    }

    internal void SubmitAssessment()
    {
        if (isSaved)
            return;
        if (degree.SelectedItem is not DegreeProgramme p)
            throw new ValidationException("Choose an active programme.");
        var input = new AdvisoryInput
        {
            Learner = ReadProfile(),
            DegreeId = p.Id,
            PreferredFacultyId = Ui.SelectedId(faculty),
            SubjectsComplete = complete.Checked,
            FundingRequested = funding.Checked,
            Marks = marks.Select(m => new SubjectMark { SubjectId = m.SubjectId, Mark = m.Mark }).ToList(),
            Responses = ReadResponses()
        };
        var session = services.Advising.Assess(input);
        isSaved = true;
        HasUnsavedInput = false;
        savedSession = session;
        RenderResult(session);
        ShowStep(4);
    }

    private void RenderResult(AdvisorySession session)
    {
        resultStack.Controls.Clear();
        var r = session.Selected;
        var heading = Ui.Label(r.OutcomeText, 26, FontStyle.Bold);
        heading.ForeColor = r.Outcome == AdvisoryOutcome.StrongMatch ? Ui.Teal : r.Outcome == AdvisoryOutcome.NotCurrentlyEligible ? Color.Firebrick : Color.FromArgb(153, 89, 24);
        resultStack.Controls.Add(heading);
        resultStack.Controls.Add(Ui.Label($"{r.Programme}  |  {r.Faculty}", 14, FontStyle.Bold));
        resultStack.Controls.Add(Ui.Label($"Saved session #{session.Id}  •  Overall {r.OverallScore:0.##}%  •  Interest alignment {r.InterestScore:0.##}%"));
        var explanation = Ui.Card("Your result, explained", r.Explanation);
        resultStack.Controls.Add(explanation);
        var alternatives = Ui.Card("Other pathways to explore", session.Alternatives.Count == 0 ? "No suitable alternatives can be confirmed with the current information. Complete missing data or speak to an advisor." : "Ranked by outcome, then by weighted score. These alternatives are guidance, not admission offers.");
        foreach (var alternative in session.Alternatives)
        {
            alternatives.Controls.Add(Ui.Label($"{alternative.Programme}  •  {alternative.OutcomeText}  •  {alternative.OverallScore:0.##}%", 11, FontStyle.Bold));
            var captured = alternative;
            alternatives.Controls.Add(Ui.Button("Why this alternative?", () => MessageBox.Show(captured.Explanation + "\n\n" + RecommendationEngine.Disclaimer, captured.Programme)));
        }

        resultStack.Controls.Add(alternatives);
        var fund = Ui.Card("Funding guidance", session.Input.FundingRequested ? "Active, open funding linked to this programme or faculty. All sample records require verification." : "You did not request funding guidance for this session.");
        if (session.Input.FundingRequested)
        {
            var options = services.Advising.Funding(session.Selected.DegreeId);
            foreach (var f in options)
                fund.Controls.Add(Ui.Label($"{f.Name} | {f.Type} | Closes {f.ClosingDate:yyyy-MM-dd}\nEligibility: {f.Eligibility}\nDocuments: {f.Documents}\nContact: {f.Contact}\nSAMPLE — verify with provider."));
            if (options.Count == 0)
                fund.Controls.Add(Ui.Label("No currently open linked records. This does not mean you are ineligible for financial assistance."));
        }

        resultStack.Controls.Add(fund);
        resultStack.Controls.Add(Ui.Button("View, export or print advisory summary", () => showReport(services.Reports.Summary(session.Id)), true));
        resultStack.Controls.Add(Ui.Label("This saved assessment is read-only. Choose New advisory session to make another assessment. Staff can start follow-up sessions from Learners & sessions.", 9));
        resultStack.Controls.Add(Ui.Button("Finish — return to overview", this.finish));
        resultStack.Controls.Add(Ui.Label(RecommendationEngine.Disclaimer, 9));
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            foreach (var host in stepHosts.Values)
                host.Dispose();
            foreach (var step in steps)
                step.Dispose();
        }
        base.Dispose(disposing);
    }
}
