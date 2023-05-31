using UniversityContracts.Enums;

namespace UniversityContracts.BindingModels
{
    public class MessageBindingModel
    {
        public int? Id { get; set; }
        public ReportTypes ReportType { get; set; }
        public Status? Status { get; set; }
        public int DepartmentId { get; set; }
        public int? PlanId { get; set; }
        public int? TeacherId { get; set; }
        public int? DisciplineId { get; set; }
    }
}
