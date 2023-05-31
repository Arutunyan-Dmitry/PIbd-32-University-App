using UniversityContracts.BindingModels;
using UniversityContracts.ViewModels;

namespace UniversityContracts.StorageContracts
{
    public interface ITeacherStorage
    {
        List<TeacherViewModel> GetFullList();
        List<TeacherViewModel> GetFilteredList(TeacherBindingModel model);
        TeacherViewModel GetElement(TeacherBindingModel model);
        void Insert(TeacherBindingModel model);
        void Update(TeacherBindingModel model);
        void Delete(TeacherBindingModel model);
    }
}
