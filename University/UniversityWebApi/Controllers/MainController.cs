using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;
using UniversityBusinessLogic.MailWorker;
using UniversityContracts.BindingModels;
using UniversityContracts.BusinessLogicContracts;
using UniversityContracts.Enums;
using UniversityContracts.ViewModels;

namespace UniversityWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class MainController : ControllerBase
    {
        private readonly IDepartmentLogic _department;
        private readonly IDisciplineLogic _discipline;
        private readonly IGroupLogic _group;
        private readonly IPlanLogic _plan;
        private readonly IStudentLogic _student;
        private readonly ITeacherLogic _teacher;
        private readonly ITestingLogic _testing;
        private readonly IMessageLogic _message;
        private readonly IReportLogic _report;
        private readonly AbstractMailWorker _mailWorker;

        private readonly int objectsOnPage = 4;
        private int NumOfPages;
        public MainController(IDepartmentLogic department, IDisciplineLogic discipline,
                              IGroupLogic group, IPlanLogic plan,
                              IStudentLogic student, ITeacherLogic teacher,
                              ITestingLogic testing, IMessageLogic message,
                              IReportLogic report, AbstractMailWorker mailWorker)
        {
            _department = department;
            _discipline = discipline;
            _group = group;
            _plan = plan;
            _student = student;
            _teacher = teacher;
            _testing = testing;
            _message = message;
            _report = report;
            _mailWorker = mailWorker;

            if (objectsOnPage < 1) { objectsOnPage = 5; }
            _report = report;
        }

        #region COMMON METHODS
        //------------------------------Filtered Lists----------------------------
        [HttpGet]
        public List<DisciplineViewModel> GetDisciplines(int departmentId) => _discipline.Read(new DisciplineBindingModel
        { DepartmentId = departmentId });
        [HttpGet]
        public List<GroupViewModel> GetGroups(int departmentId) => _group.Read(new GroupBindingModel
        { DepartmentId = departmentId });
        [HttpGet]
        public List<StudentViewModel> GetStudents(int groupId) => _student.Read(new StudentBindingModel
        { GroupId = groupId });
        //------------------------------Filtered Lists----------------------------

        //------------------------------   Elements   ----------------------------
        [HttpGet]
        public DepartmentViewModel? GetDepartment(int id, string? login, string? password) => _department.Read(new DepartmentBindingModel
        { Id = id, Login = login, Password = password })?[0];
        [HttpGet]
        public TeacherViewModel? GetTeacher(int id, string? login, string? password) => _teacher.Read(new TeacherBindingModel
        { Id = id, Login = login, Password = password })?[0];
        [HttpGet]
        public DisciplineViewModel? GetDiscipline(int id) => _discipline.Read(new DisciplineBindingModel
        { Id = id })?[0];
        [HttpGet]
        public GroupViewModel? GetGroup(int id) => _group.Read(new GroupBindingModel
        { Id = id })?[0];
        [HttpGet]
        public StudentViewModel? GetStudent(int id) => _student.Read(new StudentBindingModel
        { Id = id })?[0];
        [HttpGet]
        public PlanViewModel? GetPlan(int id) => _plan.Read(new PlanBindingModel
        { Id = id })?[0];
        [HttpGet]
        public MessageViewModel? GetMessage(int id) => _message.Read(new MessageBindingModel
        { Id = id })?[0];
        //------------------------------   Elements   ----------------------------
        #endregion

        #region METHODS USED AS TEACHER CONTROLLER METHODS ONLY
        //------------------------------Filtered Lists----------------------------
        [HttpGet]
        public List<PlanViewModel> GetTeachertPlans(int teacherId, int disciplineId) => _plan.Read(new PlanBindingModel
        { TeacherId = teacherId, DisciplineId = disciplineId });
        [HttpGet]
        public List<PlanViewModel> GetFilteredPlans(int teacherId, int disciplineId, int groupId, PlanType type) => _plan.Read(new PlanBindingModel
        {
            TeacherId = teacherId,
            DisciplineId = disciplineId,
            GroupId = groupId,
            Type = type
        });
        [HttpGet]
        public List<Tuple<int, string, int, DateTime>> GetTestingsToday(int teacherId)
        {
            var plans = _plan.Read(new PlanBindingModel { TeacherId = teacherId });
            List<Tuple<int, string, int, DateTime>> result = new List<Tuple<int, string, int, DateTime>>();
            foreach (var plan in plans)
            {
                foreach (var testing in plan.Testings)
                {
                    if (testing.Item4.Date == DateTime.Now.Date)
                    {
                        result.Add(testing);
                    }
                }
            }
            return result;
        }
        [HttpGet]
        public Tuple<List<PlanViewModel>, int> GetPaginatedPlans(int teacherId, int page)
        {
            var fullList = _plan.Read(null);
            NumOfPages = fullList.Count / objectsOnPage;
            if (fullList.Count % objectsOnPage != 0) { NumOfPages++; }

            var list = _plan.Read(new PlanBindingModel { TeacherId = teacherId, ToSkip = (page - 1) * objectsOnPage, ToTake = objectsOnPage }).ToList();
            var result = new Tuple<List<PlanViewModel>, int>(list.Take(objectsOnPage).ToList(), NumOfPages);
            return result;
        }
        [HttpGet]
        public List<TestingViewModel> GetTestings(int planId) => _testing.Read(new TestingBindingModel
        { PlanId = planId });
        [HttpGet]
        public List<MessageViewModel> GetTeacherMessages(int id) => _message.Read(new MessageBindingModel
        { TeacherId = id });
        [HttpGet]
        public List<MessageViewModel> GetMessagesByStatus(int id, Status status) => _message.Read(new MessageBindingModel
        { TeacherId = id, Status = status });
        [HttpGet]
        public List<MessageViewModel> GetMessagesByStatusAndType(int id, ReportTypes typeReport, Status status) => _message.Read(new MessageBindingModel
        { TeacherId = id, Status = status, ReportType = typeReport });
        //------------------------------Filtered Lists----------------------------

        //------------------------------   Elements   ----------------------------
        [HttpGet]
        public TestingViewModel? GetTesting(int id) => _testing.Read(new TestingBindingModel
        { Id = id })?[0];
        [HttpGet]
        public TestingViewModel? GetTestingByPlanAndName(int planId, string topic) => _testing.Read(new TestingBindingModel
        { Topic = topic, PlanId = planId })?[0];
        //------------------------------   Elements   ----------------------------

        //------------------------------     C-U      ----------------------------
        [HttpPost]
        public void CreateOrUpdateTesting(TestingBindingModel model) => _testing.CreateOrUpdate(model);
        //------------------------------     C-U      ----------------------------

        //------------------------------     Put      ----------------------------
        [HttpPost]
        public void AnswerRequest(MessageBindingModel model) => _message.AnswerRequest(model);
        [HttpGet]
        public bool AddGroupToTesting(int testingId, int groupId)
        {
            var students = _student.Read(new StudentBindingModel { GroupId = groupId });
            foreach (var student in students)
            {
                _testing.AddStudent(new StudentTestingBindingModel
                {
                    TestingId = testingId,
                    StudentId = student.Id,
                    Grade = MarkType.Нет
                });
            }
            return true;
        }
        [HttpPost]
        public void PutStudentMarks(List<StudentTestingBindingModel> studentGrades)
        {
            foreach (var studentGrade in studentGrades)
            {
                _testing.AddStudent(studentGrade);
            }
        }
        //------------------------------     Put      ----------------------------

        //------------------------------      D       ----------------------------
        [HttpGet]
        public bool DeleteTesting(int id)
        {
            var testing = _testing.Read(new TestingBindingModel() { Id = id })?[0];
            _testing.CreateOrUpdate(new TestingBindingModel()
            {
                Id = id,
                PlanId = testing.PlanId,
                StudentTestings = new Dictionary<int, MarkType>()
            });
            _testing.Delete(new TestingBindingModel() { Id = id });
            return true;
        }
        //------------------------------      D       ----------------------------

        //------------------------------   Reports    ----------------------------
        [HttpGet]
        public ReportPlanViewModel GetObjectsForPlanReport(int id)
        {
            var tmp = _message.Read(new MessageBindingModel { Id = id })[0];
            MessageBindingModel model = new MessageBindingModel
            {
                Id = tmp.Id,
                ReportType = tmp.ReportType,
                Status = tmp.Status,
                DepartmentId = tmp.DepartmentId,
                PlanId = tmp.PlanId,
                TeacherId = tmp.TeacherId,
                DisciplineId = tmp.DisciplineId
            };
            return _report.GetObjectsForPlanReport(model);
        }
        [HttpGet]
        public ReportFullViewModel GetObjectsForSumReport(int id)
        {
            var tmp = _message.Read(new MessageBindingModel { Id = id })[0];
            MessageBindingModel model = new MessageBindingModel
            {
                Id = tmp.Id,
                ReportType = tmp.ReportType,
                Status = tmp.Status,
                DepartmentId = tmp.DepartmentId,
                PlanId = tmp.PlanId,
                TeacherId = tmp.TeacherId,
                DisciplineId = tmp.DisciplineId
            };
            return _report.GetObjectsForSumReport(model);
        }
        [HttpPost]
        public void SavePlanReport(Tuple<MessageBindingModel, string> model) => _report.SavePlanReport(model.Item1, model.Item2);
        [HttpPost]
        public void SaveSumReport(Tuple<MessageBindingModel, string> model) => _report.SaveSumReport(model.Item1, model.Item2);
        //------------------------------   Reports    ----------------------------

        //------------------------------     Mail     ----------------------------
        [HttpPost]
        public void SendMessage(MailSendInfoBindingModel model) => _mailWorker.MailSendAsync(model);
        //------------------------------     Mail     ----------------------------
        #endregion

        #region METHODS USED AS DEPARTMENT CONTROLLER METHODS ONLY
        //------------------------------Filtered Lists----------------------------
        [HttpGet]
        public List<DepartmentViewModel> GetAllDepartments() => _department.Read(null);
        [HttpGet]
        public List<TeacherViewModel> GetTeachers(int departmentId) => _teacher.Read(new TeacherBindingModel
        { DepartmentId = departmentId });
        [HttpGet]
        public List<PlanViewModel> GetDepartmentPlans(int departmentId) => _plan.Read(new PlanBindingModel
        { DepartmentId = departmentId });
        [HttpGet]
        public List<MessageViewModel> GetDepartmentMessages(int id) => _message.Read(new MessageBindingModel
        { DepartmentId = id });
        [HttpGet]
        public List<StudentViewModel> GetStudentsByDepartment(int departmentId) => _student.Read(new StudentBindingModel
        { DepartmentId = departmentId });
        [HttpGet]
        public List<PlanViewModel> GetPlansByDiscipline(int disciplineId)
        {
            return _plan.GetPlansByDiscipline(disciplineId);
        }
        [HttpGet]
        public List<TeacherViewModel> GetTeacherByDiscipline(int disciplineId)
        {
            return _teacher.GetTeacherByDiscipline(disciplineId);
        }
        //------------------------------Filtered Lists----------------------------

        //------------------------------    Element   ----------------------------
        [HttpGet]
        public DepartmentViewModel AuthorizationDepartment(string login, string password) => _department.Authorization(login, password);
        [HttpGet]
        public List<GroupViewModel> GetGroupsByName(string groupName, int departmentId) => _group.Read(new GroupBindingModel
        {
            DepartmentId = departmentId,
            Name = groupName
        });
        [HttpGet]
        public List<DisciplineViewModel> GetDisciplinesByName(string disciplineName, int departmentId) => _discipline.Read(new DisciplineBindingModel
        {
            DepartmentId = departmentId,
            Name = disciplineName
        });
        //------------------------------    Element   ----------------------------

        //------------------------------     C-U      ----------------------------
        [HttpPost]
        public void CreateOrUpdateDepartment(DepartmentBindingModel model) => _department.CreateOrUpdate(model);
        [HttpPost]
        public void CreateOrUpdateTeacher(TeacherBindingModel model) => _teacher.CreateOrUpdate(model);
        [HttpPost]
        public void CreateOrUpdateDiscipline(DisciplineBindingModel model) => _discipline.CreateOrUpdate(model);
        [HttpPost]
        public void CreateOrUpdateGroup(GroupBindingModel model) => _group.CreateOrUpdate(model);
        [HttpPost]
        public void CreateOrUpdateStudent(StudentBindingModel model) => _student.CreateOrUpdate(model);
        [HttpPost]
        public void CreateOrUpdatePlan(PlanBindingModel model) => _plan.CreateOrUpdate(model);
        [HttpPost]
        public void CreateMessage(MessageBindingModel model) => _message.CreateMessage(model);
        [HttpPost]
        public void SendGroupReport(Tuple<int> id) => _report.SendGroupReport(id.Item1);
        //------------------------------     C-U      ----------------------------

        //------------------------------     Put      ----------------------------       
        [HttpGet]
        public bool AddTeachersToDiscipline(int disciplineId, int teacherId)
        {

            _teacher.AddDiscipline(new TeacherDisciplineBindingModel
            {
                TeacherId = teacherId,
                DisciplineId = disciplineId
            });
            return true;
        }
        [HttpPut]
        public void RemoveTeacerFromDiscipline(TeacherDisciplineBindingModel model) => _teacher.RemoveDiscipline(model);
        [HttpPost]
        public void CloseRequest(MessageBindingModel model) => _message.CloseRequest(model);
        [HttpPost]
        public void MessageRollBack(MessageBindingModel model) => _message.MessageRollBack(model);
        //------------------------------     Put      ----------------------------

        //------------------------------      D       ----------------------------
        [HttpGet]
        public bool DeletePlan(int id)
        {
            _plan.Delete(new PlanBindingModel() { Id = id });
            return true;
        }
        [HttpGet]
        public bool DeleteStudent(int id)
        {
            _student.Delete(new StudentBindingModel
            {
                Id = id
            });
            return true;
        }
        [HttpGet]
        public bool DeleteTeacher(int id)
        {
            TeacherViewModel teacher = _teacher.Read(new TeacherBindingModel
            {
                Id = id
            })[0];
            TeacherBindingModel model = new TeacherBindingModel
            {
                Id = teacher.Id,
                DepartmentId = teacher.DepartmentId,
                Flm = teacher.Flm,
                Login = teacher.Login,
                Password = teacher.Password,
                TeacherDisciplines = teacher.TeacherDisciplines.Keys.ToList()
            };
            if (model.TeacherDisciplines.Count() > 0)
            {
                foreach (int teacherDiscipline in model.TeacherDisciplines)
                {
                    _teacher.RemoveDiscipline(new TeacherDisciplineBindingModel
                    {
                        TeacherId = model.Id.Value,
                        DisciplineId = teacherDiscipline
                    });
                }
            }
            List<PlanViewModel> listPlans = _plan.Read(new PlanBindingModel
            {
                DepartmentId = model.DepartmentId,
            });
            if (listPlans.Count() > 0)
            {
                foreach (PlanViewModel plan in listPlans)
                {
                    if (plan.TeacherId == model.Id)
                    {
                        _plan.Delete(new PlanBindingModel
                        {
                            Id = plan.Id
                        });
                    }
                }
            }
            _teacher.Delete(model);
            return true;
        }
        [HttpGet]
        public bool DeleteDiscipline(int id)
        {
            DisciplineViewModel discipline = _discipline.Read(new DisciplineBindingModel
            {
                Id = id
            })[0];
            DisciplineBindingModel model = new DisciplineBindingModel
            {
                Id = discipline.Id,
                DepartmentId = discipline.DepartmentId,
                Name = discipline.Name
            };
            List<PlanViewModel> listPlans = _plan.Read(new PlanBindingModel
            {
                DepartmentId = model.DepartmentId,
            });
            if (listPlans.Count() > 0)
            {
                foreach (PlanViewModel plan in listPlans)
                {
                    if (plan.DisciplineId == model.Id)
                    {
                        _plan.Delete(new PlanBindingModel
                        {
                            Id = plan.Id
                        });
                    }
                }
            }
            List<TeacherViewModel> teachersDiscipline = _teacher.GetTeacherByDiscipline(model.Id.Value);
            if (teachersDiscipline.Count() > 0)
            {
                foreach (TeacherViewModel teacherDiscipline in teachersDiscipline)
                {
                    _teacher.RemoveDiscipline(new TeacherDisciplineBindingModel
                    {
                        TeacherId = teacherDiscipline.Id,
                        DisciplineId = model.Id.Value
                    });
                }
            }
            _discipline.Delete(new DisciplineBindingModel
            {
                Id = id
            });
            return true;
        }
        [HttpGet]
        public bool DeleteGroup(int id)
        {
            GroupViewModel group = _group.Read(new GroupBindingModel
            {
                Id = id
            })[0];
            GroupBindingModel model = new GroupBindingModel
            {
                Id = group.Id,
                DepartmentId = group.DepartmentId,
                Name = group.Name,
                Course = group.Course
            };

            List<PlanViewModel> listPlans = _plan.Read(new PlanBindingModel
            {
                DepartmentId = model.DepartmentId,
            });
            if (listPlans.Count() > 0)
            {
                foreach (PlanViewModel plan in listPlans)
                {
                    if (plan.GroupId == model.Id)
                    {
                        _plan.Delete(new PlanBindingModel
                        {
                            Id = plan.Id
                        });
                    }
                }
            }
            _group.Delete(model);
            return true;
        }
        [HttpGet]
        public bool DeleteMessage(int id)
        {
            _message.Delete(new MessageBindingModel
            {
                Id = id
            });
            return true;
        }
        //------------------------------      D       ----------------------------
        #endregion

        }
    }
