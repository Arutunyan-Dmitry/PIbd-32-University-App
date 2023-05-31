using UniversityContracts.Enums;

namespace UniversityContracts.ViewModels
{
    public class MessageViewModel
    {
        public int Id { get; set; }
        public ReportTypes ReportType { get; set; }
        public Status Status { get; set; }
        public int DepartmentId { get; set; }
        public int PlanId { get; set; }
        public string PlanName { get; set; }
        public int TeacherId { get; set; }
        public int DisciplineId { get; set; }
        public string DisciplineName { get; set; }
    }
}
