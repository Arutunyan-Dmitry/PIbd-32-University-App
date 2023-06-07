using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Text;
using System.Text.RegularExpressions;
using UniversityContracts.BindingModels;
using UniversityContracts.Enums;
using UniversityContracts.ViewModels;

namespace TeacherApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly string rootPath = "D:\\ULSTU\\ПиАПС\\Курсовая\\PROJECT\\TeacherApp";
        private readonly ILogger<HomeController> _logger;
        protected readonly IWebHostEnvironment _hostEnvironment;
        protected static string error = "";
        protected static string unxpError = "";
        protected static string success = "";
        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _hostEnvironment = webHostEnvironment;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        //------------------------------ SignUp -------------------------------
        public IActionResult SignUp()
        {
            if (error != "")
            {
                ViewData["Error"] = error;
                error = "";
            }
            if (unxpError != "")
            {
                ViewData["UnxpError"] = unxpError;
                unxpError = "";
            }
            return View();
        }

        [HttpPost]
        public void TeacherSignUp(string Login, string Password)
        {
            try
            {
                if (!string.IsNullOrEmpty(Login) && !string.IsNullOrEmpty(Password))
                {
                    if (Login.Any(ch => ! char.IsLetterOrDigit(ch)))
                    {
                        error = "В логине не может содержаться спецсимволов";
                        Response.Redirect("SignUp");
                        return;
                    }
                    if (!Regex.IsMatch(Password, @"^((\w+\d+\W+)|(\w+\W+\d+)|(\d+\w+\W+)|(\d+\W+\w+)|(\W+\w+\d+)|(\W+\d+\w+))[\w\d\W]*$"))
                    {
                        error = "Пароль должен содержать в себе цифры, буквы и небуквенные символы";
                        Response.Redirect("SignUp");
                        return;
                    }
                    APIClient.teacher = APIClient.GetRequest<TeacherViewModel>($"api/Main/GetTeacher?login={Login}&password={Password}");
                    Response.Redirect("Index");
                    return;
                } 
                else
                {
                    Response.Redirect("SignUp");
                    return;
                } 
            }
            catch (Exception ex)
            {
                unxpError = GetCyrillic(ex.Message);
                Response.Redirect("SignUp");
                return;
            }
        }

        public IActionResult Exit()
        {
            APIClient.teacher = null;
            return Redirect("SignUp");
        }
        //------------------------------ SignUp -------------------------------

        //----------------------------- HomePage ------------------------------
        [HttpGet]
        public IActionResult Index()
        {
            if (unxpError != "")
            {
                ViewData["UnxpError"] = unxpError;
                unxpError = "";
            }
            if (APIClient.teacher == null)
            {
                error = "Похожке, что вы вышли из системы";
                return Redirect("SignUp");
            }
            try
            {
                DepartmentViewModel d = APIClient.GetRequest<DepartmentViewModel>
                    ($"api/Main/GetDepartment?id={APIClient.teacher.DepartmentId}&login={null}&password={null}");

                var messages = APIClient.GetRequest<List<MessageViewModel>>(
                    $"api/Main/GetMessagesByStatus?id={APIClient.teacher.Id}&status={Status.Активен}");

                var testings = APIClient.GetRequest<List<Tuple<int, string, int, DateTime>>>
                            ($"api/Main/GetTestingsToday?teacherId={APIClient.teacher.Id}");

                var model = (d.Name, APIClient.teacher.Flm, messages, testings);
                return View(model);
            } catch (Exception ex)
            {
                unxpError = GetCyrillic(ex.Message);
                return View();
            }
        }
        //----------------------------- HomePage ------------------------------

        //------------------------------ EdPlan -------------------------------
        [HttpGet]
        public IActionResult Plan(int? disciplineId)
        {
            if (unxpError != "")
            {
                ViewData["UnxpError"] = unxpError;
                unxpError = "";
            }
            if (APIClient.teacher == null)
            {
                error = "Похожке, что вы вышли из системы";
                return Redirect("SignUp");
            }
            try
            {
                List<PlanViewModel> plans = new List<PlanViewModel>();
                if (disciplineId == null)
                {
                    if (APIClient.teacher.TeacherDisciplines.Count() != 0)
                    {
                        plans = APIClient.GetRequest<List<PlanViewModel>>
                            ($"api/Main/GetTeachertPlans?teacherId={APIClient.teacher.Id}&disciplineId={APIClient.teacher.TeacherDisciplines.Keys.ToList()[0]}");
                    }
                }
                else
                {
                    plans = APIClient.GetRequest<List<PlanViewModel>>
                        ($"api/Main/GetTeachertPlans?teacherId={APIClient.teacher.Id}&disciplineId={disciplineId}");
                }
                var model = (plans, APIClient.teacher.TeacherDisciplines);
                return View(model);
            } catch (Exception ex)
            {
                unxpError = GetCyrillic(ex.Message);
                return View();
            }
        }

        [HttpGet]
        public IActionResult Testing(int groupId, int planId, int? testingId)
        {
            if (error != "")
            {
                ViewData["Error"] = error;
                error = "";
            }
            if (unxpError != "")
            {
                ViewData["UnxpError"] = unxpError;
                unxpError = "";
            }
            if (APIClient.teacher == null)
            {
                error = "Похожке, что вы вышли из системы";
                return Redirect("SignUp");
            }
            try
            {
                TestingViewModel testing = new TestingViewModel();
                if (testingId != null)
                {
                    testing = APIClient.GetRequest<TestingViewModel>(
                        $"api/Main/GetTesting?id={testingId}");
                }
                var model = (groupId, planId, testing);
                return View(model);
            } catch(Exception ex)
            {
                unxpError = GetCyrillic(ex.Message);
                return View();
            }
        }

        [HttpPost]
        public void UpdateTesting(int? Id, int GroupId, int PlanId, string Topic, [Bind("PlanId, Topic, Hours, Date")] TestingBindingModel model)
        {
            if (APIClient.teacher == null)
            {
                error = "Похожке, что вы вышли из системы";
                Response.Redirect("SignUp");
                return;
            }
            try
            {
                if (string.IsNullOrEmpty(model.Topic))
                {
                    error = "Название темы не может быть пустым";
                    if (Id == null)
                    {
                        Response.Redirect($"Testing?groupId={GroupId}&planId={PlanId}");
                    } else
                    {
                        Response.Redirect($"Testing?groupId={GroupId}&planId={PlanId}&testingId={Id}");
                    }               
                    return;
                }
                if (model.Hours == 0)
                {
                    error = "Укажите корректную продолжительность занятия";
                    if (Id == null)
                    {
                        Response.Redirect($"Testing?groupId={GroupId}&planId={PlanId}");
                    }
                    else
                    {
                        Response.Redirect($"Testing?groupId={GroupId}&planId={PlanId}&testingId={Id}");
                    }
                    return;
                }
                PlanViewModel plan = APIClient.GetRequest<PlanViewModel>(
                        $"api/Main/GetPlan?id={PlanId}");
                int sum = 0;
                foreach (var item in plan.Testings)
                {
                    if(Id == null)
                    {
                        sum += item.Item3;
                    } else
                    {
                        if(item.Item1 != Id)
                        {
                            sum += item.Item3;
                        }
                    }
                }
                if ((model.Hours + sum) <= plan.Hours)
                {
                    if (Id == null)
                    {
                        APIClient.PostRequest($"api/Main/CreateOrUpdateTesting", model);
                        var testing = APIClient.GetRequest<TestingViewModel>(
                            $"api/Main/GetTestingByPlanAndName?planId={PlanId}&topic={Topic}");
                        APIClient.GetRequest<bool>(
                            $"api/Main/AddGroupToTesting?testingId={testing.Id}&groupId={GroupId}");
                    }
                    else
                    {
                        model.Id = Id;
                        APIClient.PostRequest($"api/Main/CreateOrUpdateTesting", model);
                    }
                } else
                {
                    error = "Продолжительность занятия не может превышать продолжительность курса";
                    if (Id == null)
                    {
                        Response.Redirect($"Testing?groupId={GroupId}&planId={PlanId}");
                    }
                    else
                    {
                        Response.Redirect($"Testing?groupId={GroupId}&planId={PlanId}&testingId={Id}");
                    }
                    return;
                }
                Response.Redirect("Plan");
            } catch (Exception ex)
            {
                unxpError = GetCyrillic(ex.Message);
                if (Id == null)
                {
                    Response.Redirect($"Testing?groupId={GroupId}&planId={PlanId}");
                }
                else
                {
                    Response.Redirect($"Testing?groupId={GroupId}&planId={PlanId}&testingId={Id}");
                }
                return;
            }
        }
        
        [HttpGet]
        public void DeleteTesting(int id, int planId)
        {
            if (APIClient.teacher == null)
            {
                error = "Похожке, что вы вышли из системы";
                Response.Redirect("SignUp");
                return;
            }
            try
            {
                APIClient.GetRequest<bool>($"api/Main/DeleteTesting?id={id}");
                Response.Clear();
                Response.Redirect($"/Home/Plan");
            } catch (Exception ex)
            {
                unxpError = GetCyrillic(ex.Message);
                Response.Redirect("Plan");
                return;
            }
        }
        //------------------------------ EdPlan -------------------------------

        //---------------------------- Plan&Marks -----------------------------
        [HttpGet]
        public IActionResult Plans(string? serializedList, int page = 1)
        {
            if (unxpError != "")
            {
                ViewData["UnxpError"] = unxpError;
                unxpError = "";
            }
            if (APIClient.teacher == null)
            {
                error = "Похожке, что вы вышли из системы";
                return Redirect("SignUp");
            }
            try
            {
                ViewBag.Disciplines = APIClient.GetRequest<List<DisciplineViewModel>>($"api/Main/GetDisciplines?departmentId={APIClient.teacher.DepartmentId}");
                ViewBag.Groups = APIClient.GetRequest<List<GroupViewModel>>($"api/Main/GetGroups?departmentId={APIClient.teacher.DepartmentId}");
                if (serializedList != null)
                {
                    List<PlanViewModel>? list = JsonConvert.DeserializeObject<List<PlanViewModel>>(serializedList);
                    return View((list, 1, 1));
                }
                var tmp = APIClient.GetRequest<Tuple<List<PlanViewModel>, int>>
                    ($"api/Main/GetPaginatedPlans?teacherId={APIClient.teacher.Id}&page={page}");

                var model = (tmp.Item1, tmp.Item2, page);
                return View(model);
            } catch (Exception ex)
            {
                unxpError = GetCyrillic(ex.Message);
                return View();
            }
        }

        [HttpGet]
        public IActionResult FindPlan(PlanType Type, int DisciplineId, int GroupId)
        {          
            if (APIClient.teacher == null)
            {
                error = "Похожке, что вы вышли из системы";
                return Redirect("SignUp");
            }
            List<PlanViewModel> list = APIClient.GetRequest<List<PlanViewModel>>(
                $"api/Main/GetFilteredPlans?teacherId={APIClient.teacher.Id}&disciplineId={DisciplineId}&groupId={GroupId}&type={Type}");
            return RedirectToAction("Plans", "Home", new { serializedList = JsonConvert.SerializeObject(list) });
        }

        [HttpGet]
        public IActionResult Student(int id)
        {
            if (unxpError != "")
            {
                ViewData["UnxpError"] = unxpError;
                unxpError = "";
            }
            try
            {
                if (APIClient.teacher == null)
                {
                    error = "Похожке, что вы вышли из системы";
                    return Redirect("SignUp");
                }
                TestingViewModel testing = APIClient.GetRequest<TestingViewModel>($"api/Main/GetTesting?id={id}");
                return View((testing.StudentTestings, id, testing.Type));
            } catch (Exception ex)
            {
                unxpError = GetCyrillic(ex.Message);
                return View();
            }
        }

        [HttpPost]
        public void PutStudentsGrades(int testingId, List<int> studentId, List<MarkType> Grade)
        {
            if (APIClient.teacher == null)
            {
                error = "Похожке, что вы вышли из системы";
                Response.Redirect("SignUp");
                return;
            }
            try
            {
                List<StudentTestingBindingModel> list = new List<StudentTestingBindingModel>();
                for (int i = 0; i < studentId.Count; i++)
                {
                    list.Add(new StudentTestingBindingModel()
                    {
                        TestingId = testingId,
                        StudentId = studentId[i],
                        Grade = Grade[i]
                    });
                }
                APIClient.PostRequest($"api/Main/PutStudentMarks", list);
                Response.Redirect("Plans");
                return;
            } catch (Exception ex)
            {
                unxpError = GetCyrillic(ex.Message);
                Response.Redirect("Plans");
                return;
            }
        }
        //---------------------------- Plan&Marks -----------------------------

        //------------------------- Messages&Reports --------------------------
        [HttpGet]
        public IActionResult Reports(string? serializedList)
        {
            if (unxpError != "")
            {
                ViewData["UnxpError"] = unxpError;
                unxpError = "";
            }
            if (APIClient.teacher == null)
            {
                error = "Похожке, что вы вышли из системы";
                return Redirect("SignUp");
            }
            try
            {
                if (serializedList != null)
                {
                    List<MessageViewModel>? list = JsonConvert.DeserializeObject<List<MessageViewModel>>(serializedList);
                    return View(list);
                }
                var messages = APIClient.GetRequest<List<MessageViewModel>>(
                    $"api/Main/GetTeacherMessages?id={APIClient.teacher.Id}");
                return View(messages);
            } catch (Exception ex)
            {
                unxpError = GetCyrillic(ex.Message);
                return View();
            }
        }

        public IActionResult filterMessages(ReportTypes typeReport, Status status)
        {
            if (APIClient.teacher == null)
            {
                error = "Похожке, что вы вышли из системы";
                return Redirect("SignUp");
            }
            List<MessageViewModel> list = APIClient.GetRequest<List<MessageViewModel>>(
                $"api/Main/GetMessagesByStatusAndType?id={APIClient.teacher.Id}&status={status}&typeReport={typeReport}");
            return RedirectToAction("Reports", "Home", new { serializedList = JsonConvert.SerializeObject(list) });
        }
        //------------------------- Messages&Reports --------------------------

        //----------------------------- Reports -------------------------------
        [HttpGet]
        public IActionResult PlanReport(int id)
        {
            if (success != "")
            {
                ViewData["Success"] = success;
                success = "";
            }
            if (unxpError != "")
            {
                ViewData["UnxpError"] = unxpError;
                unxpError = "";
            }
            if (APIClient.teacher == null)
            {
                error = "Похожке, что вы вышли из системы";
                return Redirect("SignUp");
            }
            try
            {
                var model = APIClient.GetRequest<ReportPlanViewModel>($"api/Main/GetObjectsForPlanReport?id={id}");
                return View((model, id));
            } catch (Exception ex)
            {
                unxpError = GetCyrillic(ex.Message);
                return View();
            }
        }

        public IActionResult SavePlanReport(int messageId)
        {
            if (APIClient.teacher == null)
            {
                error = "Похожке, что вы вышли из системы";
                return Redirect("SignUp");
            }
            try
            {
                var model = APIClient.GetRequest<MessageViewModel>($"api/Main/GetMessage?id={messageId}");
                MessageBindingModel resModel = new MessageBindingModel
                {
                    Id = model.Id,
                    ReportType = model.ReportType,
                    Status = model.Status,
                    DepartmentId = model.DepartmentId,
                    PlanId = model.PlanId,
                    DisciplineId = model.DisciplineId,
                    TeacherId = model.TeacherId
                };
                APIClient.PostRequest($"api/Main/SavePlanReport", new Tuple<MessageBindingModel, string>(resModel, rootPath + "\\TeacherApp\\wwwroot\\reports\\ОтчётПоЗанятию.docx"));
                string fileName = "ОтчётПоЗанятию.docx";
                byte[] fileBytes = System.IO.File.ReadAllBytes(_hostEnvironment.WebRootPath + @"\reports\" + fileName);
                return File(fileBytes, "application/force-download", fileName);
            } catch (Exception ex)
            {
                unxpError = "Файл не был сохранён \n";
                unxpError += GetCyrillic(ex.Message);
                return Redirect($"/Home/PlanReport?id={messageId}");
            }
        }

        [HttpGet]
        public IActionResult FullReport(int id)
        {
            if (success != "")
            {
                ViewData["Success"] = success;
                success = "";
            }
            if (unxpError != "")
            {
                ViewData["UnxpError"] = unxpError;
                unxpError = "";
            }
            if (APIClient.teacher == null)
            {
                error = "Похожке, что вы вышли из системы";
                return Redirect("SignUp");
            }
            try
            {
                var model = APIClient.GetRequest<ReportFullViewModel>($"api/Main/GetObjectsForSumReport?id={id}");
                return View((model, id));
            } catch (Exception ex)
            {
                unxpError = GetCyrillic(ex.Message);
                return View();
            }
        }

        public IActionResult SaveFullReport(int messageId)
        {
            if (APIClient.teacher == null)
            {
                error = "Похожке, что вы вышли из системы";
                return Redirect("SignUp");
            }
            try
            {
                var model = APIClient.GetRequest<MessageViewModel>($"api/Main/GetMessage?id={messageId}");
                MessageBindingModel resModel = new MessageBindingModel
                {
                    Id = model.Id,
                    ReportType = model.ReportType,
                    Status = model.Status,
                    DepartmentId = model.DepartmentId,
                    PlanId = model.PlanId,
                    DisciplineId = model.DisciplineId,
                    TeacherId = model.TeacherId
                };
                APIClient.PostRequest($"api/Main/SaveSumReport", new Tuple<MessageBindingModel, string>(resModel, rootPath + "\\TeacherApp\\wwwroot\\reports\\ОтчётПолный.docx"));
                string fileName = "ОтчётПолный.docx";
                byte[] fileBytes = System.IO.File.ReadAllBytes(_hostEnvironment.WebRootPath + @"\reports\" + fileName);
                success = "Файл успешно сохранён";
                return File(fileBytes, "application/force-download", fileName);
            } catch (Exception ex)
            {
                unxpError = "Файл не был сохранён \n";
                unxpError += GetCyrillic(ex.Message);
                return Redirect($"/Home/FullReport?id={messageId}");
            }
        }
        //----------------------------- Reports -------------------------------

        //------------------------------- Mail --------------------------------
        public IActionResult SendReportByMail(int messageId)
        {
            if (APIClient.teacher == null)
            {
                error = "Похожке, что вы вышли из системы";
                return Redirect("SignUp");
            }
            var department = APIClient.GetRequest<DepartmentViewModel>($"api/Main/GetDepartment?id={APIClient.teacher.DepartmentId}&login={null}&password={null}");
            var model = APIClient.GetRequest<MessageViewModel>($"api/Main/GetMessage?id={messageId}");
            MessageBindingModel resModel = new MessageBindingModel
            {
                Id = model.Id,
                ReportType = model.ReportType,
                Status = model.Status,
                DepartmentId = model.DepartmentId,
                PlanId = model.PlanId,
                DisciplineId = model.DisciplineId,
                TeacherId = model.TeacherId
            };

            if (resModel.ReportType == ReportTypes.SumReport)
            {
                try
                {
                    APIClient.PostRequest($"api/Main/SaveSumReport", new Tuple<MessageBindingModel, string>(resModel, rootPath + "\\TeacherApp\\wwwroot\\reports\\ОтчётПолный.docx"));
                    string fileName = "ОтчётПолный.docx";
                    string filePath = _hostEnvironment.WebRootPath + @"\reports\" + fileName;
                    APIClient.PostRequest($"api/Main/SendMessage", new MailSendInfoBindingModel
                    {
                        MailAddress = department.Login,
                        Subject = "Итоговый отчёт по дисциплине " + model.DisciplineName + ", преподаватель: " + APIClient.teacher.Flm,
                        Text = "Отчёт находится в прикреплённом файле \n С уважением, \n" + APIClient.teacher.Flm,
                        FilePath = filePath
                    });
                    success = "Письмо успешно отправлено";
                    APIClient.PostRequest($"api/Main/AnswerRequest", new MessageBindingModel
                    {
                        Id = model.Id,
                        DepartmentId = model.DepartmentId
                    });
                    return Redirect($"/Home/FullReport?id={messageId}");
                } catch (Exception ex)
                {
                    unxpError = "Письмо не отправлено \n";
                    unxpError += GetCyrillic(ex.Message);
                    return Redirect($"/Home/FullReport?id={messageId}");
                }
            }
            else
            {
                try
                {
                    APIClient.PostRequest($"api/Main/SavePlanReport", new Tuple<MessageBindingModel, string>(resModel, rootPath + "\\TeacherApp\\wwwroot\\reports\\ОтчётПоЗанятию.docx"));
                    string fileName = "ОтчётПоЗанятию.docx";
                    string filePath = _hostEnvironment.WebRootPath + @"\reports\" + fileName;
                    APIClient.PostRequest($"api/Main/SendMessage", new MailSendInfoBindingModel
                    {
                        MailAddress = department.Login,
                        Subject = "Отчёт по учебному плану " + model.PlanName + ", преподаватель: " + APIClient.teacher.Flm,
                        Text = model.PlanName + "\nОтчёт находится в прикреплённом файле \n С уважением, \n" + APIClient.teacher.Flm,
                        FilePath = filePath
                    });
                    success = "Письмо успешно отправлено";
                    APIClient.PostRequest($"api/Main/AnswerRequest", new MessageBindingModel
                    {
                        Id = model.Id,
                        DepartmentId = model.DepartmentId
                    });
                    return Redirect($"/Home/PlanReport?id={messageId}");
                } catch(Exception ex)
                {
                    unxpError = "Письмо не отправлено \n";
                    unxpError += GetCyrillic(ex.Message);
                    return Redirect($"/Home/PlanReport?id={messageId}");
                }
            }
        }
        //------------------------------- Mail --------------------------------

        //----------------------------- Encoding ------------------------------
        public string GetCyrillic(string input)
        {
            string result = "";
            string pattern = @"[А-Яа-я]+[,.:;!?]*(\s+)|[А-Яа-я]+[,.:;!?]";

            foreach (Match m in Regex.Matches(input, pattern))
            {
                result += m.Value;
            }           
            return result;
        }
    }
}
