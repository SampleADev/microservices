using System;
using System.Threading.Tasks;

using RdErp.Financial;

namespace RdErp.Reporting.FinancialResults
{
    public interface IMonthlyPlanFactResultsReportDataAccessor
    {
        Task<MonthlyPlanFactResultsRecord[]> GetMonthlyPlanFactResultsReportData(string currency);
    }
}