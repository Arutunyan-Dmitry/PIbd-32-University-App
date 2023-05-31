using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityDatabaseImplement.Models
{
    public class Discipline
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }
        [ForeignKey("DisciplineId")]
        public virtual List<TeacherDiscipline> TeacherDisciplines { get; set; }
        [ForeignKey("DisciplineId")]
        public virtual List<Plan> Plans { get; set; }
    }
}
