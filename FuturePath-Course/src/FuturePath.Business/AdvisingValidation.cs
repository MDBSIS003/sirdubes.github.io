using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Net.Mail;

namespace FuturePath.Business
{
         public static class AdvisingValidation
    {
        public const int MinimumSubjects = 7;


        public static void ProfileCapture(LearnerProfile learner)
        {
            Profile(learner);
            if (learner.Name.Length == 0)
            {
                throw new ValidationException("Name and surname are required.");
            }

            if (learner.Email.Length == 0)
            {
                throw new ValidationException("Email is required.");
            }

            if (learner.Phone.Length == 0)
            {
                throw new ValidationException("Contact number is required.");
            }

            if (learner.School.Length == 0)
            {
                throw new ValidationException("School is required.");
            }

            if (learner.Province.Length == 0)
            {
                throw new ValidationException("Province is required.");
            }
        }

        public static void Profile(LearnerProfile learner)
        {
            if (learner.Grade != 11 && learner.Grade != 12)
            {
                throw new ValidationException("Choose Grade 11 or Grade 12.");
            }

            learner.Name = learner.Name.Trim();
            learner.School = learner.School.Trim();
            learner.Province = learner.Province.Trim();
            learner.Email = learner.Email.Trim();
            learner.Phone = learner.Phone.Trim();
            if (learner.Name.Length > 120 || learner.School.Length > 120 || learner.Email.Length > 150)
            {
                throw new ValidationException("Names and schools must be at most 120 characters; email at most 150.");
            }

            if (learner.Name.Length > 0 && !ValidName(learner.Name))
            {
                throw new ValidationException("Use letters, spaces, hyphens and apostrophes for the learner name.");
            }

            if (learner.Email.Length > 0 && (!MailAddress.TryCreate(learner.Email, out var email) || email.Address != learner.Email))
            {
                throw new ValidationException("Enter a valid email address.");
            }

            if (learner.Phone.Length > 0 && (!IsValidContactNumber(learner.Phone)))
            {
                throw new ValidationException("Enter a contact number of exactly 10 digits, for example 0712345678.");
            }

            if (learner.Province.Length > 0 && Array.IndexOf(ReferenceCatalog.Provinces, learner.Province) < 0)
            {
                throw new ValidationException("Select a South African province.");
            }
        }

        public static void Marks(IReadOnlyList<SubjectMark> marks, IReadOnlyList<LookupItem> activeSubjects)
        {
            if (marks.Count < MinimumSubjects)
            {
                throw new ValidationException("Enter at least seven different subjects and their marks before continuing.");
            }

            HashSet<int> captured = new HashSet<int>();
            foreach (SubjectMark mark in marks)
            {
                if (!captured.Add(mark.SubjectId))
                {
                    throw new ValidationException("A subject may only be captured once in a session.");
                }

                if (mark.Mark < 0 || mark.Mark > 100)
                {
                    throw new ValidationException("Marks must be between 0 and 100.");
                }

                LookupItem? subject = null;
                foreach (LookupItem item in activeSubjects)
                {
                    if (item.Id == mark.SubjectId)
                    {
                        subject = item;
                        break;
                    }
                }

                if (subject == null)
                {
                    throw new ValidationException("Select a valid active subject.");
                }

                mark.SubjectName = subject.Name;
            }
        }

        public static void Interests(IReadOnlyList<InterestResponse> responses, IReadOnlyList<InterestQuestion> questions, bool requireComplete)
        {
            HashSet<int> answered = new HashSet<int>();
            foreach (InterestResponse response in responses)
            {
                if (!answered.Add(response.QuestionId))
                {
                    throw new ValidationException("Only one response per question is allowed.");
                }

                if (response.Score < 1 || response.Score > 5 || !HasQuestion(questions, response.QuestionId))
                {
                    throw new ValidationException("Questionnaire answers must be from 1 to 5 and refer to active questions.");
                }
            }

            if (requireComplete && questions.Count == 0)
            {
                throw new ValidationException("Ask an administrator to configure the active interest questionnaire.");
            }

            if (requireComplete)
            {
                foreach (InterestQuestion question in questions)
                {
                    if (!answered.Contains(question.Id))
                    {
                        throw new ValidationException("Choose a response for every interest question before continuing.");
                    }
                }
            }
        }

        private static bool HasQuestion(IReadOnlyList<InterestQuestion> questions, int id)
        {
            foreach (InterestQuestion question in questions)
            {
                if (question.Id == id)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool ValidName(string name)
        {
            bool hasLetter = false;
            foreach (char character in name)
            {
                if (char.IsLetter(character))
                {
                    hasLetter = true;
                }
                else if (character != ' ' && character != '-' && character != '\'' && character != '’')
                {
                    return false;
                }
            }

            return hasLetter;
        }

        public static bool IsValidContactNumber(string phone)
        {
            if (phone.Length != 10)
            {
                return false;
            }
            foreach (char digit in phone)
            {
                if (digit < '0' || digit > '9')
                {
                    return false;
                }
            }
            return true;
        }
    }
}
