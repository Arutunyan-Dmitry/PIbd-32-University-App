using DocumentFormat.OpenXml.Office.CustomUI;
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
        public IDepartmentStorage _departmentStorage;
        private readonly AbstractSaveToWord _saveToWord;
        public ReportLogic(IPlanStorage planStorage, ITeacherStorage teacherStorage,
            ITestingStorage testingStorage, IDisciplineStorage disciplineStorage,
            IStudentStorage studentStorage, IGroupStorage groupStorage, IDepartmentStorage departmentStorage,
            AbstractSaveToWord abstractSaveToWord)
        {
            _planStorage = planStorage;
            _teacherStorage = teacherStorage;
            _testingStorage = testingStorage;
            _disciplineStorage = disciplineStorage;
            _studentStorage = studentStorage;
            _groupStorage = groupStorage;
            _departmentStorage = departmentStorage;
            _saveToWord = abstractSaveToWord;
        }

        public ReportFullViewModel GetObjectsForSumReport(MessageBindingModel model)
        {
            List<Tuple<string, List<Tuple<string, List<Tuple<string, List<Tuple<string, MarkType>>>>>>>> items = new List<Tuple<string, List<Tuple<string, List<Tuple<string, List<Tuple<string, MarkType>>>>>>>>();
            List<Tuple<List<string>>> Itog = new List<Tuple<List<string>>>();

            TeacherViewModel teacher = _teacherStorage.GetElement(new TeacherBindingModel
            {
                Id = model.TeacherId
            });
            DepartmentViewModel department = _departmentStorage.GetElement(new DepartmentBindingModel
            {
                Id = teacher.DepartmentId
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
                            (subitem.Item2.Count()+1) + " / " + student.NumFB + " / " + student.Flm, new List<Tuple<string, List<Tuple<string, MarkType>>>>()
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

            for (int i = 0; i < items.Count(); i++)
            {
                
                List<string> itog = new List<string> { "Итог:" };
                for (int j = 0; j < items[i].Item2[0].Item2.Count(); j++)
                {
                    for (int k = 0; k < items[i].Item2[0].Item2[j].Item2.Count(); k++)
                    {
                        if (items[i].Item2[0].Item2[j].Item2[k].Item2 == MarkType.П ||
                            items[i].Item2[0].Item2[j].Item2[k].Item2 == MarkType.НП ||
                            items[i].Item2[0].Item2[j].Item2[k].Item2 == MarkType.УП)
                        {
                            int upCount = 0;
                            int pCount = 0;
                            int npCount = 0;
                            foreach (var student in items[i].Item2)
                            {
                                if (student.Item2[j].Item2[k].Item2 == MarkType.УП)
                                    upCount++;
                                else if (student.Item2[j].Item2[k].Item2 == MarkType.П)
                                    pCount++;
                                else if (student.Item2[j].Item2[k].Item2 == MarkType.НП)
                                    npCount++;
                            }
                            itog.Add("УП - " + upCount +
                                     " П - " + pCount +
                                     " НП - " + npCount);
                        }
                        else
                        {
                            int Count0 = 0;
                            int Count2 = 0;
                            int Count3 = 0;
                            int Count4 = 0;
                            int Count5 = 0;
                            foreach (var student in items[i].Item2)
                            {
                                if (student.Item2[j].Item2[k].Item2 == MarkType.Нет)
                                    Count0++;
                                else if (student.Item2[j].Item2[k].Item2 == MarkType.Неуд)
                                    Count2++;
                                else if (student.Item2[j].Item2[k].Item2 == MarkType.Удовл)
                                    Count3++;
                                else if (student.Item2[j].Item2[k].Item2 == MarkType.Хор)
                                    Count4++;
                                else if (student.Item2[j].Item2[k].Item2 == MarkType.Отл)
                                    Count5++;
                            }
                            itog.Add("Нет - " + Count0 +
                                     " Неуд - " + Count2 +
                                     " Удовл - " + Count3 +
                                     " Хор - " + Count4 +
                                     " Отл - " + Count5);
                        }
                    }
                }
                Itog.Add(new Tuple<List<string>>(itog));
            }

            string groupList = "";
            foreach (var item in items)
            {
                if (items.IndexOf(item) != items.Count - 1)
                    groupList += item.Item1 + ", ";
                else
                    groupList += item.Item1;
            }

            List<string> footer = new List<string>
            {
                "Подпись декана _____________________________",
                "Подпись преподавателя _____________________________"
            };
            List<string> disciplineName = new List<string>
            {
                "University",
                "Кафедра: " + department.Name + "                  Дисциплина: " + discipline.Name,
                "Список групп: " + groupList,
                "Преподаватель: " + teacher.Flm,
                "Дата: " + DateTime.Now.Date.ToShortDateString()
            };
            return new ReportFullViewModel
            {
                Title = "Итоговый отчёт по всей дисциплине",
                Footer = footer,
                DisciplineName = disciplineName,
                Itog = Itog,
                Items = items
            };
        }

        public ReportPlanViewModel GetObjectsForPlanReport(MessageBindingModel model)
        {
            List<Tuple<string, string, List<Tuple<string, MarkType>>>> items = new List<Tuple<string, string, List<Tuple<string, MarkType>>>>();

            TeacherViewModel teacher = _teacherStorage.GetElement(new TeacherBindingModel
            {
                Id = model.TeacherId
            });
            DepartmentViewModel department = _departmentStorage.GetElement(new DepartmentBindingModel
            {
                Id = teacher.DepartmentId
            });
            PlanViewModel plan = _planStorage.GetElement(new PlanBindingModel
            {
                Id = model.PlanId
            });
            GroupViewModel group = _groupStorage.GetElement(new GroupBindingModel
            {
                Id = plan.GroupId
            });
            List<TestingViewModel> testings = _testingStorage.GetFilteredList(new TestingBindingModel
            {
                PlanId = (int)model.PlanId
            });
            var studentTestings = testings[0].StudentTestings;

            foreach (var item in studentTestings)
            {
                StudentViewModel student = _studentStorage.GetElement(new StudentBindingModel
                {
                    Id = item.Item1
                });
                items.Add(new Tuple<string, string, List<Tuple<string, MarkType>>>
                    (
                        student.NumFB, item.Item2, new List<Tuple<string, MarkType>>()
                    ));
            }

            for (int i = 0; i < testings.Count; i++)
            {
                foreach (var grades in testings[i].StudentTestings)
                {
                    items.Where(rec => rec.Item2 == grades.Item2).ToList()?[0]
                        .Item3.Add(new Tuple<string, MarkType>(testings[i].Topic, grades.Item3));
                }
            }
           
            List<string> itog = new List<string> { "", "", "Итог: " };

            for (int i = 0; i < items[0].Item3?.Count; i++)
            {
                if (items[0].Item3[i].Item2 == MarkType.П ||
                    items[0].Item3[i].Item2 == MarkType.НП ||
                    items[0].Item3[i].Item2 == MarkType.УП)
                {
                    int upCount = 0;
                    int pCount = 0;
                    int npCount = 0;
                    foreach (var item in items)
                    {
                        if (item.Item3[i].Item2 == MarkType.УП)
                            upCount++;
                        else if (item.Item3[i].Item2 == MarkType.П)
                            pCount++;
                        else if (item.Item3[i].Item2 == MarkType.НП)
                            npCount++;
                    }
                    itog.Add("УП - " + upCount + 
                             " П - " + pCount + 
                             " НП - " + npCount);
                } else
                {
                    int Count0 = 0;
                    int Count2 = 0;
                    int Count3 = 0;
                    int Count4 = 0;
                    int Count5 = 0;
                    foreach (var item in items)
                    {
                        if (item.Item3[i].Item2 == MarkType.Нет)
                            Count0++;
                        else if (item.Item3[i].Item2 == MarkType.Неуд)
                            Count2++;
                        else if (item.Item3[i].Item2 == MarkType.Удовл)
                            Count3++;
                        else if (item.Item3[i].Item2 == MarkType.Хор)
                            Count4++;
                        else if (item.Item3[i].Item2 == MarkType.Отл)
                            Count5++;
                    }
                    itog.Add("Нет - " + Count0 +
                             " Неуд - " + Count2 +
                             " Удовл - " + Count3 +
                             " Хор - " + Count4 +
                             " Отл - " + Count5);
                }
            }

            List<string> footer = new List<string> 
            {
                "Подпись декана _____________________________",
                "Подпись преподавателя _____________________________"
            };
            List<string> planName = new List<string>
            {
                "University",
                "Кафедра: " + department.Name + "                  Группа: " + group.Name,
                plan.Name,
                "Преподаватель: " + teacher.Flm,
                "Общее количество часов: " + plan.Hours + "             Дата: " + DateTime.Now.Date.ToShortDateString()
            };

            return new ReportPlanViewModel
            {
                Title = "Отчёт по плану",
                Footer = footer,
                PlanName = planName,
                Itog = itog,
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
