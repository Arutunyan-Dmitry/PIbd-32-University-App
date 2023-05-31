using UniversityContracts.BindingModels;
using UniversityContracts.StorageContracts;
using UniversityContracts.ViewModels;
using UniversityDatabaseImplement.Models;

namespace UniversityDatabaseImplement.Implements
{
    public class DisciplineStorage : IDisciplineStorage
    {
        public List<DisciplineViewModel> GetFullList()
        {
            using var context = new UniversityDatabase();
            return context.Disciplines
            .Select(CreateModel)
            .ToList();
        }
        public List<DisciplineViewModel> GetFilteredList(DisciplineBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using var context = new UniversityDatabase();
            return context.Disciplines
            .Where(rec => rec.DepartmentId == model.DepartmentId)
            .Select(CreateModel)
            .ToList();
        }
        public DisciplineViewModel GetElement(DisciplineBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using var context = new UniversityDatabase();
            var discipline = context.Disciplines
            .FirstOrDefault(rec => (rec.Name == model.Name && 
                                    rec.DepartmentId == model.DepartmentId)
                                    || rec.Id == model.Id);
            return discipline != null ? CreateModel(discipline) : null;
        }
        public void Insert(DisciplineBindingModel model)
        {
            using var context = new UniversityDatabase();
            context.Disciplines.Add(CreateModel(model, new Discipline()));
            context.SaveChanges();
        }
        public void Update(DisciplineBindingModel model)
        {
            using var context = new UniversityDatabase();
            var element = context.Disciplines.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            CreateModel(model, element);
            context.SaveChanges();
        }
        public void Delete(DisciplineBindingModel model)
        {
            using var context = new UniversityDatabase();
            Discipline? element = context.Disciplines.FirstOrDefault(rec => rec.Id == model.Id);
            if (element != null)
            {
                context.Disciplines.Remove(element);
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
        private static Discipline CreateModel(DisciplineBindingModel model, Discipline discipline)
        {
            discipline.DepartmentId = model.DepartmentId;
            discipline.Name = model.Name;
            return discipline;
        }
        private static DisciplineViewModel CreateModel(Discipline discipline)
        {
            return new DisciplineViewModel
            {
                Id = discipline.Id,
                DepartmentId = discipline.DepartmentId,
                Name = discipline.Name,
            };
        }
    }
}
