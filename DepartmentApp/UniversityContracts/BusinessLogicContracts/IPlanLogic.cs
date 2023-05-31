using UniversityContracts.BindingModels;
using UniversityContracts.ViewModels;

namespace UniversityContracts.BusinessLogicContracts
{
    public interface IPlanLogic
    {
        List<PlanViewModel> Read(PlanBindingModel model);
        void CreateOrUpdate(PlanBindingModel model);
        void Delete(PlanBindingModel model);
    }
}
