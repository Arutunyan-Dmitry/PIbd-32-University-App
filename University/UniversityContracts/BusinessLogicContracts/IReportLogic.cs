using UniversityContracts.BindingModels;
using UniversityContracts.ViewModels;

namespace UniversityContracts.BusinessLogicContracts
{
    public interface IReportLogic
    {
        ReportGroupViewModel GetObjectsForGroupReport(int id);
        ReportFullViewModel GetObjectsForSumReport(MessageBindingModel model);
        ReportPlanViewModel GetObjectsForPlanReport(MessageBindingModel model);
        void SendGroupReport(int id);
        void SaveSumReport(MessageBindingModel model, string filename);
        void SavePlanReport(MessageBindingModel model, string filename);
    }
}
