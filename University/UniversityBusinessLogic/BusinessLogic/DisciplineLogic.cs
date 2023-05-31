using UniversityContracts.BindingModels;
using UniversityContracts.BusinessLogicContracts;
using UniversityContracts.StorageContracts;
using UniversityContracts.ViewModels;

namespace UniversityBusinessLogic.BusinessLogic
{
    public class DisciplineLogic : IDisciplineLogic
    {
        private readonly IDisciplineStorage _disciplineStorage;
        public DisciplineLogic(IDisciplineStorage disciplineStorage)
        {
            _disciplineStorage = disciplineStorage;
        }
        public List<DisciplineViewModel> Read(DisciplineBindingModel model)
        {
            if (model == null)
            {
                return _disciplineStorage.GetFullList();
            }
            if (model.Id.HasValue || (model.DepartmentId != null &&
                model.Name != null))
            {
                return new List<DisciplineViewModel> { _disciplineStorage.GetElement(model) };
            }
            return _disciplineStorage.GetFilteredList(model);
        }
        public void CreateOrUpdate(DisciplineBindingModel model)
        {
            var element = _disciplineStorage.GetElement(new DisciplineBindingModel
            {
                DepartmentId = model.DepartmentId,
                Name = model.Name
            });
            if (element != null && element.Id != model.Id)
            {
                throw new Exception("Уже есть такой элемент");
            }
            if (model.Id.HasValue)
            {
                _disciplineStorage.Update(model);
            }
            else
            {
                _disciplineStorage.Insert(model);
            }
        }
        public void Delete(DisciplineBindingModel model)
        {
            var element = _disciplineStorage.GetElement(new DisciplineBindingModel
            {
                Id = model.Id
            });
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            _disciplineStorage.Delete(model);
        }
    }
}
