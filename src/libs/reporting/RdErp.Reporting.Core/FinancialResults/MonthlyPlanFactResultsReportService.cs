using System;
using System.Threading.Tasks;

namespace RdErp.Reporting.FinancialResults
{
    public class MonthlyPlanFactResultsReportService : IMonthlyPlanFactResultsReportService
    {
        private readonly IMonthlyPlanFactResultsReportDataAccessor dataAccessor;

        public MonthlyPlanFactResultsReportService(IMonthlyPlanFactResultsReportDataAccessor dataAccessor)
        {
            this.dataAccessor = dataAccessor
                ??
                throw new ArgumentNullException(nameof(dataAccessor));
        }

        public Task<MonthlyPlanFactResultsRecord[]> MonthlyPlanFactResultsReport(string currency)
        {
            if (string.IsNullOrWhiteSpace(currency))
                throw new ArgumentException("message", nameof(currency));

            return dataAccessor.GetMonthlyPlanFactResultsReportData(currency);
        }
    }
}