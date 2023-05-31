using UniversityContracts.BindingModels;
using UniversityContracts.BusinessLogicContracts;
using UniversityContracts.StorageContracts;
using UniversityContracts.ViewModels;

namespace UniversityBusinessLogic.BusinessLogic
{
    public class PlanLogic : IPlanLogic
    {
        private readonly IPlanStorage _planStorage;
        private readonly IDisciplineStorage _disciplineStorage;
        private readonly IGroupStorage _groupStorage;
        public PlanLogic(IPlanStorage planStorage, IDisciplineStorage disciplineStorage, 
            IGroupStorage groupStorage)
        {
            _planStorage = planStorage;
            _disciplineStorage = disciplineStorage;
            _groupStorage = groupStorage;
        }
        public List<PlanViewModel> Read(PlanBindingModel model)
        {
            if (model == null)
            {
                return _planStorage.GetFullList();
            }
            if (model.Id.HasValue || (model.DepartmentId != null &&
                model.Name != null))
            {
                return new List<PlanViewModel> { _planStorage.GetElement(model) };
            }
            return _planStorage.GetFilteredList(model);
        }
        public List<PlanViewModel> GetPlansByDiscipline(int id)
        {
            return _planStorage.GetPlansByDiscipline(id);
        }
        public void CreateOrUpdate(PlanBindingModel model)
        {
            var element = _planStorage.GetElement(new PlanBindingModel
            {
                DepartmentId = model.DepartmentId,
                TeacherId = model.TeacherId,
                DisciplineId = model.DisciplineId,
                GroupId = model.GroupId,
                Name = model.Name,
                Hours = model.Hours,
                Type = model.Type
            });
            DisciplineViewModel discipline = _disciplineStorage.GetElement(new DisciplineBindingModel
            {
                Id = model.DisciplineId
            });
            GroupViewModel group = _groupStorage.GetElement(new GroupBindingModel
            {
                Id = model.GroupId
            });
            model.Name = model.Type + " план группы " + group.Name + " по дисциплине " +
                discipline.Name + " с кол-вом часов " + model.Hours;
            if (element != null && element.Id != model.Id)
            {
                throw new Exception("Уже есть такой элемент");
            }
            if (model.Id.HasValue)
            {
                _planStorage.Update(model);
            }
            else
            {
                _planStorage.Insert(model);
            }
        }
        public void Delete(PlanBindingModel model)
        {
            var element = _planStorage.GetElement(new PlanBindingModel
            {
                Id = model.Id
            });
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            _planStorage.Delete(model);
        }
    }
}
