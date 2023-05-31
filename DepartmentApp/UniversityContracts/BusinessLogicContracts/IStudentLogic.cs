using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityContracts.BindingModels;
using UniversityContracts.ViewModels;

namespace UniversityContracts.BusinessLogicContracts
{
    public interface IStudentLogic
    {
        List<StudentViewModel> Read(StudentBindingModel model);
        void CreateOrUpdate(StudentBindingModel model);
        void Delete(StudentBindingModel model);
    }
}
