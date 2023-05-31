using UniversityContracts.Enums;

namespace UniversityContracts.BindingModels
{
    public class PlanBindingModel
    {
        public int? Id { get; set; }
        public int DepartmentId { get; set; }
        public int TeacherId { get; set; }
        public int DisciplineId { get; set; }
        public int GroupId { get; set; }
        public string Name { get; set; }
        public int Hours { get; set; }
        public PlanType Type { get; set; }
        public int? ToSkip { get; set; }
        public int? ToTake { get; set; }
    }
}
