using UniversityContracts.BindingModels;
using UniversityContracts.ViewModels;

namespace UniversityContracts.BusinessLogicContracts
{
    public interface IReportLogic
    {
        ReportFullViewModel GetObjectsForSumReport(MessageBindingModel model);
        ReportPlanViewModel GetObjectsForPlanReport(MessageBindingModel model);
        void SaveSumReport(MessageBindingModel model, string filename);
        void SavePlanReport(MessageBindingModel model, string filename);
    }
}
