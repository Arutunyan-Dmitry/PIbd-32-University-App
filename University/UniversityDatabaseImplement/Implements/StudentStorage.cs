using UniversityContracts.BindingModels;
using UniversityContracts.StorageContracts;
using UniversityContracts.ViewModels;
using UniversityDatabaseImplement.Models;

namespace UniversityDatabaseImplement.Implements
{
    public class StudentStorage : IStudentStorage
    {
        public List<StudentViewModel> GetFullList()
        {
            using var context = new UniversityDatabase();
            return context.Students
            .Select(CreateModel)
            .ToList();
        }
        public List<StudentViewModel> GetFilteredList(StudentBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using var context = new UniversityDatabase();
            return context.Students
            .Where(rec => rec.DepartmentId == model.DepartmentId || rec.GroupId == model.GroupId)
            .Select(CreateModel)
            .ToList();
        }
        public StudentViewModel GetElement(StudentBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using var context = new UniversityDatabase();
            var student = context.Students
            .FirstOrDefault(rec => rec.Id == model.Id);
            return student != null ? CreateModel(student) : null;
        }
        public void Insert(StudentBindingModel model)
        {
            using var context = new UniversityDatabase();
            context.Students.Add(CreateModel(model, new Student()));
            context.SaveChanges();
        }
        public void Update(StudentBindingModel model)
        {
            using var context = new UniversityDatabase();
            var element = context.Students.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            CreateModel(model, element);
            context.SaveChanges();
        }
        public void Delete(StudentBindingModel model)
        {
            using var context = new UniversityDatabase();
            Student? element = context.Students.FirstOrDefault(rec => rec.Id == model.Id);
            if (element != null)
            {
                context.Students.Remove(element);
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
        private static Student CreateModel(StudentBindingModel model, Student student)
        {
            using var context = new UniversityDatabase();
            int? idLastStudent;
            if (context.Students.ToList().Count() > 0)
            {
                idLastStudent = context.Students.OrderBy(x => x.Id).Last().Id;
            }
            else
            {
                idLastStudent = 0;
            }
            student.DepartmentId = model.DepartmentId.Value;
            student.GroupId = model.GroupId;
            student.Flm = model.Flm;
            student.NumFB = DateTime.Now.Year.ToString() + '/' + (idLastStudent + 1).ToString();
            student.Basement = model.Basement;
            return student;
        }
        private static StudentViewModel CreateModel(Student student)
        {
            return new StudentViewModel
            {
                Id = student.Id,
                DepartmentId = student.DepartmentId,
                Basement = student.Basement,
                NumFB = student.NumFB,
                GroupId = student.GroupId,
                Flm = student.Flm
            };
        }
    }
}
