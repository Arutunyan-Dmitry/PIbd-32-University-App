using UniversityContracts.Enums;

namespace UniversityContracts.ViewModels
{
    public class ReportPlanViewModel
    {
        public string Title { get; set; }
        public List<string> Footer { get; set; }
        public List<string> PlanName { get; set; }
        public List<string> Itog { get; set; }
        public List<Tuple<string, string, List<Tuple<string, MarkType>>>> Items { get; set;}
    }
}
