using System.ComponentModel.DataAnnotations;
using UniversityContracts.Enums;

namespace UniversityDatabaseImplement.Models
{
    public class StudentTesting
    {
        public int Id { get; set; }
        public int TestingId { get; set; }
        public int StudentId { get; set; }
        [Required]
        public MarkType Grade { get; set; }
        public virtual Testing Testing { get; set; }
        public virtual Student Student { get; set; }
    }
}
