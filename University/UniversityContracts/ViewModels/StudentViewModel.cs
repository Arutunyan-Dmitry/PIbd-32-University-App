using UniversityContracts.Enums;

namespace UniversityContracts.ViewModels
{
    public class StudentViewModel
    {
        public int Id { get; set; }
        public int DepartmentId { get; set; }
        public int GroupId { get; set; }
        public string Flm { get; set; }
        public string NumFB { get; set; }
        public TypeEducationBasement Basement { get; set; }
    }
}
