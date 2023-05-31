using UniversityContracts.Enums;

namespace UniversityContracts.ViewModels
{
    public class PlanViewModel
    {
        public int Id { get; set; }
        public int DepartmentId { get; set; }
        public int TeacherId { get; set; }
        public int DisciplineId { get; set; }
        public string DisciplineName { get; set; }
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public string Name { get; set; }
        public int Hours { get; set; }
        public PlanType Type { get; set; }
        public List<Tuple<int, string, int, DateTime>> Testings { get; set; }
    }
}
