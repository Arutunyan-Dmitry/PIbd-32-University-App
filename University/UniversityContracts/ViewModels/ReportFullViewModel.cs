using UniversityContracts.Enums;

namespace UniversityContracts.ViewModels
{
    public class ReportFullViewModel
    {
        public string Title { get; set; }
        public string Footer { get; set; }
        public string DisciplineName { get; set; }
        public List<Tuple<string, List<Tuple<string, List<Tuple<string, List<Tuple<string, MarkType>>>>>>>> Items { get; set; }
    }
}
