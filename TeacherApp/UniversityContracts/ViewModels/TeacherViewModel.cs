namespace UniversityContracts.ViewModels
{
    public class TeacherViewModel
    {
        public int Id { get; set; }
        public int DepartmentId { get; set; }
        public string Flm { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public Dictionary<int, string> TeacherDisciplines { get; set; }
    }
}
