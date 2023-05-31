using UniversityContracts.BindingModels;
using UniversityContracts.ViewModels;

namespace UniversityContracts.StorageContracts
{
    public interface IPlanStorage
    {
        List<PlanViewModel> GetFullList();
        List<PlanViewModel> GetFilteredList(PlanBindingModel model);
        List<PlanViewModel> GetPlansByDiscipline(int id);
        PlanViewModel GetElement(PlanBindingModel model);
        void Insert(PlanBindingModel model);
        void Update(PlanBindingModel model);
        void Delete(PlanBindingModel model);
    }
}
