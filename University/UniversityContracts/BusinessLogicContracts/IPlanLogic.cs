using UniversityContracts.BindingModels;
using UniversityContracts.ViewModels;

namespace UniversityContracts.BusinessLogicContracts
{
    public interface IPlanLogic
    {
        List<PlanViewModel> Read(PlanBindingModel model);
        List<PlanViewModel> GetPlansByDiscipline(int id);
        void CreateOrUpdate(PlanBindingModel model);
        void Delete(PlanBindingModel model);
    }
}
