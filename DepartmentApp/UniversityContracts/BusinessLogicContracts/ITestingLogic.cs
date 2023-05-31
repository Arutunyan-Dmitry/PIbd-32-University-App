using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityContracts.BindingModels;
using UniversityContracts.ViewModels;

namespace UniversityContracts.BusinessLogicContracts
{
    public interface ITestingLogic
    {
        List<TestingViewModel> Read(TestingBindingModel model);
        void CreateOrUpdate(TestingBindingModel model);
        void Delete(TestingBindingModel model);
        void AddStudent(StudentTestingBindingModel model);
        void RemoveStudent(StudentTestingBindingModel model);
    }
}
