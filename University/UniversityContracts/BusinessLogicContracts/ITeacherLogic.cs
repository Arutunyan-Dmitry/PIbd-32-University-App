using UniversityContracts.BindingModels;
using UniversityContracts.ViewModels;

namespace UniversityContracts.BusinessLogicContracts
{
    public interface ITeacherLogic
    {
        List<TeacherViewModel> Read(TeacherBindingModel model);
        List<TeacherViewModel> GetTeacherByDiscipline(int id);
        void CreateOrUpdate(TeacherBindingModel model);
        void Delete(TeacherBindingModel model);
        void AddDiscipline(TeacherDisciplineBindingModel model);
        void RemoveDiscipline(TeacherDisciplineBindingModel model);
    }
}
