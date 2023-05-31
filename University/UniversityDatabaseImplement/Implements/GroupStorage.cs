using UniversityContracts.BindingModels;
using UniversityContracts.StorageContracts;
using UniversityContracts.ViewModels;
using UniversityDatabaseImplement.Models;

namespace UniversityDatabaseImplement.Implements
{
    public class GroupStorage : IGroupStorage
    {
        public List<GroupViewModel> GetFullList()
        {
            using var context = new UniversityDatabase();
            return context.Groups
            .Select(CreateModel)
            .ToList();
        }
        public List<GroupViewModel> GetFilteredList(GroupBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using var context = new UniversityDatabase();
            return context.Groups
            .Where(rec => rec.DepartmentId == model.DepartmentId)
            .Select(CreateModel)
            .ToList();
        }
        public GroupViewModel GetElement(GroupBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using var context = new UniversityDatabase();
            var group = context.Groups
            .FirstOrDefault(rec => (rec.Name == model.Name &&
                                    rec.DepartmentId == model.DepartmentId)
                                    || rec.Id == model.Id);
            return group != null ? CreateModel(group) : null;
        }
        public void Insert(GroupBindingModel model)
        {
            using var context = new UniversityDatabase();
            context.Groups.Add(CreateModel(model, new Group()));
            context.SaveChanges();
        }
        public void Update(GroupBindingModel model)
        {
            using var context = new UniversityDatabase();
            var element = context.Groups.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            CreateModel(model, element);
            context.SaveChanges();
        }
        public void Delete(GroupBindingModel model)
        {
            using var context = new UniversityDatabase();
            Group? element = context.Groups.FirstOrDefault(rec => rec.Id == model.Id);
            if (element != null)
            {
                context.Groups.Remove(element);
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
        private static Group CreateModel(GroupBindingModel model, Group group)
        {
            group.DepartmentId = model.DepartmentId;
            group.Name = model.Name;
            group.Course = model.Course;
            return group;
        }
        private static GroupViewModel CreateModel(Group group)
        {
            return new GroupViewModel
            {
                Id = group.Id,
                DepartmentId = group.DepartmentId,
                Name = group.Name,
                Course = group.Course
            };
        }
    }
}
