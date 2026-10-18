using System;
using System.Collections.Generic;
using System.Globalization;

namespace FuturePath.Business
{
    public class AdminMaintenanceController
    {
        #region Data Members
        private IFuturePathStore store;
        private AccessSession access;
        private IClock clock;
        #endregion
        public AdminMaintenanceController(IFuturePathStore store, AccessSession access, IClock clock)
        {
            this.store = store;
            this.access = access;
            this.clock = clock;
        }

        public List<ReferenceRow> Rows(string key)
        {
            access.RequireAdmin();
            if (key != "UserAccount" && key != "AuditEvent")
            {
                ReferenceCatalog.Get(key);
            }

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
            Dictionary<string, object?> values = ReadValues(ReferenceCatalog.Get(key), input);
            bool active = Convert.ToInt32(values["Active"]) == 1;
            List<DegreeProgramme> programmes = store.GetProgrammes(false);
            if (key == "DegreeProgramme" && active)
            {
                ValidateDegree(id, values, programmes);
            }

            if (key == "ProgrammeRequirement" || key == "DegreeInterestMapping")
            {
                ValidateRule(key, id, values, active, programmes);
            }

            if (key == "InterestQuestion" && id > 0)
            {
                ValidateQuestion(id, values, active, programmes);
            }

            if (!active && (key == "Subject" || key == "InterestCategory"))
            {
                ValidateDeactivation(key, id, programmes);
            }

            int saved = store.SaveReference(key, id, values);
            store.Audit(access.UserId, "Reference saved", key + " #" + saved, clock.UtcNow);
            return saved;
        }

        private Dictionary<string, object?> ReadValues(ReferenceDefinition definition, IReadOnlyDictionary<string, object?> input)
        {
            Dictionary<string, object?> values = new Dictionary<string, object?>();
            foreach (ReferenceField field in definition.Fields)
            {
                object? raw = input.GetValueOrDefault(field.Name);
                string text = Convert.ToString(raw, CultureInfo.InvariantCulture)?.Trim() ?? "";
                switch (field.Kind)
                {
                    case FieldKind.Text:
                    case FieldKind.LongText:
                    case FieldKind.Choice:
                        ValidateText(field, text);
                        values[field.Name] = text;
                        break;
                    case FieldKind.Boolean:
                        if (text != "0" && text != "1" && text != "True" && text != "False")
                        {
                            throw new ValidationException("Choose a valid value for " + field.Label + ".");
                        }

                        values[field.Name] = text == "1" || text == "True" ? 1 : 0;
                        break;
                    case FieldKind.Date:
                        DateOnly date;
                        if (!DateOnly.TryParse(text, CultureInfo.InvariantCulture, out date))
                        {
                            throw new ValidationException("Enter a valid " + field.Label + ".");
                        }

                        values[field.Name] = date.ToString("yyyy-MM-dd");
                        break;
                    default:
                        decimal number;
                        if (!decimal.TryParse(text, NumberStyles.Number, CultureInfo.InvariantCulture, out number))
                        {
                            throw new ValidationException("Enter a valid number for " + field.Label + ".");
                        }

                        ValidateNumber(field, number);
                        if (field.Kind == FieldKind.Lookup || field.Kind == FieldKind.Integer)
                        {
                            values[field.Name] = Convert.ToInt32(number);
                        }
                        else
                        {
                            values[field.Name] = number;
                        }

                        break;
                }
            }

            return values;
        }

        private void ValidateText(ReferenceField field, string text)
        {
            if (field.Required && text.Length == 0)
            {
                throw new ValidationException(field.Label + " is required.");
            }

            int limit = field.Kind == FieldKind.LongText ? 2000 : 150;
            if (field.MaximumLength > 0)
            {
                limit = field.MaximumLength;
            }

            if (text.Length > limit)
            {
                throw new ValidationException(field.Label + " is too long.");
            }

            if (field.Kind == FieldKind.Choice && (field.Choices == null || Array.IndexOf(field.Choices, text) < 0))
            {
                throw new ValidationException("Select a valid " + field.Label + ".");
            }
        }

        private void ValidateNumber(ReferenceField field, decimal number)
        {
            if (field.Kind == FieldKind.Lookup)
            {
                if (number != decimal.Truncate(number) || !HasLookup(field.Lookup! + ":all", number))
                {
                    throw new ValidationException("Select a valid " + field.Label + ".");
                }
            }
            else if (number < field.Minimum || number > field.Maximum || (field.Kind == FieldKind.Integer && number != decimal.Truncate(number)))
            {
                throw new ValidationException(field.Label + " must be between " + field.Minimum + " and " + field.Maximum + ".");
            }
        }

        private bool HasLookup(string key, decimal id)
        {
            foreach (LookupItem item in store.GetLookup(key))
            {
                if (item.Id == id)
                {
                    return true;
                }
            }

            return false;
        }

        private bool HasQuestion(int categoryId, int excludingId = 0)
        {
            foreach (InterestQuestion question in store.GetQuestions())
            {
                if (question.CategoryId == categoryId && question.Id != excludingId)
                {
                    return true;
                }
            }

            return false;
        }

        private DegreeProgramme? FindProgramme(int id, List<DegreeProgramme> programmes)
        {
            foreach (DegreeProgramme programme in programmes)
            {
                if (programme.Id == id)
                {
                    return programme;
                }
            }

            return null;
        }

        private void ValidateDegree(int id, Dictionary<string, object?> values, List<DegreeProgramme> programmes)
        {
            DegreeProgramme? programme = FindProgramme(id, programmes);
            bool mandatory = false;
            if (programme != null)
            {
                foreach (ProgrammeRequirement rule in programme.Requirements)
                {
                    if (rule.Mandatory)
                    {
                        mandatory = true;
                    }
                }
            }

            if (programme == null || !mandatory || programme.Interests.Count == 0)
            {
                throw new ValidationException("Save a new programme as inactive, add mandatory requirements and interest weights, then activate it.");
            }

            if (!HasLookup("Faculty", Convert.ToInt32(values["FacultyId"])))
            {
                throw new ValidationException("Choose an active faculty before activating the programme.");
            }

            foreach (ProgrammeRequirement rule in programme.Requirements)
            {
                if (!HasLookup("Subject", rule.SubjectId))
                {
                    throw new ValidationException("Activate the referenced subjects before activating this programme.");
                }
            }

            foreach (InterestWeight weight in programme.Interests)
            {
                if (!HasQuestion(weight.CategoryId))
                {
                    throw new ValidationException("Each degree interest category must have an active questionnaire question before the programme is activated.");
                }
            }
        }

        private void ValidateRule(string key, int id, Dictionary<string, object?> values, bool active, List<DegreeProgramme> programmes)
        {
            if (active && key == "ProgrammeRequirement" && !HasLookup("Subject", Convert.ToInt32(values["SubjectId"])))
            {
                throw new ValidationException("An active requirement must refer to an active subject.");
            }

            if (active && key == "DegreeInterestMapping" && !HasQuestion(Convert.ToInt32(values["CategoryId"])))
            {
                throw new ValidationException("An active interest mapping must have an active question in that category.");
            }

            ReferenceRow? existing = null;
            if (id > 0)
            {
                foreach (ReferenceRow row in store.GetReference(key))
                {
                    if (row.Id == id)
                    {
                        existing = row;
                        break;
                    }
                }
            }

            if (existing == null)
            {
                return;
            }

            int oldDegree = Convert.ToInt32(existing.Values["DegreeId"]);
            DegreeProgramme? programme = FindProgramme(oldDegree, programmes);
            if (programme == null || !programme.Active)
            {
                return;
            }

            bool removed = !active || oldDegree != Convert.ToInt32(values["DegreeId"]);
            if (key == "ProgrammeRequirement" && Convert.ToInt32(values["Mandatory"]) == 0)
            {
                removed = true;
            }

            if (removed)
            {
                throw new ValidationException("Deactivate the programme before removing or moving its active requirements or interest mappings.");
            }
        }

        private bool UsesCategory(DegreeProgramme programme, int categoryId)
        {
            foreach (InterestWeight weight in programme.Interests)
            {
                if (weight.CategoryId == categoryId)
                {
                    return true;
                }
            }

            return false;
        }

        private void ValidateQuestion(int id, Dictionary<string, object?> values, bool active, List<DegreeProgramme> programmes)
        {
            InterestQuestion? old = null;
            foreach (InterestQuestion question in store.GetQuestions(false))
            {
                if (question.Id == id)
                {
                    old = question;
                    break;
                }
            }

            if (old == null || (active && old.CategoryId == Convert.ToInt32(values["CategoryId"])))
            {
                return;
            }

            if (HasQuestion(old.CategoryId, id))
            {
                return;
            }

            foreach (DegreeProgramme programme in programmes)
            {
                if (programme.Active && UsesCategory(programme, old.CategoryId))
                {
                    throw new ValidationException("Deactivate affected programmes before removing their last active interest question.");
                }
            }
        }

        private void ValidateDeactivation(string key, int id, List<DegreeProgramme> programmes)
        {
            foreach (DegreeProgramme programme in programmes)
            {
                if (!programme.Active)
                {
                    continue;
                }

                bool used = false;
                if (key == "Subject")
                {
                    foreach (ProgrammeRequirement rule in programme.Requirements)
                    {
                        if (rule.SubjectId == id)
                        {
                            used = true;
                        }
                    }
                }
                else
                {
                    used = UsesCategory(programme, id);
                }

                if (used)
                {
                    throw new ValidationException("Deactivate affected degree programmes before deactivating a referenced subject or interest category.");
                }
            }
        }

        public void Backup(string path)
        {
            access.RequireAdmin();
            store.Backup(path);
            store.Audit(access.UserId, "Database backed up", "Local database backup created", clock.UtcNow);
        }
    }
}
