using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityDatabaseImplement.Models
{
    public class Teacher
    {
        public int Id { get; set; }
        [Required]
        public string Flm { get; set; }
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }
        [ForeignKey("TeacherId")]
        public virtual List<TeacherDiscipline> TeacherDisciplines { get; set; }
        [ForeignKey("TeacherId")]
        public virtual List<Plan> Plans { get; set; }
    }
}
