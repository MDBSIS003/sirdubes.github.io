namespace FuturePathStudentAdvisingSystem.Models
{
    public class DegreeProgramme
    {
        public int DegreeID { get; set; }

        public int FacultyID { get; set; }

        public string ProgrammeName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public string FacultyName { get; set; } = string.Empty;
    }
}