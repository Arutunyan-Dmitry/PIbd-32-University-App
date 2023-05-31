namespace UniversityContracts.BindingModels
{
    public class GroupBindingModel
    {
        public int? Id { get; set; }
        public int DepartmentId { get; set; }
        public string Name { get; set; }
        public int Course { get; set; }
    }
}
