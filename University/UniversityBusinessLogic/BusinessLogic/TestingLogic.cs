using UniversityContracts.BindingModels;
using UniversityContracts.BusinessLogicContracts;
using UniversityContracts.Enums;
using UniversityContracts.StorageContracts;
using UniversityContracts.ViewModels;

namespace UniversityBusinessLogic.BusinessLogic
{
    public class TestingLogic : ITestingLogic
    {
        private readonly ITestingStorage _testingStorage;
        private readonly IStudentStorage _studentStorage;
        private readonly IPlanStorage _planStorage;
        public TestingLogic(ITestingStorage testingStorage, IStudentStorage studentStorage, 
            IPlanStorage planStorage)
        {
            _testingStorage = testingStorage;
            _studentStorage = studentStorage;
            _planStorage = planStorage;
        }
        public List<TestingViewModel> Read(TestingBindingModel model)
        {
            if (model == null)
            {
                return _testingStorage.GetFullList();
            }
            if (model.Id.HasValue || model.Topic != null)
            {
                return new List<TestingViewModel> { _testingStorage.GetElement(model) };
            }
            return _testingStorage.GetFilteredList(model);
        }
        public void CreateOrUpdate(TestingBindingModel model)
        {
            var element = _testingStorage.GetElement(new TestingBindingModel
            {
                PlanId = model.PlanId,
                Topic = model.Topic,
                Type = model.Type,
                Hours = model.Hours,
                Date = model.Date
            });
            var plan = _planStorage.GetElement(new PlanBindingModel
            {
                Id = model.PlanId
            });
            model.Type = plan.Type;
            if ((element != null && element.Id != model.Id) ||
                (element != null && element.Topic != model.Topic && element.PlanId == model.PlanId))
            {
                throw new Exception("Уже есть такой элемент");
            }
            if (model.Id.HasValue)
            {
                _testingStorage.Update(model);
            }
            else
            {
                _testingStorage.Insert(model);
            }
        }
        public void Delete(TestingBindingModel model)
        {
            var element = _testingStorage.GetElement(new TestingBindingModel
            {
                Id = model.Id
            });
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            _testingStorage.Delete(model);
        }
        public void AddStudent(StudentTestingBindingModel model)
        {
            var testing = _testingStorage.GetElement(new TestingBindingModel
            {
                Id = model.TestingId
            });

            var student = _studentStorage.GetElement(new StudentBindingModel
            {
                Id = model.StudentId
            });

            if (testing == null || student == null)
            {
                throw new Exception("Элемент не найден");
            }

            Dictionary<int, MarkType> studentTestings = new Dictionary<int, MarkType>();
            foreach (var item in testing.StudentTestings)
                studentTestings.Add(item.Item1, item.Item3);

            if (studentTestings.ContainsKey(model.StudentId))
            {
                studentTestings[model.StudentId] = model.Grade;
            }
            else
            {
                studentTestings.Add(model.StudentId, model.Grade);
            }

            _testingStorage.Update(new TestingBindingModel
            {
                Id = testing.Id,
                Topic = testing.Topic,
                Hours = testing.Hours,
                Date = testing.Date,
                Type = testing.Type,
                StudentTestings = studentTestings
            });
        }
        public void RemoveStudent(StudentTestingBindingModel model)
        {
            var testing = _testingStorage.GetElement(new TestingBindingModel
            {
                Id = model.TestingId
            });

            var student = _studentStorage.GetElement(new StudentBindingModel
            {
                Id = model.StudentId
            });

            if (testing == null || student == null)
            {
                throw new Exception("Элемент не найден");
            }

            Dictionary<int, MarkType> studentTestings = new Dictionary<int, MarkType>();
            foreach (var item in testing.StudentTestings)
                studentTestings.Add(item.Item1, item.Item3);
            
            if (studentTestings.ContainsKey(model.StudentId))
            {
                studentTestings.Remove(model.StudentId);
            }
            else
            {
                throw new Exception("Запись не найдена");
            }
            _testingStorage.Update(new TestingBindingModel
            {
                Id = testing.Id,
                Topic = testing.Topic,
                Hours = testing.Hours,
                Date = testing.Date,
                Type = testing.Type,
                StudentTestings = studentTestings
            });
        }
    }
}
