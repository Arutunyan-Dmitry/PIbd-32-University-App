using UniversityContracts.BindingModels;
using UniversityContracts.ViewModels;

namespace UniversityContracts.BusinessLogicContracts
{
    public interface IMessageLogic
    {
        List<MessageViewModel> Read(MessageBindingModel model);
        void CreateMessage(MessageBindingModel model);
        void AnswerRequest(MessageBindingModel model);
        void CloseRequest(MessageBindingModel model);
        void Delete(MessageBindingModel model);
    }
}
