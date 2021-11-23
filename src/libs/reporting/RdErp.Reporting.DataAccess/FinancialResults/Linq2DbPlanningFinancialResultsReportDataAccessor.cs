using System;
using System.Reflection;
using System.Threading.Tasks;

using LinqToDB;
using LinqToDB.Data;

using RdErp.Financial;
using static LinqToDB.Sql;

namespace RdErp.Reporting.FinancialResults
{
    public class Linq2DbPlanningFinancialResultsReportDataAccessor : IPlanningFinancialResultsReportDataAccessor
    {
        private readonly DataConnection db;

        public Linq2DbPlanningFinancialResultsReportDataAccessor(DataConnection db)
        {
            this.db = db
                ??
                throw new ArgumentNullException(nameof(db));
        }

        public async Task<FinancialTransaction> GetLastTransactionBeforeDate(DateTime beforeDate) =>
            await db.GetTable<FinancialTransaction>()
            .FirstOrDefaultAsync(t => t.OccurredAt < beforeDate);

        public async Task<PlanningEventRecord[]> GetPlanningFinancialResultsReportData(string currency, DateTime planFactSplitDate)
        {
            return await PlanningFinancialResultsFunction(currency, planFactSplitDate.ToString("yyyy-MM-dd"))
                .ToArrayAsync();
        }

        [TableFunction("rep.planned_financial_results")]
        public ITable<PlanningEventRecord> PlanningFinancialResultsFunction(string currency, string actualizationDate) =>
            db.GetTable<PlanningEventRecord>(this, (MethodInfo) MethodInfo.GetCurrentMethod(), currency, actualizationDate);
    }
}