using UniversityContracts.BindingModels;
using UniversityContracts.ViewModels;

namespace UniversityContracts.StorageContracts
{
    public interface IGroupStorage
    {
        List<GroupViewModel> GetFullList();
        List<GroupViewModel> GetFilteredList(GroupBindingModel model);
        GroupViewModel GetElement(GroupBindingModel model);
        void Insert(GroupBindingModel model);
        void Update(GroupBindingModel model);
        void Delete(GroupBindingModel model);
    }
}
