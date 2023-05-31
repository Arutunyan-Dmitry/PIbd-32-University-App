using System.ComponentModel.DataAnnotations;

namespace UniversityContracts.Enums
{
    public enum ReportTypes
    {
        [Display(Name = "Итоговый отчёт по дисциплине")]
        SumReport = 0,

        [Display(Name = "Отчёт по плану")]
        PlanReport = 1
    }
}
