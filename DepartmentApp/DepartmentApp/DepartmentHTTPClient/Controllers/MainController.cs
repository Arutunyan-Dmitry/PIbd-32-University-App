using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Text.RegularExpressions;
using UniversityContracts.BindingModels;
using UniversityContracts.ViewModels;

namespace DepartmentHTTPClient.Controllers
{
    public class MainController
    {
        [HttpGet]
        public List<DepartmentViewModel> GetDepartments()
        {
            return APIClient.GetRequest<List<DepartmentViewModel>>($"api/Main/GetAllDepartments");
        }
        
        [HttpPost]
        //----------------------------------------------------------------- Department ---------------------------------------------------------------------------
        public void CreateOrUpdateDepartment(DepartmentBindingModel model)
        {
            APIClient.PostRequest($"api/Main/CreateOrUpdateDepartment", model);
        }
        [HttpGet]
        public DepartmentViewModel AuthorizationDepartment(DepartmentBindingModel model)
        {
            return APIClient.GetRequest<DepartmentViewModel>($"api/Main/AuthorizationDepartment?login={model.Login}&password={model.Password}");
        }

        //----------------------------------------------------------------- Teachers ------------------------------------------------------------------------------
        [HttpPost]
        public void CreateOrUpdateTeacher(TeacherBindingModel model)
        {
            APIClient.PostRequest($"api/Main/CreateOrUpdateTeacher", model);
        }
        [HttpGet]
        public List<TeacherViewModel> GetTeachers(int departmentId)
        {
            return APIClient.GetRequest<List<TeacherViewModel>>($"api/Main/GetTeachers?departmentId={departmentId}");
        }
        [HttpGet]
        public bool DeleteTeacher(int id)
        {
            return APIClient.GetRequest<bool>($"api/Main/DeleteTeacher?id={id}");
        }
        [HttpGet]
        public TeacherViewModel? GetTeacher(int id, string? login, string? password)
        {
            return APIClient.GetRequest<TeacherViewModel>($"api/Main/GetTeacher?id={id}&login={login}&password={password}");
        }
        //------------------------------------------------------------------ Disciplines ----------------------------------------------------------
        [HttpPost]
        public void CreateOrUpdateDiscipline(DisciplineBindingModel model)
        {
            APIClient.PostRequest($"api/Main/CreateOrUpdateDiscipline", model);
        }
        [HttpGet]
        public List<DisciplineViewModel> GetDisciplines(int id)
        {
            return APIClient.GetRequest<List<DisciplineViewModel>>($"api/Main/GetDisciplines?departmentId={id}");
        }
        [HttpGet]
        public DisciplineViewModel GetDiscipline(int id)
        {
            return APIClient.GetRequest<DisciplineViewModel>($"api/Main/GetDiscipline?id={id}");
        }
        [HttpGet]
        public List<TeacherViewModel> GetTeacherByDiscipline(int disciplineId)
        {
            return APIClient.GetRequest<List<TeacherViewModel>>($"api/Main/GetTeacherByDiscipline?disciplineId={disciplineId}");
        }
        [HttpGet]
        public bool AddTeachersToDiscipline(int disciplineId, int teacherId)
        {
            return APIClient.GetRequest<bool>($"api/Main/AddTeachersToDiscipline?disciplineId={disciplineId}&teacherId={teacherId}");
        }
        [HttpGet]
        public bool RemoveTeacerFromDiscipline(int teacherId, int disciplineId)
        {
            return APIClient.GetRequest<bool>($"api/Main/RemoveTeacerFromDiscipline?disciplineId={disciplineId}&teacherId={teacherId}");
        }
        [HttpGet]
        public List<DisciplineViewModel> GetDisciplinesByName(string disciplineName,int departmentId)
        {
            return APIClient.GetRequest<List<DisciplineViewModel>>($"api/Main/GetDisciplinesByName?disciplineName={disciplineName}&departmentId={departmentId}");
        }
        [HttpGet]
        public bool DeleteDiscipline(int id)
        {
            return APIClient.GetRequest<bool>($"api/Main/DeleteDiscipline?id={id}");
        }
        //---------------------------------------------------------- Plans --------------------------------------------------------------------
        [HttpPost]
        public void CreateOrUpdatePlan(PlanBindingModel model)
        {
            APIClient.PostRequest($"api/Main/CreateOrUpdatePlan", model);
        }
        [HttpGet]
        public List<PlanViewModel> GetFilteredPlans(int teacherId, int disciplineId, int groupId, string date)
        {
            return  APIClient.GetRequest<List<PlanViewModel>>($"api/Main/GetFilteredPlans?teacherId={teacherId}&disciplineId={disciplineId}&groupId={groupId}&date={date}");
        }

        [HttpGet]
        public List<PlanViewModel> GetPlansByDiscipline(int disciplineId)
        {
            return APIClient.GetRequest<List<PlanViewModel>>($"api/Main/GetPlansByDiscipline?disciplineId={disciplineId}");
        }
        [HttpGet]
        public bool DeleteLesson(int id)
        {
            return APIClient.GetRequest<bool>($"api/Main/DeleteLesson?id={id}");
        }
        [HttpGet]
        public List<PlanViewModel> GetDepartmentPlans(int departmentId)
        {
            return APIClient.GetRequest<List<PlanViewModel>>($"api/Main/GetDepartmentPlans?departmentId={departmentId}");
        }
        //---------------------------------------------------------- Groups -------------------------------------------------------------------
        [HttpPost]
        public void CreateOrUpdateGroup(GroupBindingModel model)
        {
            APIClient.PostRequest($"api/Main/CreateOrUpdateGroup", model);
        }
        [HttpGet]
        public List<GroupViewModel> GetGroups(int departmentId)
        {
            return APIClient.GetRequest<List<GroupViewModel>>($"api/Main/GetGroups?departmentId={departmentId}");
        }        
        [HttpGet]
        public List<GroupViewModel> GetGroupsByName(string groupName, int departmentId)
        {
            return APIClient.GetRequest<List<GroupViewModel>>($"api/Main/GetGroupsByName?groupName={groupName}&departmentId={departmentId}");
        }
        [HttpGet]
        public GroupViewModel? GetGroup(int id)
        {
            return APIClient.GetRequest<GroupViewModel>($"api/Main/GetGroup?id={id}");

        }
        [HttpGet]
        public bool DeleteGroup(int id)
        {
            return APIClient.GetRequest<bool>($"api/Main/DeleteGroup?id={id}");
        }
        //---------------------------------------------------------- Students -----------------------------------------------------------------
        [HttpPost]
        public void CreateOrUpdateStudent(StudentBindingModel model)
        {
            APIClient.PostRequest($"api/Main/CreateOrUpdateStudent", model);
        }
        [HttpGet]
        public List<StudentViewModel> GetStudents(int groupId)
        {
            return APIClient.GetRequest<List<StudentViewModel>>($"api/Main/GetStudents?groupId={groupId}");
        }
        [HttpGet]
        public StudentViewModel? GetStudent(int id)
        {
            return APIClient.GetRequest<StudentViewModel>($"api/Main/GetStudent?id={id}");
        }
        [HttpGet]
        public bool DeleteStudent(int id)
        {
            return APIClient.GetRequest<bool>($"api/Main/DeleteStudent?id={id}");
        }       
        [HttpGet]
        public List<StudentViewModel> GetStudentsByDepartment(int departmentId)
        {
            return APIClient.GetRequest<List<StudentViewModel>>($"api/Main/GetStudentsByDepartment?departmentId={departmentId}");
        }
        //----------------------------------------------------------- Messages ---------------------------------------------------------
        [HttpPost]
        public void CreateMessage(MessageBindingModel model)
        {
            APIClient.PostRequest($"api/Main/CreateMessage", model);
        }
        [HttpGet]
        public List<MessageViewModel> GetDepartmentMessages(int id)
        {
            return APIClient.GetRequest<List<MessageViewModel>>($"api/Main/GetDepartmentMessages?id={id}");
        }
        [HttpGet]
        public MessageViewModel? GetMessage(int id)
        {
            return APIClient.GetRequest<MessageViewModel>($"api/Main/GetMessage?id={id}");
        }
        [HttpPost]
        public void CloseRequest(MessageBindingModel model)
        {
            APIClient.PostRequest($"api/Main/CloseRequest", model);
        }
        [HttpPost]
        public void MessageRollBack(MessageBindingModel model)
        {
            APIClient.PostRequest($"api/Main/MessageRollBack", model);
        }
    }
}
