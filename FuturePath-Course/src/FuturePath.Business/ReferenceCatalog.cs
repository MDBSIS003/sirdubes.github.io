using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace FuturePath.Business
{
    /// <summary>Whitelisted reference entities shared by typed validation, data access and form editors.</summary>
    public static class ReferenceCatalog
    {
        public static readonly string[] Provinces = new string[]
        {
            "Eastern Cape",
            "Free State",
            "Gauteng",
            "KwaZulu-Natal",
            "Limpopo",
            "Mpumalanga",
            "Northern Cape",
            "North West",
            "Western Cape"
        };
        public static readonly string[] FundingTypes = new string[]
        {
            "Scholarship",
            "Bursary",
            "Financial aid",
            "External funding",
            "Merit award"
        };
        private static readonly IReadOnlyList<ReferenceDefinition> definitions = new ReferenceDefinition[]
        {
            new ReferenceDefinition("Faculty", "Faculties", new ReferenceField[] {
                Name(),
                Active()
            }),
            new ReferenceDefinition("Subject", "School subjects", new ReferenceField[] {
                Name(),
                Active()
            }),
            new ReferenceDefinition("InterestCategory", "Interest categories", new ReferenceField[] {
                Name(),
                Active()
            }),
            new ReferenceDefinition("DegreeProgramme", "Degree programmes", new ReferenceField[] {
                Name(),
                Link("FacultyId", "Faculty", "Faculty"),
                new ReferenceField("Description", "Description", FieldKind.LongText),
                new ReferenceField("MinimumAverage", "Minimum overall average (%)", FieldKind.Decimal),
                Active()
            }),
            new ReferenceDefinition("ProgrammeRequirement", "Admission requirements", new ReferenceField[] {
                Link("DegreeId", "Degree", "DegreeProgramme"),
                Link("SubjectId", "Subject", "Subject"),
                new ReferenceField("MinimumMark", "Minimum mark (%)", FieldKind.Decimal),
                new ReferenceField("Mandatory", "Mandatory", FieldKind.Boolean),
                new ReferenceField("AlternativeGroup", "OR group (0 = independent)", FieldKind.Integer, Maximum: 99),
                new ReferenceField("Note", "Advisory note", FieldKind.LongText, false),
                Active()
            }),
            new ReferenceDefinition("InterestQuestion", "Questionnaire", new ReferenceField[] {
                new ReferenceField("Text", "Question", FieldKind.LongText, MaximumLength: 400),
                Link("CategoryId", "Category", "InterestCategory"),
                Active()
            }),
            new ReferenceDefinition("DegreeInterestMapping", "Degree interest weights", new ReferenceField[] {
                Link("DegreeId", "Degree", "DegreeProgramme"),
                Link("CategoryId", "Category", "InterestCategory"),
                new ReferenceField("Weight", "Weight (0.01–10)", FieldKind.Decimal, Minimum: .01m, Maximum: 10),
                Active()
            }),
            new ReferenceDefinition("FundingOption", "Funding opportunities", new ReferenceField[] {
                Name(),
                new ReferenceField("Type", "Funding type", FieldKind.Choice, Choices: FundingTypes),
                new ReferenceField("Eligibility", "Eligibility / merit / financial need", FieldKind.LongText),
                new ReferenceField("Documents", "Required documents", FieldKind.LongText),
                new ReferenceField("ClosingDate", "Closing date", FieldKind.Date),
                new ReferenceField("Contact", "Contact / application information", FieldKind.LongText),
                new ReferenceField("Sample", "Sample / requires verification", FieldKind.Boolean),
                Active()
            }),
            new ReferenceDefinition("FundingFaculty", "Funding ↔ faculties", new ReferenceField[] {
                Link("FundingId", "Funding", "FundingOption"),
                Link("FacultyId", "Faculty", "Faculty"),
                Active()
            }),
            new ReferenceDefinition("FundingProgramme", "Funding ↔ programmes", new ReferenceField[] {
                Link("FundingId", "Funding", "FundingOption"),
                Link("DegreeId", "Degree", "DegreeProgramme"),
                Active()
            })
        };
        private static ReferenceField Active()
        {
            return new ReferenceField("Active", "Active", FieldKind.Boolean);
        }

        private static ReferenceField Name()
        {
            return new ReferenceField("Name", "Name");
        }

        private static ReferenceField Link(string name, string label, string table)
        {
            return new ReferenceField(name, label, FieldKind.Lookup, Lookup: table);
        }

        public static IReadOnlyList<ReferenceDefinition> Definitions
        {
            get { return definitions; }
        }

        public static ReferenceDefinition Get(string key)
        {
            foreach (ReferenceDefinition definition in Definitions)
            {
                if (definition.Key == key)
                {
                    return definition;
                }
            }

            throw new ValidationException("Unknown reference-data area.");
        }
    }
}
