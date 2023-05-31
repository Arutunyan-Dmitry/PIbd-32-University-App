using UniversityContracts.BindingModels;
using UniversityContracts.ViewModels;

namespace UniversityContracts.StorageContracts
{
    public interface IDepartmentStorage
    {
        List<DepartmentViewModel> GetFullList();
        DepartmentViewModel GetElement(DepartmentBindingModel model);
        void Insert(DepartmentBindingModel model);
        void Update(DepartmentBindingModel model);
        void Delete(DepartmentBindingModel model);
    }
}
