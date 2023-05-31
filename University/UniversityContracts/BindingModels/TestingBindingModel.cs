using UniversityContracts.Enums;

namespace UniversityContracts.BindingModels
{
    public class TestingBindingModel
    {
        public int? Id { get; set; }
        public int PlanId { get; set; }
        public string Topic { get; set; }
        public int Hours { get; set; }
        public DateTime Date { get; set; }
        public PlanType? Type { get; set; }
        public Dictionary<int, MarkType>? StudentTestings { get; set; }
    }
}
