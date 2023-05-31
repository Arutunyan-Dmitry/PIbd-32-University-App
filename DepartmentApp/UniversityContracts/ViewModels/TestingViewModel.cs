using UniversityContracts.Enums;

namespace UniversityContracts.ViewModels
{
    public class TestingViewModel
    {
        public int? Id { get; set; }
        public int PlanId { get; set; }
        public string Topic { get; set; }
        public int Hours { get; set; }
        public DateTime Date { get; set; }
        public PlanType Type { get; set; }
        public List<Tuple<int, string, MarkType>> StudentTestings { get; set; }
    }
}
