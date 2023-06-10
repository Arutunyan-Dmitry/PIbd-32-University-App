using UniversityContracts.Enums;

namespace UniversityContracts.ViewModels
{
    public class ReportGroupViewModel
    {
        public string Mail { get; set; }
        public string Title { get; set; }
        public List<string> Upper { get; set; }
        public List<string> Footer { get; set; }
        public List<Tuple<string, string, TypeEducationBasement>> Items { get; set; }
    }
}
