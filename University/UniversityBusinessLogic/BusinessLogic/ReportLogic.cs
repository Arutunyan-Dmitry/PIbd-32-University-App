using UniversityBusinessLogic.OfficePackage;
using UniversityContracts.BindingModels;
using UniversityContracts.BusinessLogicContracts;
using UniversityContracts.Enums;
using UniversityContracts.StorageContracts;
using UniversityContracts.ViewModels;

namespace UniversityBusinessLogic.BusinessLogic
{
    public class ReportLogic : IReportLogic
    {
        public IPlanStorage _planStorage;
        public ITestingStorage _testingStorage;
        public ITeacherStorage _teacherStorage;
        public IDisciplineStorage _disciplineStorage;
        public IStudentStorage _studentStorage;
        public IGroupStorage _groupStorage;
        private readonly AbstractSaveToWord _saveToWord;
        public ReportLogic(IPlanStorage planStorage, ITeacherStorage teacherStorage,
            ITestingStorage testingStorage, IDisciplineStorage disciplineStorage,
            IStudentStorage studentStorage, IGroupStorage groupStorage, 
            AbstractSaveToWord abstractSaveToWord)
        {
            _planStorage = planStorage;
            _teacherStorage = teacherStorage;
            _testingStorage = testingStorage;
            _disciplineStorage = disciplineStorage;
            _studentStorage = studentStorage;
            _groupStorage = groupStorage;
            _saveToWord = abstractSaveToWord;
        }

        public ReportFullViewModel GetObjectsForSumReport(MessageBindingModel model)
        {
            List<Tuple<string, List<Tuple<string, List<Tuple<string, List<Tuple<string, MarkType>>>>>>>> items = new List<Tuple<string, List<Tuple<string, List<Tuple<string, List<Tuple<string, MarkType>>>>>>>>();

            TeacherViewModel teacher = _teacherStorage.GetElement(new TeacherBindingModel
            {
                Id = model.TeacherId
            });
            DisciplineViewModel discipline = _disciplineStorage.GetElement(new DisciplineBindingModel
            {
                Id = model.DisciplineId
            });
            List<PlanViewModel> plans = _planStorage.GetFilteredList(new PlanBindingModel
            {
                TeacherId = (int)model.TeacherId,
                DisciplineId = (int)model.DisciplineId
            });

            foreach (var plan in plans)
            {
                bool flag = true;
                foreach (var item in items)
                {
                    if (item.Item1 == plan.GroupName)
                    {
                        flag = false;
                    }
                }

                if (flag)
                {
                    items.Add(new Tuple<string, List<Tuple<string, List<Tuple<string, List<Tuple<string, MarkType>>>>>>>
                        (plan.GroupName, new List<Tuple<string, List<Tuple<string, List<Tuple<string, MarkType>>>>>>()));
                }
            }

            foreach (var subitem in items)
            {
                GroupViewModel group = _groupStorage.GetElement(new GroupBindingModel
                {
                    DepartmentId = (int)model.DepartmentId,
                    Name = subitem.Item1
                });
                List<StudentViewModel> students = _studentStorage.GetFilteredList(new StudentBindingModel
                {
                    GroupId = group.Id
                });
                foreach (var student in students)
                {
                    subitem.Item2.Add(new Tuple<string, List<Tuple<string, List<Tuple<string, MarkType>>>>>
                        (
                            student.Flm, new List<Tuple<string, List<Tuple<string, MarkType>>>>()
                        ));

                    foreach (var subItem2 in subitem.Item2)
                    {
                        List<PlanViewModel> plns = _planStorage.GetFilteredList(new PlanBindingModel
                        {
                            TeacherId = (int)model.TeacherId,
                            GroupId = group.Id,
                            DisciplineId = (int)model.DisciplineId
                        });
                        foreach (var plan in plns)
                        {
                            if (subItem2.Item2.Where(rec => rec.Item1 == (plan.Type.ToString() + " план / " + plan.Hours + "ч.")).ToList().Count() == 0)
                            {
                                subItem2.Item2.Add(new Tuple<string, List<Tuple<string, MarkType>>>
                                (
                                    plan.Type.ToString() + " план / " + plan.Hours + "ч.", new List<Tuple<string, MarkType>>()
                                ));

                                foreach (var subItem3 in subItem2.Item2)
                                {
                                    if (subItem3.Item2.Count() == 0)
                                    {
                                        List<TestingViewModel> testings = _testingStorage.GetFilteredList(new TestingBindingModel
                                        {
                                            PlanId = plan.Id,
                                        });
                                        for (int i = 0; i < testings.Count; i++)
                                        {

                                            foreach (var grades in testings[i].StudentTestings)
                                            {
                                                if (student.Flm == grades.Item2)
                                                {
                                                    subItem3.Item2.Add(new Tuple<string, MarkType>(testings[i].Topic + " / " + testings[i].Hours + "ч.", grades.Item3));
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return new ReportFullViewModel
            {
                Title = "Итоговый отчёт по всей дисциплине",
                Footer = "Выполнил " + teacher.Flm,
                DisciplineName = discipline.Name,
                Items = items
            };
        }

        public ReportPlanViewModel GetObjectsForPlanReport(MessageBindingModel model)
        {
            List<Tuple<string, List<Tuple<string, MarkType>>>> items = new List<Tuple<string, List<Tuple<string, MarkType>>>>();

            TeacherViewModel teacher = _teacherStorage.GetElement(new TeacherBindingModel
            {
                Id = model.TeacherId
            });
            PlanViewModel plan = _planStorage.GetElement(new PlanBindingModel
            {
                Id = model.PlanId
            });
            List<TestingViewModel> testings = _testingStorage.GetFilteredList(new TestingBindingModel
            {
                PlanId = (int)model.PlanId
            });
            var studentTestings = testings[0].StudentTestings;

            foreach (var item in studentTestings)
            {
                items.Add(new Tuple<string, List<Tuple<string, MarkType>>>
                    (
                        item.Item2, new List<Tuple<string, MarkType>>()
                    ));
            }

            for (int i = 0; i < testings.Count; i++)
            {
                foreach (var grades in testings[i].StudentTestings)
                {
                    items.Where(rec => rec.Item1 == grades.Item2).ToList()?[0]
                        .Item2.Add(new Tuple<string, MarkType>(testings[i].Topic, grades.Item3));
                }
            }

            return new ReportPlanViewModel
            {
                Title = "Отчёт по плану",
                Footer = "Выполнил " + teacher.Flm,
                PlanName = plan.Name,
                Items = items
            };
        }

        public void SaveSumReport(MessageBindingModel model, string filename)
        {
            var result = GetObjectsForSumReport(model);
            _saveToWord.CreateFullDoc(result, filename);
        }

        public void SavePlanReport(MessageBindingModel model, string filename)
        {
            var result = GetObjectsForPlanReport(model);
            _saveToWord.CreatePlanDoc(result, filename);
        }
    }
}
