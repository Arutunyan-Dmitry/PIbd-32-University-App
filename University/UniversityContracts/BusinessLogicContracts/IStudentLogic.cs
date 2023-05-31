using UniversityContracts.BindingModels;
using UniversityContracts.ViewModels;

namespace UniversityContracts.BusinessLogicContracts
{
    public interface IStudentLogic
    {
        List<StudentViewModel> Read(StudentBindingModel model);
        void CreateOrUpdate(StudentBindingModel model);
        void Delete(StudentBindingModel model);
    }
}
