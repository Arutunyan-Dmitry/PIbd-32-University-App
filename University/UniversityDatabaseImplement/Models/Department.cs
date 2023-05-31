using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityDatabaseImplement.Models
{
    public class Department
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
        [ForeignKey("DepartmentId")]
        public virtual List<Teacher> Teachers { get; set; }
        [ForeignKey("DepartmentId")]
        public virtual List<Discipline> Disciplines { get; set; }
        [ForeignKey("DepartmentId")]
        public virtual List<Plan> Plans { get; set; }
        [ForeignKey("DepartmentId")]
        public virtual List<Group> Groups { get; set; }
        [ForeignKey("DepartmentId")]
        public virtual List<Message> Messages { get; set; }
    }
}
