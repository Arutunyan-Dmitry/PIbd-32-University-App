using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityDatabaseImplement.Models
{
    public class Group
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Course { get; set; }
        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }
        [ForeignKey("GroupId")]
        public virtual List<Student> Students { get; set; }
        [ForeignKey("GroupId")]
        public virtual List<Plan> Plans { get; set; }
    }
}
