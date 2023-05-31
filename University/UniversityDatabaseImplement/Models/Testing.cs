using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UniversityContracts.Enums;

namespace UniversityDatabaseImplement.Models
{
    public class Testing
    {
        public int Id { get; set; }
        [Required]
        public string Topic { get; set; }
        [Required]
        public int Hours { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public PlanType Type { get; set; }
        [Required]
        public int PlanId { get; set; }
        public virtual Plan Plan { get; set; }
        [ForeignKey("TestingId")]
        public virtual List<StudentTesting> StudentTestings { get; set;}
    }
}
