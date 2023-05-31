using UniversityContracts.BindingModels;
using UniversityContracts.BusinessLogicContracts;
using UniversityContracts.StorageContracts;
using UniversityContracts.ViewModels;

namespace UniversityBusinessLogic.BusinessLogic
{
    public class GroupLogic : IGroupLogic
    {
        private readonly IGroupStorage _groupStorage;
        public GroupLogic(IGroupStorage groupStorage)
        {
            _groupStorage = groupStorage;
        }
        public List<GroupViewModel> Read(GroupBindingModel model)
        {
            if (model == null)
            {
                return _groupStorage.GetFullList();
            }
            if (model.Id.HasValue || (model.DepartmentId != null &&
                model.Name != null))
            {
                return new List<GroupViewModel> { _groupStorage.GetElement(model) };
            }
            return _groupStorage.GetFilteredList(model);
        }
        public void CreateOrUpdate(GroupBindingModel model)
        {
            var element = _groupStorage.GetElement(new GroupBindingModel
            {
                DepartmentId = model.DepartmentId,
                Name = model.Name,
                Course = model.Course
            });
            if (element != null && element.Id != model.Id)
            {
                throw new Exception("Уже есть такой элемент");
            }
            if (model.Id.HasValue)
            {
                _groupStorage.Update(model);
            }
            else
            {
                _groupStorage.Insert(model);
            }
        }
        public void Delete(GroupBindingModel model)
        {
            var element = _groupStorage.GetElement(new GroupBindingModel
            {
                Id = model.Id
            });
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            _groupStorage.Delete(model);
        }
    }
}
