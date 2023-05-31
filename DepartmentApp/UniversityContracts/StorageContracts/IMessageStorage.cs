using UniversityContracts.BindingModels;
using UniversityContracts.ViewModels;

namespace UniversityContracts.StorageContracts
{
    public interface IMessageStorage
    {
        List<MessageViewModel> GetFullList();
        List<MessageViewModel> GetFilteredList(MessageBindingModel model);
        MessageViewModel GetElement(MessageBindingModel model);
        void Insert(MessageBindingModel model);
        void Update(MessageBindingModel model);
        void Delete(MessageBindingModel model);
    }
}
