using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UniversityContracts.Enums;

namespace UniversityDatabaseImplement.Models
{
    public class Plan
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Hours { get; set; }
        [Required]
        public PlanType Type { get; set; }
        [Required]
        public int DepartmentId { get; set; }
        public int TeacherId { get; set; }
        public int DisciplineId { get; set; }
        public int GroupId { get; set; }
        public virtual Department Department { get; set; }
        public virtual Teacher Teacher { get; set; }
        public virtual Discipline Discipline { get; set; }
        public virtual Group Group { get; set; }
        [ForeignKey("PlanId")]
        public virtual List<Testing> Testings { get; set; }
    }
}
