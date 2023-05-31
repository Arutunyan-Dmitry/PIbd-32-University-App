using UniversityContracts.Enums;

namespace UniversityContracts.BindingModels
{
    public class StudentBindingModel
    {
        public int? Id { get; set; }
        public int? DepartmentId { get; set; }
        public int GroupId { get; set; }
        public string Flm { get; set; }
        public string? NumFB { get; set; }
        public TypeEducationBasement Basement { get; set; }

    }
}
