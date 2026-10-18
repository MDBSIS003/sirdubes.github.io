using System.Net.Mail;
using FuturePathStudentAdvisingSystem.Models;

namespace FuturePathStudentAdvisingSystem.BusinessLogic;

public static class Validation
{
    public static readonly string[] Provinces = ["Eastern Cape", "Free State", "Gauteng", "KwaZulu-Natal", "Limpopo",
        "Mpumalanga", "Northern Cape", "North West", "Western Cape"];
    public static string Required(string value, string label, int max)
    {
        value = value.Trim();
        if (value.Length == 0 || value.Length > max) throw new ArgumentException($"{label} is required (maximum {max} characters).");
        return value;
    }
    public static void Profile(LearnerProfile p, bool requireGrade = true)
    {
        p.FirstName = Required(p.FirstName, "First name", 50);
        p.LastName = Required(p.LastName, "Last name", 50);
        p.SchoolName = Required(p.SchoolName, "School", 100);
        p.Email = Required(p.Email, "Email", 50);
        if (!MailAddress.TryCreate(p.Email, out var address) || address.Address != p.Email)
            throw new ArgumentException("Enter a valid email address.");
        if (!Provinces.Contains(p.Province)) throw new ArgumentException("Select a South African province.");
        if (p.Phone.Length > 25 || p.Phone.Any(c => !char.IsDigit(c) && !"+ ()-".Contains(c)))
            throw new ArgumentException("Enter a valid contact number (maximum 25 characters).");
        if (requireGrade && p.GradeLevelID is null) throw new ArgumentException("Select Grade 11 or Grade 12.");
    }
    public static void Marks(IReadOnlyList<SubjectMark> marks)
    {
        if (marks.Count == 0) throw new ArgumentException("Add at least one subject.");
        if (marks.Any(x => x.SubjectID <= 0 || x.Mark < 0 || x.Mark > 100)) throw new ArgumentException("Marks must be between 0 and 100.");
        if (marks.Select(x => x.SubjectID).Distinct().Count() != marks.Count) throw new ArgumentException("Each subject may be entered once.");
    }
    public static void Responses(IReadOnlyList<InterestResponse> responses, IReadOnlyList<InterestQuestion> questions)
    {
        if (questions.Count == 0) throw new InvalidOperationException("No questionnaire is configured. Ask the administrator.");
        if (responses.Count != questions.Count || responses.Select(x => x.QuestionID).Distinct().Count() != questions.Count ||
            responses.Any(x => !questions.Any(q => q.QuestionID == x.QuestionID) || x.Score < 1 || x.Score > 5 || x.Score != decimal.Truncate(x.Score)))
            throw new ArgumentException("Answer every question using a score from 1 to 5.");
    }
}
