using UniversityContracts.BindingModels;
using UniversityContracts.Enums;
using UniversityContracts.StorageContracts;
using UniversityContracts.ViewModels;
using UniversityDatabaseImplement.Models;

namespace UniversityDatabaseImplement.Implements
{
    public class PlanStorage : IPlanStorage
    {
        public List<PlanViewModel> GetFullList()
        {
            using var context = new UniversityDatabase();
            return context.Plans
            .Select(CreateModel)
            .ToList();
        }
        public List<PlanViewModel> GetFilteredList(PlanBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using var context = new UniversityDatabase();
            return context.Plans
            .Where(rec => (rec.DepartmentId == model.DepartmentId) || 
                          (rec.TeacherId == model.TeacherId && rec.DisciplineId == model.DisciplineId && 
                          rec.GroupId == model.GroupId && rec.Type == model.Type) ||
                          (rec.TeacherId == model.TeacherId && rec.DisciplineId == model.DisciplineId && 
                          model.GroupId == 0) || 
                          (rec.TeacherId == model.TeacherId && rec.GroupId == model.GroupId && 
                          rec.DisciplineId == model.DisciplineId && model.Type == null) ||
                          (rec.TeacherId == model.TeacherId && rec.Type == model.Type &&
                          model.GroupId == 0 && model.DisciplineId == 0) ||
                          (rec.TeacherId == model.TeacherId && model.DisciplineId == 0 && model.GroupId == 0) ||
                          (rec.TeacherId == 0 && rec.GroupId == 0 && rec.DisciplineId == model.DisciplineId))
                            .Skip(model.ToSkip ?? 0)
                            .Take(model.ToTake ?? context.Plans.Count())
            .Select(CreateModel)
            .ToList();
        }

        public List<PlanViewModel> GetPlansByDiscipline(int id)
        {
            using var context = new UniversityDatabase();
            return context.Plans
                .Where(rec => rec.Discipline.Id == id)
                .Select(CreateModel)
                .ToList();
        }

        public PlanViewModel GetElement(PlanBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using var context = new UniversityDatabase();
            var lesson = context.Plans
            .FirstOrDefault(rec => rec.Id == model.Id || 
            (rec.DepartmentId == model.DepartmentId && rec.Name == model.Name));
            return lesson != null ? CreateModel(lesson) : null;
        }
        public void Insert(PlanBindingModel model)
        {
            using var context = new UniversityDatabase();
            context.Plans.Add(CreateModel(model, new Plan()));
            context.SaveChanges();
        }
        public void Update(PlanBindingModel model)
        {
            using var context = new UniversityDatabase();
            var element = context.Plans.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            CreateModel(model, element);
            context.SaveChanges();
        }
        public void Delete(PlanBindingModel model)
        {
            using var context = new UniversityDatabase();
            Plan? element = context.Plans.FirstOrDefault(rec => rec.Id == model.Id);
            if (element != null)
            {
                context.Plans.Remove(element);
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
        private static Plan CreateModel(PlanBindingModel model, Plan plan)
        {
            plan.DepartmentId = model.DepartmentId;
            plan.TeacherId = model.TeacherId;
            plan.GroupId = model.GroupId;
            plan.DisciplineId = model.DisciplineId;
            plan.Name = model.Name;
            plan.Hours = model.Hours;
            plan.Type = (PlanType)model.Type;
            return plan;
        }
        private static PlanViewModel CreateModel(Plan plan)
        {
            using var context = new UniversityDatabase();
            List<Testing> testings = context.Testings
                .Where(rec => rec.PlanId == plan.Id)
                .ToList();

            Discipline? discipline = context.Disciplines.FirstOrDefault(rec => rec.Id == plan.DisciplineId);
            Group? group = context.Groups.FirstOrDefault(rec => rec.Id == plan.GroupId);

            PlanViewModel model = new PlanViewModel()
            {
                Id = plan.Id,
                TeacherId = plan.TeacherId,
                GroupId = plan.GroupId,
                DisciplineId = plan.DisciplineId,
                DepartmentId = plan.DepartmentId,
                Name = plan.Name,
                Hours = plan.Hours,
                Type = plan.Type,
                Testings = new List<Tuple<int, string, int, DateTime>>()
            };

            if(discipline != null)
            {
                model.DisciplineName = discipline.Name;
            }

            if(group != null)
            {
                model.GroupName = group.Name;
            }

            if (testings != null)
            {
                foreach (var test in testings)
                {
                    model.Testings.Add(new Tuple<int, string, int, DateTime>( test.Id, test.Topic, test.Hours, test.Date ));
                }
            }

            return model;
        }
    }
}
