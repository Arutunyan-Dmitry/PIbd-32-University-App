namespace UniversityDatabaseImplement.Models
{
    public class TeacherDiscipline
    {
        public int Id { get; set; }
        public int TeacherId { get; set; }
        public int DisciplineId { get; set; }
        public virtual Teacher Teacher { get; set; }
        public virtual Discipline Discipline { get; set; }
    }
}
