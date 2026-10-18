namespace FuturePath.Business;
public enum FieldKind
{
    Text,
    LongText,
    Integer,
    Decimal,
    Boolean,
    Date,
    Lookup,
    Choice
}

public sealed record ReferenceField(string Name, string Label, FieldKind Kind = FieldKind.Text, bool Required = true, string? Lookup = null, string[]? Choices = null, decimal Minimum = 0, decimal Maximum = 100, int MaximumLength = 0);
public sealed record ReferenceDefinition(string Key, string Label, ReferenceField[] Fields);
public sealed class ReferenceRow
{
    public int Id { get; init; }
    public Dictionary<string, object?> Values { get; init; } = [];

    public string Text(string name) => Convert.ToString(Values.GetValueOrDefault(name), System.Globalization.CultureInfo.InvariantCulture) ?? "";
}

/// <summary>Whitelisted reference entities shared by typed validation, data access and form editors.</summary>
public static class ReferenceCatalog
{
    public static readonly string[] Provinces = ["Eastern Cape", "Free State", "Gauteng", "KwaZulu-Natal", "Limpopo", "Mpumalanga", "Northern Cape", "North West", "Western Cape"];
    public static readonly string[] FundingTypes = ["Scholarship", "Bursary", "Financial aid", "External funding", "Merit award"];
    private static ReferenceField Active() => new("Active", "Active", FieldKind.Boolean);
    private static ReferenceField Name() => new("Name", "Name");
    private static ReferenceField Link(string name, string label, string table) => new(name, label, FieldKind.Lookup, Lookup: table);
    public static IReadOnlyList<ReferenceDefinition> Definitions { get; } = [new("Faculty", "Faculties", [Name(), Active()]), new("Subject", "School subjects", [Name(), Active()]), new("InterestCategory", "Interest categories", [Name(), Active()]), new("DegreeProgramme", "Degree programmes", [Name(), Link("FacultyId", "Faculty", "Faculty"), new("Description", "Description", FieldKind.LongText), new("MinimumAverage", "Minimum overall average (%)", FieldKind.Decimal), Active()]), new("ProgrammeRequirement", "Admission requirements", [Link("DegreeId", "Degree", "DegreeProgramme"), Link("SubjectId", "Subject", "Subject"), new("MinimumMark", "Minimum mark (%)", FieldKind.Decimal), new("Mandatory", "Mandatory", FieldKind.Boolean), new("AlternativeGroup", "OR group (0 = independent)", FieldKind.Integer, Maximum: 99), new("Note", "Advisory note", FieldKind.LongText, false), Active()]), new("InterestQuestion", "Questionnaire", [new("Text", "Question", FieldKind.LongText, MaximumLength: 400), Link("CategoryId", "Category", "InterestCategory"), Active()]), new("DegreeInterestMapping", "Degree interest weights", [Link("DegreeId", "Degree", "DegreeProgramme"), Link("CategoryId", "Category", "InterestCategory"), new("Weight", "Weight (0.01–10)", FieldKind.Decimal, Minimum: .01m, Maximum: 10), Active()]), new("FundingOption", "Funding opportunities", [Name(), new("Type", "Funding type", FieldKind.Choice, Choices: FundingTypes), new("Eligibility", "Eligibility / merit / financial need", FieldKind.LongText), new("Documents", "Required documents", FieldKind.LongText), new("ClosingDate", "Closing date", FieldKind.Date), new("Contact", "Contact / application information", FieldKind.LongText), new("Sample", "Sample / requires verification", FieldKind.Boolean), Active()]), new("FundingFaculty", "Funding ↔ faculties", [Link("FundingId", "Funding", "FundingOption"), Link("FacultyId", "Faculty", "Faculty"), Active()]), new("FundingProgramme", "Funding ↔ programmes", [Link("FundingId", "Funding", "FundingOption"), Link("DegreeId", "Degree", "DegreeProgramme"), Active()]), ];

    public static ReferenceDefinition Get(string key) => Definitions.SingleOrDefault(d => d.Key == key) ?? throw new ValidationException("Unknown reference-data area.");
}

public sealed class MaintenanceService
{
    private readonly IFuturePathStore store;
    private readonly AccessSession access;
    private readonly IClock clock;
    public MaintenanceService(IFuturePathStore store, AccessSession access, IClock clock)
    {
        this.store = store;
        this.access = access;
        this.clock = clock;
    }

    public List<ReferenceRow> Rows(string key)
    {
        access.RequireAdmin();
        if (key != "UserAccount" && key != "AuditEvent")
            ReferenceCatalog.Get(key);
        return store.GetReference(key);
    }

    public List<LookupItem> Lookup(string key)
    {
        access.RequireAdmin();
        return store.GetLookup(key + ":all");
    }

    public int Save(string key, int id, IReadOnlyDictionary<string, object?> input)
    {
        access.RequireAdmin();
        var definition = ReferenceCatalog.Get(key);
        var values = new Dictionary<string, object?>();
        foreach (var field in definition.Fields)
        {
            var raw = input.GetValueOrDefault(field.Name);
            var text = Convert.ToString(raw, System.Globalization.CultureInfo.InvariantCulture)?.Trim() ?? "";
            if (field.Kind is FieldKind.Text or FieldKind.LongText or FieldKind.Choice)
            {
                if (field.Required && text.Length == 0)
                    throw new ValidationException($"{field.Label} is required.");
                if (text.Length > (field.MaximumLength > 0 ? field.MaximumLength : field.Kind == FieldKind.LongText ? 2000 : 150))
                    throw new ValidationException($"{field.Label} is too long.");
                if (field.Kind == FieldKind.Choice && !field.Choices!.Contains(text))
                    throw new ValidationException($"Select a valid {field.Label}.");
                values[field.Name] = text;
            }
            else if (field.Kind == FieldKind.Boolean)
            {
                if (text is not ("0" or "1" or "True" or "False"))
                    throw new ValidationException($"Choose a valid value for {field.Label}.");
                values[field.Name] = text is "1" or "True" ? 1 : 0;
            }
            else if (field.Kind == FieldKind.Date)
            {
                if (!DateOnly.TryParse(text, System.Globalization.CultureInfo.InvariantCulture, out var date))
                    throw new ValidationException($"Enter a valid {field.Label}.");
                values[field.Name] = date.ToString("yyyy-MM-dd");
            }
            else
            {
                if (!decimal.TryParse(text, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out var number))
                    throw new ValidationException($"Enter a valid number for {field.Label}.");
                if (field.Kind == FieldKind.Lookup)
                {
                    if (number != decimal.Truncate(number) || !store.GetLookup(field.Lookup! + ":all").Any(r => r.Id == number))
                        throw new ValidationException($"Select a valid {field.Label}.");
                }
                else if (number < field.Minimum || number > field.Maximum || (field.Kind == FieldKind.Integer && number != decimal.Truncate(number)))
                    throw new ValidationException($"{field.Label} must be between {field.Minimum} and {field.Maximum}.");
                values[field.Name] = field.Kind is FieldKind.Lookup or FieldKind.Integer ? (object)Convert.ToInt32(number) : number;
            }
        }

        var active = Convert.ToInt32(values.GetValueOrDefault("Active")) == 1;
        var programmes = store.GetProgrammes(false);
        if (key == "DegreeProgramme" && active)
        {
            var programme = programmes.SingleOrDefault(p => p.Id == id);
            if (programme is null || !programme.Requirements.Any(r => r.Mandatory) || programme.Interests.Count == 0)
                throw new ValidationException("Save a new programme as inactive, add mandatory requirements and interest weights, then activate it.");
            var faculty = Convert.ToInt32(values["FacultyId"]);
            if (!store.GetLookup("Faculty").Any(f => f.Id == faculty))
                throw new ValidationException("Choose an active faculty before activating the programme.");
            if (programme.Requirements.Any(r => !store.GetLookup("Subject").Any(s => s.Id == r.SubjectId)))
                throw new ValidationException("Activate the referenced subjects before activating this programme.");
            if (programme.Interests.Any(w => !store.GetQuestions().Any(q => q.CategoryId == w.CategoryId)))
                throw new ValidationException("Each degree interest category must have an active questionnaire question before the programme is activated.");
        }

        if (key is "ProgrammeRequirement" or "DegreeInterestMapping")
        {
            if (active && key == "ProgrammeRequirement" && !store.GetLookup("Subject").Any(s => s.Id == Convert.ToInt32(values["SubjectId"])))
                throw new ValidationException("An active requirement must refer to an active subject.");
            if (active && key == "DegreeInterestMapping" && !store.GetQuestions().Any(q => q.CategoryId == Convert.ToInt32(values["CategoryId"])))
                throw new ValidationException("An active interest mapping must have an active question in that category.");
            var existing = id == 0 ? null : store.GetReference(key).SingleOrDefault(r => r.Id == id);
            // Require deactivation before changing structural rules, making incomplete maintenance visible.
            var affected = existing is null ? 0 : Convert.ToInt32(existing.Values["DegreeId"]);
            if (affected > 0 && programmes.Any(p => p.Id == affected && p.Active) && (!active || affected != Convert.ToInt32(values["DegreeId"]) || key == "ProgrammeRequirement" && Convert.ToInt32(values["Mandatory"]) == 0))
                throw new ValidationException("Deactivate the programme before removing or moving its active requirements or interest mappings.");
        }

        if (key == "InterestQuestion" && id > 0)
        {
            var old = store.GetQuestions(false).SingleOrDefault(q => q.Id == id);
            if (old != null && (!active || old.CategoryId != Convert.ToInt32(values["CategoryId"])) && !store.GetQuestions().Any(q => q.Id != id && q.CategoryId == old.CategoryId) && programmes.Any(p => p.Active && p.Interests.Any(w => w.CategoryId == old.CategoryId)))
                throw new ValidationException("Deactivate affected programmes before removing their last active interest question.");
        }

        if (!active && key is "Subject" or "InterestCategory")
        {
            if (programmes.Any(p => p.Active && (key == "Subject" ? p.Requirements.Any(r => r.SubjectId == id) : p.Interests.Any(w => w.CategoryId == id))))
                throw new ValidationException("Deactivate affected degree programmes before deactivating a referenced subject or interest category.");
        }

        var saved = store.SaveReference(key, id, values);
        store.Audit(access.UserId, "Reference saved", $"{key} #{saved}", clock.UtcNow);
        return saved;
    }

    public void Backup(string path)
    {
        access.RequireAdmin();
        store.Backup(path);
        store.Audit(access.UserId, "Database backed up", "Local database backup created", clock.UtcNow);
    }
}
