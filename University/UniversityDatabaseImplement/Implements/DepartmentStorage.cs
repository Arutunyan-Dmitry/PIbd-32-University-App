using UniversityContracts.BindingModels;
using UniversityContracts.StorageContracts;
using UniversityContracts.ViewModels;
using UniversityDatabaseImplement.Models;

namespace UniversityDatabaseImplement.Implements
{
    public class DepartmentStorage : IDepartmentStorage
    {
        public List<DepartmentViewModel> GetFullList()
        {
            using var context = new UniversityDatabase();
            return context.Departments
            .Select(CreateModel)
            .ToList();
        }
        public DepartmentViewModel GetElement(DepartmentBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using var context = new UniversityDatabase();
            var department = context.Departments
            .FirstOrDefault(rec => rec.Login == model.Login || rec.Id == model.Id);
            return department != null ? CreateModel(department) : null;
        }
        public void Insert(DepartmentBindingModel model)
        {
            using var context = new UniversityDatabase();
            context.Departments.Add(CreateModel(model, new Department()));
            context.SaveChanges();
        }
        public void Update(DepartmentBindingModel model)
        {
            using var context = new UniversityDatabase();
            var element = context.Departments.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            CreateModel(model, element);
            context.SaveChanges();
        }
        public void Delete(DepartmentBindingModel model)
        {
            using var context = new UniversityDatabase();
            Department? element = context.Departments.FirstOrDefault(rec => rec.Id == model.Id);
            if (element != null)
            {
                context.Departments.Remove(element);
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
        private static Department CreateModel(DepartmentBindingModel model, Department department)
        {
            department.Name = model.Name;
            department.Login = model.Login;
            department.Password = model.Password;
            return department;
        }
        private static DepartmentViewModel CreateModel(Department department)
        {
            return new DepartmentViewModel
            {
                Id = department.Id,
                Name = department.Name,
                Login = department.Login,
                Password = department.Password
            };
        }
    }
}
