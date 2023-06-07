using Microsoft.EntityFrameworkCore;
using UniversityDatabaseImplement.Models;

namespace UniversityDatabaseImplement
{
    public class UniversityDatabase : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured == false)
            {
                optionsBuilder.UseSqlServer(@"Data Source=ArutunyanWin64\SQLEXPRESS;Initial Catalog=UniversityDatabase;Integrated Security=True;MultipleActiveResultSets=True;");
            }
            base.OnConfiguring(optionsBuilder);
        }
        public virtual DbSet<Department> Departments { set; get; }
        public virtual DbSet<Teacher> Teachers { get; set; }
        public virtual DbSet<TeacherDiscipline> TeacherDisciplines { get; set; }
        public virtual DbSet<Discipline> Disciplines { set; get; }
        public virtual DbSet<Plan> Plans { set; get; }
        public virtual DbSet<Group> Groups { set; get; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<StudentTesting> StudentTestings { get; set; }
        public virtual DbSet<Testing> Testings { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
    }
}
