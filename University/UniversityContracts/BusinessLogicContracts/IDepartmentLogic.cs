using UniversityContracts.BindingModels;
using UniversityContracts.ViewModels;

namespace UniversityContracts.BusinessLogicContracts
{
    public interface IDepartmentLogic
    {
        List<DepartmentViewModel> Read(DepartmentBindingModel model);
        DepartmentViewModel? Authorization(string login, string password);
        void CreateOrUpdate(DepartmentBindingModel model);
        void Delete(DepartmentBindingModel model);
    }
}
