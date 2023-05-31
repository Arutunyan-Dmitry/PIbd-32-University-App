using Microsoft.EntityFrameworkCore;
using UniversityContracts.BindingModels;
using UniversityContracts.Enums;
using UniversityContracts.StorageContracts;
using UniversityContracts.ViewModels;
using UniversityDatabaseImplement.Models;

namespace UniversityDatabaseImplement.Implements
{
    public class TestingStorage : ITestingStorage
    {
        public List<TestingViewModel> GetFullList()
        {
            using var context = new UniversityDatabase();
            return context.Testings
            .Include(rec => rec.StudentTestings)
            .ThenInclude(rec => rec.Student)
            .ToList()
            .Select(CreateModel)
            .ToList();
        }
        public List<TestingViewModel> GetFilteredList(TestingBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using var context = new UniversityDatabase();
            return context.Testings
            .Include(rec => rec.StudentTestings)
            .ThenInclude(rec => rec.Student)
            .Where(rec => rec.PlanId == model.PlanId)
            .ToList()
            .Select(CreateModel)
            .ToList();
        }
        public TestingViewModel GetElement(TestingBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using var context = new UniversityDatabase();
            var testing = context.Testings
            .Include(rec => rec.StudentTestings)
            .ThenInclude(rec => rec.Student)
            .FirstOrDefault(rec => rec.Id == model.Id || (rec.Topic == model.Topic && rec.PlanId == model.PlanId));
            return testing != null ? CreateModel(testing) : null;
        }
        public void Insert(TestingBindingModel model)
        {
            using var context = new UniversityDatabase();
            using var transaction = context.Database.BeginTransaction();
            try
            {
                Testing testing = new Testing()
                {
                    PlanId = model.PlanId,
                    Topic = model.Topic,
                    Type = (PlanType)model.Type,
                    Hours = model.Hours,
                    Date = model.Date
                };
                context.Testings.Add(testing);
                context.SaveChanges();
                CreateModel(model, testing, context);
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
        public void Update(TestingBindingModel model)
        {
            using var context = new UniversityDatabase();
            using var transaction = context.Database.BeginTransaction();
            try
            {
                var element = context.Testings.FirstOrDefault(rec => rec.Id == model.Id);
                if (element == null)
                {
                    throw new Exception("Элемент не найден");
                }
                CreateModel(model, element, context);
                context.SaveChanges();
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
        public void Delete(TestingBindingModel model)
        {
            using var context = new UniversityDatabase();
            Testing? element = context.Testings.FirstOrDefault(rec => rec.Id == model.Id);
            if (element != null)
            {
                context.Testings.Remove(element);
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
        private static Testing CreateModel(TestingBindingModel model, Testing testing, UniversityDatabase context)
        {
            if (model.Topic != null && model.Type != null &&
                model.Hours != null && model.Date != null)
            {
                testing.Topic = model.Topic;
                testing.Hours = model.Hours;
                testing.Type = (PlanType)model.Type;
                testing.Date = model.Date;
            }
            if (model.StudentTestings == null)
            {
                return testing;
            }
            if (model.Id.HasValue)
            {
                var studentTestings = context.StudentTestings.Where(rec => rec.TestingId == model.Id.Value).ToList();
                context.StudentTestings.RemoveRange(studentTestings.Where(rec =>
               !model.StudentTestings.ContainsKey(rec.StudentId)).ToList());
                context.SaveChanges();
                foreach (var updateTesting in studentTestings)
                {
                    try
                    {
                        updateTesting.Grade = model.StudentTestings[updateTesting.StudentId];
                    }
                    catch { }
                    model.StudentTestings.Remove(updateTesting.StudentId);
                }
                context.SaveChanges();
            }
            foreach (var st in model.StudentTestings)
            {
                context.StudentTestings.Add(new StudentTesting
                {
                    TestingId = testing.Id,
                    StudentId = st.Key,
                    Grade = st.Value
                });
                context.SaveChanges();
            }
            return testing;
        }
        private static TestingViewModel CreateModel(Testing testing)
        {
            TestingViewModel model = new TestingViewModel();
            model.Id = testing.Id;
            model.PlanId = testing.PlanId;
            model.Topic = testing.Topic;
            model.Type = testing.Type;
            model.Hours = testing.Hours;
            model.Date = testing.Date;
            model.StudentTestings = new List<Tuple<int, string, MarkType>>();

            foreach(var item in testing.StudentTestings)
            {
                model.StudentTestings.Add(new Tuple<int, string, MarkType>(item.StudentId, item.Student.Flm, item.Grade));
            }

            return model;
        }
    }
}
