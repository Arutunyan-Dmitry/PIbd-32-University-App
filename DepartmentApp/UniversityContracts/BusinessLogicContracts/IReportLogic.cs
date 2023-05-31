using UniversityContracts.BindingModels;
using UniversityContracts.ViewModels;

namespace UniversityContracts.BusinessLogicContracts
{
    public interface IReportLogic
    {
        ReportFullViewModel GetObjectsForSumReport(MessageBindingModel model);
        ReportPlanViewModel GetObjectsForLessonReport(MessageBindingModel model);
        void SaveSumReport(MessageBindingModel model, string filename);
        void SaveLessonReport(MessageBindingModel model, string filename);
    }
}
