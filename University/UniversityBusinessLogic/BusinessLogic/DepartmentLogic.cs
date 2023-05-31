using System.Text.RegularExpressions;
using UniversityContracts.BindingModels;
using UniversityContracts.BusinessLogicContracts;
using UniversityContracts.StorageContracts;
using UniversityContracts.ViewModels;

namespace UniversityBusinessLogic.BusinessLogic
{
    public class DepartmentLogic : IDepartmentLogic
    {
        private readonly IDepartmentStorage _departmentStorage;
        private readonly int _passwordMaxLength = 50;
        private readonly int _passwordMinLength = 10;

        public DepartmentLogic(IDepartmentStorage departmentStorage)
        {
            _departmentStorage = departmentStorage;
        }
        public List<DepartmentViewModel> Read(DepartmentBindingModel model)
        {
            if (model == null)
            {
                return _departmentStorage.GetFullList();
            }
            if (model.Id.HasValue || (model.Login != null && model.Password != null))
            {
                return new List<DepartmentViewModel> { _departmentStorage.GetElement(model) };
            }
            throw new Exception("Элемент не найден");
        }
        public DepartmentViewModel? Authorization(string login, string password)
        {
            var department = _departmentStorage.GetElement(new DepartmentBindingModel
            {
                Login = login,
            });
            if (department == null || !department.Password.Equals(password))
            {
                department = null;
            }
            return department;
        }
        public void CreateOrUpdate(DepartmentBindingModel model)
        {
            var element = _departmentStorage.GetElement(new DepartmentBindingModel
            {
                Login = model.Login
            });
            if (element != null && element.Id != model.Id)
            {
                throw new Exception("Уже есть такой элемент");
            }
            if (!Regex.IsMatch(model.Login, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
            {
                throw new Exception("В качестве логина должна быть указана почта");
            }
            if (model.Password.Length > _passwordMaxLength || model.Password.Length < _passwordMinLength ||
                !Regex.IsMatch(model.Password, @"^((\w+\d+\W+)|(\w+\W+\d+)|(\d+\w+\W+)|(\d+\W+\w+)|(\W+\w+\d+)|(\W+\d+\w+))[\w\d\W]*$"))
            {
                throw new Exception($"Пароль должен быть длиной от {_passwordMinLength} до " +
                    $"{_passwordMaxLength} и состоять из цифр, букв и небуквенных символов");
            }
            if (model.Id.HasValue)
            {
                _departmentStorage.Update(model);
            }
            else
            {
                _departmentStorage.Insert(model);
            }
        }
        public void Delete(DepartmentBindingModel model)
        {
            var element = _departmentStorage.GetElement(new DepartmentBindingModel
            {
                Id = model.Id
            });
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            _departmentStorage.Delete(model);
        }
    }
}
