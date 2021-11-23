using System;
using System.Threading.Tasks;

namespace RdErp.Reporting.FinancialResults
{
    public interface IPlanningFinancialResultsReportService
    {
        /// <summary>
        /// Gets the planning financial results report data.
        /// Report accepts <paramref name="planFactSplitDate" /> which determines the date before that
        /// planning events are discarded and instead financial transactions aggregated and used as initial amount.
        /// 
        /// <paramref name="planFactSplitDate" /> are clarified before getting report data.
        /// Actually method searches for latest fin transaction before that date and uses next day 
        /// (to include that latest fin transaction in fact amount calculation).
        /// </summary>
        Task<PlanningFinancialResultsReportData> PlanningFinancialResultsReport(string currency, DateTime planFactSplitDate);
    }

}