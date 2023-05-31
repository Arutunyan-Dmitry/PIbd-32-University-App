using UniversityContracts.BindingModels;
using UniversityContracts.BusinessLogicContracts;
using UniversityContracts.Enums;
using UniversityContracts.StorageContracts;
using UniversityContracts.ViewModels;

namespace UniversityBusinessLogic.BusinessLogic
{
    public class MessageLogic : IMessageLogic
    {
        private readonly IMessageStorage _messageStorage;
        public MessageLogic(IMessageStorage messageStorage)
        {
            _messageStorage = messageStorage;
        }
        public List<MessageViewModel> Read(MessageBindingModel model)
        {
            if (model == null)
            {
                return _messageStorage.GetFullList();
            }
            if (model.Id.HasValue)
            {
                return new List<MessageViewModel> { _messageStorage.GetElement(model) };
            }
            return _messageStorage.GetFilteredList(model);
        }
        public void CreateMessage(MessageBindingModel model)
        {
            model.Status = Status.Активен;
            _messageStorage.Insert(model);
        }
        public void AnswerRequest(MessageBindingModel model)
        {
            var message = _messageStorage.GetElement(new MessageBindingModel { Id = model.Id });
            if (message == null)
            {
                throw new Exception("Сообщение не найдено");
            }
            message.Status = Status.Проверяется;
            _messageStorage.Update(new MessageBindingModel
            {
                Id = message.Id,
                DepartmentId = message.DepartmentId,
                TeacherId = message.TeacherId,
                PlanId = message.PlanId,
                DisciplineId = message.DisciplineId,
                ReportType = message.ReportType,
                Status = message.Status
            });
        }
        public void CloseRequest(MessageBindingModel model)
        {
            var message = _messageStorage.GetElement(new MessageBindingModel { Id = model.Id });
            if (message == null)
            {
                throw new Exception("Сообщение не найдено");
            }
            message.Status = Status.Проверен;
            _messageStorage.Update(new MessageBindingModel
            {
                Id = message.Id,
                DepartmentId = message.DepartmentId,
                TeacherId = message.TeacherId,
                PlanId = message.PlanId,
                DisciplineId = message.DisciplineId,
                ReportType = message.ReportType,
                Status = message.Status
            });
        }
        public void Delete(MessageBindingModel model)
        {
            var message = _messageStorage.GetElement(new MessageBindingModel { Id = model.Id });
            if (message == null)
            {
                throw new Exception("Сообщение не найдено");
            }
            if (message.Status != Status.Активен)
            {
                throw new Exception("По данному запросу уже выполнен отчёт");
            }
            _messageStorage.Delete(new MessageBindingModel { Id = model.Id });
        }
    }
}
