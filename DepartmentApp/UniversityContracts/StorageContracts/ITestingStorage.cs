using UniversityContracts.BindingModels;
using UniversityContracts.ViewModels;

namespace UniversityContracts.StorageContracts
{
    public interface ITestingStorage
    {
        List<TestingViewModel> GetFullList();
        List<TestingViewModel> GetFilteredList(TestingBindingModel model);
        TestingViewModel GetElement(TestingBindingModel model);
        void Insert(TestingBindingModel model);
        void Update(TestingBindingModel model);
        void Delete(TestingBindingModel model);
    }
}
