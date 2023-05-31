using UniversityContracts.BindingModels;
using UniversityContracts.ViewModels;

namespace UniversityContracts.BusinessLogicContracts
{
    public interface IDepartmentLogic
    {
        List<DepartmentViewModel> Read(DepartmentBindingModel model);
        void CreateOrUpdate(DepartmentBindingModel model);
        void Delete(DepartmentBindingModel model);
        DepartmentViewModel Authorization(string login, string password);
    }
}
