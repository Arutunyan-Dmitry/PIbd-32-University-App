using UniversityContracts.BindingModels;
using UniversityContracts.ViewModels;

namespace UniversityContracts.StorageContracts
{
    public interface IStudentStorage
    {
        List<StudentViewModel> GetFullList();
        List<StudentViewModel> GetFilteredList(StudentBindingModel model);
        StudentViewModel GetElement(StudentBindingModel model);
        void Insert(StudentBindingModel model);
        void Update(StudentBindingModel model);
        void Delete(StudentBindingModel model);
    }
}
