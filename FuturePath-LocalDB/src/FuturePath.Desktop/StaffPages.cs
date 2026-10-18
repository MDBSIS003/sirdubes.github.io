using FuturePath.Business;

namespace FuturePath.Desktop;
internal sealed class AdvisorPage : AppPage
{
    private readonly AppServices services;
    private readonly TextBox search = Ui.Input(260);
    private readonly DateTimePicker from = new()
    {
        Format = DateTimePickerFormat.Short,
        Width = 135,
        Value = DateTime.Today.AddYears(-1)
    };
    private readonly DateTimePicker to = new()
    {
        Format = DateTimePickerFormat.Short,
        Width = 135,
        Value = DateTime.Today
    };
    private readonly DataGridView grid = Ui.Grid();
    private readonly Label count = Ui.Label("");
    private List<SessionSummary> rows = [];
    public AdvisorPage(AppServices services, Action<ReportDocument> showReport, Action<int> followUp)
    {
        this.services = services;
        services.Access.RequireStaff();
        grid.AutoGenerateColumns = false;
        foreach (var(property, label)in new[]
        {
            ("Id", "Session #"),
            ("Learner", "Learner"),
            ("Grade", "Grade"),
            ("CreatedUtc", "Assessed (UTC)"),
            ("Programme", "Programme"),
            ("Outcome", "Outcome"),
            ("Score", "Score (%)")
        }

        )
            grid.Columns.Add(new DataGridViewTextBoxColumn { Name = property, DataPropertyName = property, HeaderText = label, DefaultCellStyle = new DataGridViewCellStyle { Format = property == "CreatedUtc" ? "yyyy-MM-dd" : "" } });
        var top = new FlowLayoutPanel
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            FlowDirection = FlowDirection.TopDown,
            Padding = new Padding(20)
        };
        top.Controls.Add(Ui.Row(Ui.Field("Learner / degree / session #", search), Ui.Field("From (UTC)", from), Ui.Field("To (UTC)", to), Ui.Button("Search", RefreshRows, true)));
        top.Controls.Add(Ui.Row(Ui.Button("View / export summary", () => showReport(services.Reports.Summary(Selected()))), Ui.Button("Create follow-up session", () => followUp(Selected()))));
        top.Controls.Add(count);
        var detail = new TextBox
        {
            Dock = DockStyle.Bottom,
            Height = 135,
            Multiline = true,
            ReadOnly = true,
            ScrollBars = ScrollBars.Vertical,
            BackColor = Color.White,
            Font = Ui.Body()
        };
        var body = new Panel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(20, 0, 20, 20)
        };
        body.Controls.Add(grid);
        body.Controls.Add(detail);
        Controls.Add(body);
        Controls.Add(top);
        grid.SelectionChanged += (_, _) =>
        {
            if (grid.CurrentRow?.DataBoundItem is SessionSummary row)
                detail.Text = row.Explanation;
        };
        grid.CellDoubleClick += (_, e) =>
        {
            if (e.RowIndex >= 0)
                Ui.Run(() => showReport(services.Reports.Summary(Selected())));
        };
        RefreshRows();
        FinishLayout();
    }

    private int Selected() => grid.CurrentRow?.DataBoundItem is SessionSummary row ? row.Id : throw new ValidationException("Select an advisory session first.");
    private void RefreshRows()
    {
        rows = services.Advising.Search(DateOnly.FromDateTime(from.Value), DateOnly.FromDateTime(to.Value), search.Text);
        grid.DataSource = rows;
        count.Text = $"{rows.Count} sessions • {rows.Select(r => r.LearnerId).Distinct().Count()} learners. Select a row to see the explanation below.";
    }
}

internal sealed class ReportsPage : AppPage
{
    public ReportsPage(AppServices services, Action<ReportDocument> showReport)
    {
        services.Access.RequireStaff();
        var stack = Ui.Stack();
        Controls.Add(stack);
        stack.Controls.Add(Ui.Label("Turn advising activity into action", 23, FontStyle.Bold));
        var from = new DateTimePicker
        {
            Value = DateTime.Today.AddYears(-1),
            Format = DateTimePickerFormat.Short,
            Width = 180
        };
        var to = new DateTimePicker
        {
            Value = DateTime.Today,
            Format = DateTimePickerFormat.Short,
            Width = 180
        };
        var search = Ui.Input(330);
        stack.Controls.Add(Ui.Row(Ui.Field("From date (UTC, inclusive)", from), Ui.Field("To date (UTC, inclusive)", to), Ui.Field("Learner / programme contains (optional)", search)));
        var activity = Ui.Card("Degree Interest and Eligibility", "Counts by faculty, programme and outcome, funding requests, and session details. Counts refer to sessions; distinct learners are shown separately.");
        activity.Controls.Add(Ui.Button("Generate activity report", () => showReport(services.Reports.Activity(DateOnly.FromDateTime(from.Value), DateOnly.FromDateTime(to.Value), false, search.Text)), true));
        stack.Controls.Add(activity);
        var exception = Ui.Card("Learners Not Currently Eligible", "Find learners classified as Not Currently Eligible or More Information Needed, with the gaps needed for follow-up discussions.");
        exception.Controls.Add(Ui.Button("Generate exception report", () => showReport(services.Reports.Activity(DateOnly.FromDateTime(from.Value), DateOnly.FromDateTime(to.Value), true, search.Text)), true));
        stack.Controls.Add(exception);
        stack.Controls.Add(Ui.Card("Learner Advisory Summary", "Open Learners & sessions, select a learner’s session, and choose View / export summary. Learners can also export their own new assessment."));
        stack.Controls.Add(Ui.Label("Reports display on screen and can be saved as text or printable HTML, or printed through Windows. Use Microsoft Print to PDF if available.", 9));
        FinishLayout();
    }
}

internal sealed class MaintenancePage : AppPage
{
    private readonly AppServices services;
    private readonly ComboBox entity;
    private readonly DataGridView grid = Ui.Grid();
    private readonly Label help = Ui.Label("");
    private List<ReferenceRow> rows = [];
    private readonly Panel listHost = new()
    {
        Dock = DockStyle.Fill
    };
    private UserControl? editor;
    public bool HasUnsavedInput => editor != null;

    public MaintenancePage(AppServices services)
    {
        this.services = services;
        services.Access.RequireAdmin();
        entity = Ui.Choice(ReferenceCatalog.Definitions.Select(d => d.Label).Concat(["Staff accounts", "Audit trail"]), 300);
        var top = new FlowLayoutPanel
        {
            Dock = DockStyle.Top,
            FlowDirection = FlowDirection.TopDown,
            AutoSize = true,
            Padding = new Padding(20),
            MaximumSize = new Size(1600, 0)
        };
        top.Controls.Add(Ui.Row(Ui.Field("Reference area", entity), Ui.Button("Add record", () => Edit(null), true), Ui.Button("Edit selected", EditSelected), Ui.Button("Refresh", RefreshRows), Ui.Button("Back up database", Backup)));
        help.MaximumSize = new Size(900, 0);
        top.Controls.Add(help);
        var body = new Panel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(20, 0, 20, 20)
        };
        body.Controls.Add(grid);
        listHost.Controls.Add(body);
        listHost.Controls.Add(top);
        Controls.Add(listHost);
        entity.SelectedIndexChanged += (_, _) => Ui.Run(RefreshRows);
        grid.CellDoubleClick += (_, e) =>
        {
            if (e.RowIndex >= 0)
                Ui.Run(EditSelected);
        };
        RefreshRows();
        FinishLayout();
    }

    private string Key => entity.SelectedIndex < ReferenceCatalog.Definitions.Count ? ReferenceCatalog.Definitions[entity.SelectedIndex].Key : entity.Text == "Staff accounts" ? "UserAccount" : "AuditEvent";

    private void RefreshRows()
    {
        rows = services.Maintenance.Rows(Key);
        grid.DataSource = Ui.Table(rows);
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
        help.Text = Key switch
        {
            "DegreeProgramme" => "Create new programmes as INACTIVE, add requirements and interest mappings, then activate. Deactivation preserves session history.",
            "ProgrammeRequirement" => "OR group 0 = independent requirement. Positive group numbers mean accept ANY subject in that group. Use Mandatory for entry requirements.",
            "FundingFaculty" or "FundingProgramme" => "Explicit programme links restrict a funding option to those programmes. With no programme links, faculty links apply faculty-wide.",
            "UserAccount" => "Only administrators maintain staff accounts. You cannot demote or deactivate your signed-in account. Password hashes are never shown.",
            "AuditEvent" => "Read-only audit trail, most recent 500 events. No passwords are recorded.",
            _ => "Add or update reference data. Untick Active to archive outdated records. Existing assessments and reports retain their saved results."
        };
    }

    private void EditSelected()
    {
        if (grid.CurrentRow == null)
            throw new ValidationException("Select a record first.");
        var id = Convert.ToInt32(grid.CurrentRow.Cells["Id"].Value);
        Edit(rows.Single(r => r.Id == id));
    }

    private void Edit(ReferenceRow? row)
    {
        if (Key == "AuditEvent")
            throw new ValidationException("The audit trail is read-only.");
        void ReturnToList()
        {
            if (editor != null)
            {
                Controls.Remove(editor);
                editor.Dispose();
                editor = null;
            }

            listHost.Visible = true;
            RefreshRows();
        }

        editor = Key == "UserAccount" ? new AccountEditor(services, row, ReturnToList, ReturnToList) : new ReferenceEditor(services, ReferenceCatalog.Get(Key), row, ReturnToList, ReturnToList);
        editor.Dock = DockStyle.Fill;
        listHost.Visible = false;
        Controls.Add(editor);
        editor.BringToFront();
    }

    private void Backup()
    {
        using var dialog = new SaveFileDialog
        {
            Title = "Create local database backup",
            Filter = "SQL Server backup (*.bak)|*.bak",
            FileName = $"FuturePath-Backup-{DateTime.Now:yyyyMMdd-HHmmss}.bak",
            OverwritePrompt = true
        };
        if (dialog.ShowDialog() != DialogResult.OK)
            return;
        services.Maintenance.Backup(dialog.FileName);
        MessageBox.Show("Database backed up. Store it securely; it contains learner information. Restore only while Future Path is closed.", "Future Path");
    }
}

internal sealed class ReferenceEditor : AppPage
{
    public ReferenceEditor(AppServices services, ReferenceDefinition definition, ReferenceRow? row, Action saved, Action cancel)
    {
        services.Access.RequireAdmin();
        Text = (row == null ? "Add — " : $"Edit #{row.Id} — ") + definition.Label;
        Size = new Size(780, 770);
        var stack = Ui.Stack();
        Controls.Add(stack);
        stack.Controls.Add(Ui.Label(definition.Label, 20, FontStyle.Bold));
        stack.Controls.Add(Ui.Label("Fields marked * are required. Save changes to update the local reference data.", 9));
        var read = new Dictionary<string, Func<object?>>();
        foreach (var field in definition.Fields)
        {
            var value = row?.Values.GetValueOrDefault(field.Name);
            Control input;
            if (field.Kind == FieldKind.Boolean)
            {
                var box = new CheckBox
                {
                    Checked = value == null ? field.Name != "Active" || definition.Key != "DegreeProgramme" : Convert.ToInt32(value) == 1,
                    AutoSize = true,
                    Text = field.Label
                };
                input = box;
                read[field.Name] = () => box.Checked;
            }
            else if (field.Kind == FieldKind.Lookup)
            {
                var combo = Ui.Combo(services.Maintenance.Lookup(field.Lookup!), value == null ? null : Convert.ToInt32(value), 650);
                input = combo;
                read[field.Name] = () => Ui.SelectedId(combo);
            }
            else if (field.Kind == FieldKind.Choice)
            {
                var combo = Ui.Choice(field.Choices!, 650);
                if (value != null)
                    combo.SelectedItem = Convert.ToString(value);
                input = combo;
                read[field.Name] = () => combo.Text;
            }
            else if (field.Kind == FieldKind.Date)
            {
                var date = new DateTimePicker
                {
                    Format = DateTimePickerFormat.Short,
                    Width = 300,
                    Value = value == null ? DateTime.Today.AddMonths(6) : DateTime.Parse(Convert.ToString(value)!, System.Globalization.CultureInfo.InvariantCulture)
                };
                input = date;
                read[field.Name] = () => date.Value.ToString("yyyy-MM-dd");
            }
            else if (field.Kind is FieldKind.Integer or FieldKind.Decimal)
            {
                var number = new NumericUpDown
                {
                    Minimum = field.Minimum,
                    Maximum = field.Maximum,
                    DecimalPlaces = field.Kind == FieldKind.Integer ? 0 : 2,
                    Width = 300,
                    Value = value == null ? field.Name == "Weight" ? 1 : field.Minimum : Convert.ToDecimal(value)
                };
                input = number;
                read[field.Name] = () => number.Value;
            }
            else
            {
                var box = Ui.Input(650);
                box.Text = Convert.ToString(value) ?? "";
                if (field.Kind == FieldKind.LongText)
                {
                    box.Multiline = true;
                    box.Height = 75;
                    box.ScrollBars = ScrollBars.Vertical;
                    box.MaxLength = field.MaximumLength > 0 ? field.MaximumLength : 2000;
                }

                input = box;
                read[field.Name] = () => box.Text;
            }

            stack.Controls.Add(Ui.Field(field.Label + (field.Required ? " *" : ""), input));
        }

        stack.Controls.Add(Ui.Button("Save changes", () =>
        {
            var values = read.ToDictionary(field => field.Key, field => field.Value());
            if (row != null && row.Text("Active") == "1" && values.GetValueOrDefault("Active")is false && !Ui.Confirm("Deactivate this record? Existing history will be retained."))
                return;
            services.Maintenance.Save(definition.Key, row?.Id ?? 0, values);
            saved();
        }, true));
        stack.Controls.Add(Ui.Button("Cancel — back to list", cancel));
        FinishLayout();
    }
}

internal sealed class AccountEditor : AppPage
{
    public AccountEditor(AppServices services, ReferenceRow? row, Action saved, Action cancel)
    {
        services.Access.RequireAdmin();
        Text = row == null ? "Create staff account" : "Edit staff account";
        Size = new Size(700, 670);
        var stack = Ui.Stack();
        Controls.Add(stack);
        var username = Ui.Input(500);
        username.Text = row?.Text("Username") ?? "";
        var role = Ui.Combo([new() { Id = 1, Name = "Career Advisor" }, new() { Id = 2, Name = "System Administrator" }], row?.Text("Role") == "Administrator" ? 2 : 1, 500);
        var password = Ui.Input(500);
        var confirm = Ui.Input(500);
        password.UseSystemPasswordChar = confirm.UseSystemPasswordChar = true;
        password.MaxLength = confirm.MaxLength = 128;
        var active = new CheckBox
        {
            Text = "Active account",
            AutoSize = true,
            Checked = row == null || row.Text("Active")is "1" or "True"
        };
        stack.Controls.Add(Ui.Label("Staff account", 23, FontStyle.Bold));
        stack.Controls.Add(Ui.Field("Username *", username));
        stack.Controls.Add(Ui.Field("Role *", role));
        stack.Controls.Add(Ui.Field("New password (blank keeps existing password)", password));
        stack.Controls.Add(Ui.Field("Confirm new password", confirm));
        stack.Controls.Add(active);
        stack.Controls.Add(Ui.Label("Use 12–128 characters with a letter and a number. Saving an account also clears its failed-attempt lockout.", 9));
        stack.Controls.Add(Ui.Button("Save account", () =>
        {
            if (password.Text != confirm.Text)
                throw new ValidationException("Passwords do not match.");
            if (row != null && row.Text("Active") == "1" && !active.Checked && !Ui.Confirm("Deactivate this staff account?"))
                return;
            services.Authentication.SaveUser(row?.Id ?? 0, username.Text, Ui.SelectedId(role) == 2 ? UserRole.Administrator : UserRole.Advisor, active.Checked, password.Text);
            saved();
        }, true));
        stack.Controls.Add(Ui.Button("Cancel — back to list", cancel));
        FinishLayout();
    }
}
