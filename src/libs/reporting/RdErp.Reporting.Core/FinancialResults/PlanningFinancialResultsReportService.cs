using System;
using System.Threading.Tasks;

namespace RdErp.Reporting.FinancialResults
{
    public class PlanningFinancialResultsReportService : IPlanningFinancialResultsReportService
    {
        private readonly IPlanningFinancialResultsReportDataAccessor planningFinancialResultsReportDataAccessor;

        public PlanningFinancialResultsReportService(IPlanningFinancialResultsReportDataAccessor planningFinancialResultsReportDataAccessor)
        {
            this.planningFinancialResultsReportDataAccessor = planningFinancialResultsReportDataAccessor
                ??
                throw new ArgumentNullException(nameof(planningFinancialResultsReportDataAccessor));
        }

        public async Task<PlanningFinancialResultsReportData> PlanningFinancialResultsReport(string currency, DateTime planFactSplitDate)
        {
            var endOfTheDate = new DateTime(planFactSplitDate.Year, planFactSplitDate.Month, planFactSplitDate.Day);

            var latestTransaction = await planningFinancialResultsReportDataAccessor.GetLastTransactionBeforeDate(endOfTheDate);
            planFactSplitDate = latestTransaction != null ? latestTransaction.OccurredAt.Date.AddDays(1) : planFactSplitDate;

            return new PlanningFinancialResultsReportData
            {
                PlanFactSplitDate = latestTransaction == null ? default(DateTime?) : planFactSplitDate,
                    EventRecords = await planningFinancialResultsReportDataAccessor.GetPlanningFinancialResultsReportData(currency, planFactSplitDate)
            };
        }
    }
}