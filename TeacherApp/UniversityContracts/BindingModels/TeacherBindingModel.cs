namespace UniversityContracts.BindingModels
{
    public class TeacherBindingModel
    {
        public int? Id { get; set; }
        public int DepartmentId { get; set; }
        public string Flm { get; set; }
        public string? Login { get; set; }
        public string? Password { get; set; }
        public List<int> TeacherDisciplines { get; set; }
    }
}
