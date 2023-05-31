using UniversityContracts.Enums;

namespace UniversityContracts.ViewModels
{
    public class ReportPlanViewModel
    {
        public string Title { get; set; }
        public string Footer { get; set; }
        public string PlanName { get; set; }
        public List<Tuple<string, List<Tuple<string, MarkType>>>> Items { get; set;}
    }
}
