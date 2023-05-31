using Microsoft.EntityFrameworkCore;
using UniversityContracts.BindingModels;
using UniversityContracts.StorageContracts;
using UniversityContracts.ViewModels;
using UniversityDatabaseImplement.Models;

namespace UniversityDatabaseImplement.Implements
{
    public class TeacherStorage : ITeacherStorage
    {
        public List<TeacherViewModel> GetFullList()
        {
            using var context = new UniversityDatabase();
            return context.Teachers
            .Include(rec => rec.TeacherDisciplines)
            .ThenInclude(rec => rec.Discipline)
            .ToList()
            .Select(CreateModel)
            .ToList();
        }
        public List<TeacherViewModel> GetFilteredList(TeacherBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using var context = new UniversityDatabase();
            return context.Teachers
            .Include(rec => rec.TeacherDisciplines)
            .ThenInclude(rec => rec.Discipline)
            .Where(rec => (rec.Login == model.Login && rec.Password == model.Password) ||
                           rec.DepartmentId == model.DepartmentId)
            .Select(CreateModel)
            .ToList();
        }

        public List<TeacherViewModel> GetTeacherByDiscipline(int id)
        {
            using var context = new UniversityDatabase();
            var teacherDisciplines = context.TeacherDisciplines.Where(rec => rec.DisciplineId == id).ToList();
            List<TeacherViewModel> teachers = context.Teachers.Include(rec => rec.TeacherDisciplines).ThenInclude(rec => rec.Discipline).ToList()
            .Select(CreateModel)
            .ToList();
            List<TeacherViewModel> result = new List<TeacherViewModel>();
            if (teacherDisciplines.Count() > 0)
            {
                foreach (var discipline in teacherDisciplines)
                {
                    foreach (var teacher in teachers)
                    {
                        if (discipline.TeacherId == teacher.Id)
                        {
                            result.Add(teacher);
                        }
                    }
                }
            }
            return result;
        }

        public TeacherViewModel GetElement(TeacherBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using var context = new UniversityDatabase();
            var teacher = context.Teachers
            .Include(rec => rec.TeacherDisciplines)
            .ThenInclude(rec => rec.Discipline)
            .FirstOrDefault(rec => rec.Login == model.Login || rec.Id == model.Id);
            return teacher != null ? CreateModel(teacher) : null;
        }
        public void Insert(TeacherBindingModel model)
        {
            using var context = new UniversityDatabase();
            using var transaction = context.Database.BeginTransaction();
            try
            {
                Teacher teacher = new Teacher()
                {
                    DepartmentId = model.DepartmentId,
                    Flm = model.Flm,
                    Login = model.Login,
                    Password = model.Password
                };
                context.Teachers.Add(teacher);
                context.SaveChanges();
                CreateModel(model, teacher, context);
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
        public void Update(TeacherBindingModel model)
        {
            using var context = new UniversityDatabase();
            using var transaction = context.Database.BeginTransaction();
            try
            {
                var element = context.Teachers.FirstOrDefault(rec => rec.Id == model.Id);
                if (element == null)
                {
                    throw new Exception("Элемент не найден");
                }
                CreateModel(model, element, context);
                context.SaveChanges();
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
        public void Delete(TeacherBindingModel model)
        {
            using var context = new UniversityDatabase();
            Teacher? element = context.Teachers.FirstOrDefault(rec => rec.Id == model.Id);
            if (element != null)
            {
                context.Teachers.Remove(element);
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
        private static Teacher CreateModel(TeacherBindingModel model, Teacher teacher, UniversityDatabase context)
        {
            teacher.DepartmentId = model.DepartmentId;
            teacher.Flm = model.Flm;
            teacher.Login = model.Login;
            teacher.Password = model.Password;
            if (model.TeacherDisciplines == null)
            {
                return teacher;
            }
            if (model.Id.HasValue)
            {
                var teacherDisciplines = context.TeacherDisciplines.Where(rec => rec.TeacherId == model.Id.Value).ToList();
                context.TeacherDisciplines.RemoveRange(teacherDisciplines.Where(rec =>
               !model.TeacherDisciplines.Contains(rec.DisciplineId)).ToList());
                context.SaveChanges();
            }
            foreach (var td in model.TeacherDisciplines)
            {
                if (context.TeacherDisciplines.Where(rec => rec.TeacherId == teacher.Id &&
                    rec.DisciplineId == td).ToList().Count() == 0) {
                    context.TeacherDisciplines.Add(new TeacherDiscipline
                    {
                        TeacherId = teacher.Id,
                        DisciplineId = td,
                    });
                }
                context.SaveChanges();
            }
            return teacher;
        }
        private static TeacherViewModel CreateModel(Teacher teacher)
        {
            return new TeacherViewModel
            {
                Id = teacher.Id,
                DepartmentId = teacher.DepartmentId,
                Flm = teacher.Flm,
                Login = teacher.Login,
                Password = teacher.Password,
                TeacherDisciplines = teacher.TeacherDisciplines
            .ToDictionary(recTD => recTD.DisciplineId,
            recTD => (recTD.Discipline?.Name))
            };
        }
    }
}
