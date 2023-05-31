using UniversityContracts.BindingModels;
using UniversityContracts.Enums;
using UniversityContracts.StorageContracts;
using UniversityContracts.ViewModels;
using UniversityDatabaseImplement.Models;

namespace UniversityDatabaseImplement.Implements
{
    public class MessageStorage : IMessageStorage
    {
        public List<MessageViewModel> GetFullList()
        {
            using var context = new UniversityDatabase();
            return context.Messages
            .Select(CreateModel)
            .ToList();
        }
        public List<MessageViewModel> GetFilteredList(MessageBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using var context = new UniversityDatabase();
            return context.Messages
            .Where(rec => (rec.DepartmentId == model.DepartmentId) ||
                           (rec.TeacherId == model.TeacherId && model.Status == null) ||
                           (rec.TeacherId == model.TeacherId && rec.Status == model.Status))
            .Select(CreateModel)
            .ToList();
        }
        public MessageViewModel GetElement(MessageBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using var context = new UniversityDatabase();
            var message = context.Messages
            .FirstOrDefault(rec => rec.Id == model.Id);
            return message != null ? CreateModel(message) : null;
        }
        public void Insert(MessageBindingModel model)
        {
            using var context = new UniversityDatabase();
            context.Messages.Add(CreateModel(model, new Message(), context));
            context.SaveChanges();
        }
        public void Update(MessageBindingModel model)
        {
            using var context = new UniversityDatabase();
            var element = context.Messages.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            CreateModel(model, element, context);
            context.SaveChanges();
        }
        public void Delete(MessageBindingModel model)
        {
            using var context = new UniversityDatabase();
            Message? element = context.Messages.FirstOrDefault(rec => rec.Id == model.Id);
            if (element != null)
            {
                context.Messages.Remove(element);
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
        private static Message CreateModel(MessageBindingModel model, Message message, UniversityDatabase context)
        {
            if (model.Status == Status.Активен)
            {
                if (model.PlanId != null)
                {
                    message.PlanId = (int)model.PlanId;
                    var lesson = context.Plans.FirstOrDefault(rec => rec.Id == model.PlanId);
                    message.TeacherId = lesson.TeacherId;
                    message.ReportType = ReportTypes.PlanReport;
                }
                if (model.TeacherId != null && model.DisciplineId != null)
                {
                    message.TeacherId = (int)model.TeacherId;
                    message.DisciplineId = (int)model.DisciplineId;
                    message.ReportType = ReportTypes.SumReport;
                }
            } 
            message.DepartmentId = model.DepartmentId;
            message.Status = (Status)model.Status;
            return message;
        }
        private static MessageViewModel CreateModel(Message message)
        {
            using var context = new UniversityDatabase();
            Discipline? discipline = context.Disciplines.FirstOrDefault(rec => rec.Id == message.DisciplineId);
            Plan? plan = context.Plans.FirstOrDefault(rec => rec.Id == message.PlanId);

            MessageViewModel model = new MessageViewModel 
            {
                Id = message.Id,
                DepartmentId = message.DepartmentId,
                TeacherId = message.TeacherId,
                PlanId = message.PlanId,
                DisciplineId = message.DisciplineId,
                ReportType = message.ReportType,
                Status = message.Status
            };

            if (discipline != null)
            {
                model.DisciplineName = discipline.Name;
            }

            if (plan != null)
            {
                model.PlanName = plan.Name;
            }

            return model;
        }
    }
}
