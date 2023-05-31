using System.ComponentModel.DataAnnotations;
using UniversityContracts.Enums;

namespace UniversityDatabaseImplement.Models
{
    public class Message
    {
        public int Id { get; set; }
        [Required]
        public ReportTypes ReportType { get; set; }
        [Required]
        public Status Status { get; set; }
        public int DepartmentId { get; set; }
        public int PlanId { get; set; }
        public int TeacherId { get; set; }
        public int DisciplineId { get; set; }
        public virtual Department Department { get; set; }
    }
}
