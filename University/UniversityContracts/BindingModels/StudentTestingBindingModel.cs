using UniversityContracts.Enums;

namespace UniversityContracts.BindingModels
{
    public class StudentTestingBindingModel
    {
        public int TestingId { get; set; }
        public int StudentId { get; set; }
        public MarkType Grade { get; set; }
    }
}
