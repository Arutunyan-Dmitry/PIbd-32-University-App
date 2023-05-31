using UniversityContracts.BindingModels;
using UniversityContracts.ViewModels;

namespace UniversityContracts.BusinessLogicContracts
{
    public interface ITestingLogic
    {
        List<TestingViewModel> Read(TestingBindingModel model);
        void CreateOrUpdate(TestingBindingModel model);
        void Delete(TestingBindingModel model);
        void AddStudent(StudentTestingBindingModel model);
        void RemoveStudent(StudentTestingBindingModel model);
    }
}
