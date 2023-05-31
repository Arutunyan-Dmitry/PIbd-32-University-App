using UniversityContracts.BindingModels;
using UniversityContracts.ViewModels;

namespace UniversityContracts.BusinessLogicContracts
{
    public interface IDisciplineLogic
    {
        List<DisciplineViewModel> Read(DisciplineBindingModel model);
        void CreateOrUpdate(DisciplineBindingModel model);
        void Delete(DisciplineBindingModel model);
    }
}
