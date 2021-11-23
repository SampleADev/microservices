using System;
using System.Reflection;
using System.Threading.Tasks;

using LinqToDB;
using LinqToDB.Data;

using RdErp.Financial;
using static LinqToDB.Sql;

namespace RdErp.Reporting.FinancialResults
{
    public class Linq2DbMonthlyPlanFactResultsReportDataAccessor : IMonthlyPlanFactResultsReportDataAccessor
    {
        private readonly DataConnection db;

        public Linq2DbMonthlyPlanFactResultsReportDataAccessor(DataConnection db)
        {
            this.db = db
                ??
                throw new ArgumentNullException(nameof(db));
        }

        public async Task<MonthlyPlanFactResultsRecord[]> GetMonthlyPlanFactResultsReportData(string currency)
        {
            return await MonthlyPlanFactResultsFunction(currency)
                .ToArrayAsync();
        }

        [TableFunction("rep.monthly_plan_fact_results")]
        public ITable<MonthlyPlanFactResultsRecord> MonthlyPlanFactResultsFunction(string currency) =>
            db.GetTable<MonthlyPlanFactResultsRecord>(this, (MethodInfo) MethodInfo.GetCurrentMethod(), currency);
    }
}