using UniversityContracts.Enums;

namespace UniversityContracts.ViewModels
{
    public class ReportFullViewModel
    {
        public string Title { get; set; }
        public List<string> Footer { get; set; }
        public List<string> DisciplineName { get; set; }
        public List<Tuple<List<string>>> Itog { get; set; }
        public List<Tuple<string, List<Tuple<string, List<Tuple<string, List<Tuple<string, MarkType>>>>>>>> Items { get; set; }
    }
}
