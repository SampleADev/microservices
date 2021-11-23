using System;
using System.Threading.Tasks;

namespace RdErp.Reporting.FinancialResults
{
    public interface IMonthlyPlanFactResultsReportService
    {
        /// <summary>
        /// Gets the data about monthly
        /// planned financial results (calculated with planning events) and
        /// fact financial results (calculated based on transactions).
        /// </summary>
        Task<MonthlyPlanFactResultsRecord[]> MonthlyPlanFactResultsReport(string currency);
    }

}