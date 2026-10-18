using System.Net.Mail;

namespace FuturePath.Business;
/// <summary>Shared page/service validation; it never opens a database or reads a control.</summary>
public static class AdvisingValidation
{
    public static void Profile(LearnerProfile learner)
    {
        if (learner.Grade != 11 && learner.Grade != 12)
            throw new ValidationException("Choose Grade 11 or Grade 12.");
        learner.Name = learner.Name.Trim();
        learner.School = learner.School.Trim();
        learner.Province = learner.Province.Trim();
        learner.Email = learner.Email.Trim();
        learner.Phone = learner.Phone.Trim();
        if (learner.Name.Length > 120 || learner.School.Length > 120 || learner.Email.Length > 150)
            throw new ValidationException("Names and schools must be at most 120 characters; email at most 150.");
        if (learner.Email.Length > 0 && (!MailAddress.TryCreate(learner.Email, out var email) || email.Address != learner.Email))
            throw new ValidationException("Enter a valid email address or leave it blank.");
        if (learner.Phone.Length > 0 && (learner.Phone.Length < 7 || learner.Phone.Length > 20 || learner.Phone.Any(c => !char.IsDigit(c) && !"+()- ".Contains(c))))
            throw new ValidationException("Enter a valid contact number or leave it blank.");
        if (learner.Province.Length > 0 && !ReferenceCatalog.Provinces.Contains(learner.Province))
            throw new ValidationException("Select a South African province.");
    }

    public static void Marks(IReadOnlyList<SubjectMark> marks, IReadOnlyList<LookupItem> activeSubjects)
    {
        if (marks.Count == 0)
            throw new ValidationException("Add at least one subject and mark before continuing.");
        var captured = new HashSet<int>();
        foreach (var mark in marks)
        {
            if (!captured.Add(mark.SubjectId))
                throw new ValidationException("A subject may only be captured once in a session.");
            if (mark.Mark < 0 || mark.Mark > 100)
                throw new ValidationException("Marks must be between 0 and 100.");
            var subject = activeSubjects.FirstOrDefault(s => s.Id == mark.SubjectId);
            if (subject == null)
                throw new ValidationException("Select a valid active subject.");
            mark.SubjectName = subject.Name;
        }
    }

    public static void Interests(IReadOnlyList<InterestResponse> responses, IReadOnlyList<InterestQuestion> questions, bool requireComplete)
    {
        var answered = new HashSet<int>();
        foreach (var response in responses)
        {
            if (!answered.Add(response.QuestionId))
                throw new ValidationException("Only one response per question is allowed.");
            if (response.Score < 1 || response.Score > 5 || !questions.Any(q => q.Id == response.QuestionId))
                throw new ValidationException("Questionnaire answers must be from 1 to 5 and refer to active questions.");
        }

        if (requireComplete && questions.Count == 0)
            throw new ValidationException("Ask an administrator to configure the active interest questionnaire.");
        if (requireComplete && questions.Any(q => !answered.Contains(q.Id)))
            throw new ValidationException("Choose a response for every interest question before continuing.");
    }
}
