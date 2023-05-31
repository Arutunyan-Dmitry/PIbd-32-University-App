using UniversityContracts.BindingModels;
using UniversityContracts.ViewModels;

namespace UniversityContracts.BusinessLogicContracts
{
    public interface IGroupLogic
    {
        List<GroupViewModel> Read(GroupBindingModel model);
        void CreateOrUpdate(GroupBindingModel model);
        void Delete(GroupBindingModel model);
    }
}
