using FuturePath.Business;

namespace FuturePath.Tests;
public class ReportFilterTests
{
    [LocalDbFact]
    public void FacultyDegreeAndOutcomeFiltersUseSavedKeys()
    {
        using TestDatabase db = new TestDatabase();
        db.Advisor();
        DateOnly from = new DateOnly(2026, 9, 1);
        DateOnly to = new DateOnly(2026, 9, 30);
        AdvisorySession gadija = db.Store.GetSession(1)!;
        DegreeProgramme programme = db.Store.GetProgrammes(false).Single(degree => degree.Id == gadija.Input.DegreeId);
        string report = db.Reports.Activity(from, to, false, "", programme.FacultyId, programme.Id, "Strong Match").Content;
        Assert.Contains("Gadija Jacobs", report);
        Assert.DoesNotContain("Aadiel-Raboo Magwa", report);
        Assert.Contains("Degree filter: " + programme.Name, report);
        Assert.Contains("No sessions match", db.Reports.Activity(from, to, false, "Gadija", programme.FacultyId, programme.Id, "Not Currently Eligible").Content);
    }
    [LocalDbFact]
    public void GapAndFundingSectionsDescribeTheirSavedAndCurrentSources()
    {
        using TestDatabase db = new TestDatabase();
        db.Advisor();
        string report = db.Reports.Activity(new DateOnly(2026, 9, 1), new DateOnly(2026, 9, 30), false).Content;
        Assert.Contains("COMMON SUBJECT GAPS", report);
        Assert.Contains("Mathematics at", report);
        Assert.Contains("COMMON MARK GAPS", report);
        Assert.Contains("FUNDING REQUESTS BY LINKED CATEGORY", report);
        Assert.Contains("Each session counts once per currently open category", report);
    }
    [LocalDbFact]
    public void AdvisorReportListsIncludeInactiveHistoricalReferenceKeys()
    {
        using TestDatabase db = new TestDatabase();
        db.Admin();
        ReferenceRow programme = db.Maintenance.Rows("DegreeProgramme").First();
        Dictionary<string, object?> values = new Dictionary<string, object?>(programme.Values);
        values["Active"] = 0;
        db.Maintenance.Save("DegreeProgramme", programme.Id, values);
        db.Advisor();
        Assert.Contains(db.Reports.Programmes(), degree => degree.Id == programme.Id && !degree.Active);
        Assert.Contains(db.Reports.Faculties(), faculty => faculty.Id > 0);
    }
}
