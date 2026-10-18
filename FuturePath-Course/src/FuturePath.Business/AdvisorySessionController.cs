using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace FuturePath.Business
{
    public sealed class AdvisorySessionController
    {
        private readonly IFuturePathStore store;
        private readonly AccessSession access;
        private readonly IClock clock;
        public AdvisorySessionController(IFuturePathStore store, AccessSession access, IClock clock)
        {
            this.store = store;
            this.access = access;
            this.clock = clock;
        }

        private readonly RecommendationEngine engine = new RecommendationEngine();
        public List<LookupItem> Subjects()
        {
            return store.GetLookup("Subject");
        }

        public List<LookupItem> Faculties()
        {
            return store.GetLookup("Faculty");
        }

        public List<DegreeProgramme> Programmes()
        {
            return store.GetProgrammes(true);
        }

        public List<InterestQuestion> Questions()
        {
            return store.GetQuestions();
        }

        public List<FundingOption> Funding(int degreeId)
        {
            return store.GetFunding(degreeId, clock.Today);
        }

        public AdvisorySession Preview(AdvisoryInput input)
        {
            Validate(input);
            return engine.Assess(input, Programmes(), Questions(), clock.UtcNow);
        }

        public AdvisorySession Assess(AdvisoryInput input)
        {
            AdvisorySession session = Preview(input);
            session.Id = store.SaveSession(session, access.UserId);
            access.OwnSession(session.Id);
            return session;
        }

        public AdvisorySession Load(int id)
        {
            access.RequireSession(id);
            return store.GetSession(id) ?? throw new ValidationException("The advisory session no longer exists.");
        }

        public List<SessionSummary> Search(DateOnly from, DateOnly to, string search = "")
        {
            access.RequireStaff();
            if (from > to)
            {
                throw new ValidationException("The start date must be on or before the end date.");
            }

            return store.FindSessions(from, to, search.Trim());
        }

        private void Validate(AdvisoryInput input)
        {
            LearnerProfile learner = input.Learner;
            AdvisingValidation.ProfileCapture(learner);

            if (learner.Id != 0)
            {
                access.RequireStaff();
                LearnerProfile stored = store.GetLearner(learner.Id) ?? throw new ValidationException("Learner not found.");
                if ((stored.Name.Trim().Length > 0 && stored.Name.Trim() != learner.Name) ||
                    (stored.School.Trim().Length > 0 && stored.School.Trim() != learner.School) ||
                    (stored.Email.Trim().Length > 0 && stored.Email.Trim() != learner.Email) ||
                    (stored.Phone.Trim().Length > 0 && stored.Phone.Trim() != learner.Phone) ||
                    (stored.Province.Trim().Length > 0 && stored.Province.Trim() != learner.Province))
                {
                    throw new ValidationException("A follow-up preserves learner identity. Start a new learner profile to capture different personal details.");
                }
            }

            AdvisingValidation.Marks(input.Marks, Subjects());

            if (!input.SubjectsComplete)
            {
                throw new ValidationException("Confirm that every subject and mark has been entered before continuing.");
            }

            AdvisingValidation.Interests(input.Responses, Questions(), requireComplete: true);
            bool facultyFound = false;
            foreach (LookupItem faculty in Faculties())
            {
                if (faculty.Id == input.PreferredFacultyId)
                {
                    facultyFound = true;
                    break;
                }
            }

            if (!facultyFound)
            {
                throw new ValidationException("Select a preferred faculty.");
            }

            bool degreeFound = false;
            foreach (DegreeProgramme degree in Programmes())
            {
                if (degree.Id != input.DegreeId)
                {
                    continue;
                }

                degreeFound = true;
                if (degree.FacultyId != input.PreferredFacultyId)
                {
                    throw new ValidationException("Select a degree within the chosen faculty.");
                }
            }

            if (!degreeFound)
            {
                throw new ValidationException("Select an active degree programme.");
            }
        }
    }
}
