using System;
using System.Threading.Tasks;

using RdErp.Financial;

namespace RdErp.Reporting.FinancialResults
{
    public interface IPlanningFinancialResultsReportDataAccessor
    {
        Task<FinancialTransaction> GetLastTransactionBeforeDate(DateTime beforeDate);

        Task<PlanningEventRecord[]> GetPlanningFinancialResultsReportData(string currency, DateTime planFactSplitDate);
    }
}