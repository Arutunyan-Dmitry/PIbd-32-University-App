using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UniversityContracts.Enums;

namespace UniversityDatabaseImplement.Models
{
    public class Student
    {
        public int Id { get; set; }
        public int DepartmentId { get; set; }
        public string NumFB { get; set; }
        [Required]
        public string Flm { get; set; }
        public TypeEducationBasement Basement { get; set; }
        public int GroupId { get; set; }
        public virtual Group Group { get; set; }
        [ForeignKey("StudentId")]
        public virtual List<StudentTesting> StudentTestings { get; set; }
    }
}
