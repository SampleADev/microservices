using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using LinqToDB;
using LinqToDB.Data;
using static LinqToDB.Sql;

namespace RdErp.Planning
{
    public class Linq2DbPlanningEventDataAccessor : IPlanningEventDataAccessor
    {
        private readonly DataConnection db;

        public Linq2DbPlanningEventDataAccessor(DataConnection db)
        {
            this.db = db
                ??
                throw new ArgumentNullException(nameof(db));
        }

        [TableFunction("planning_events_info", Schema = "plan")]
        public ITable<PlanningEventInfo> All(string currency)
        {
            return db.GetTable<PlanningEventInfo>(this, (MethodInfo) MethodBase.GetCurrentMethod(), currency);
        }

        public IQueryable<PlanningEventInfo> AllByPlan(int planId, string currency) => this.All(currency)
            .Where(e => e.PlanId == planId);

        public IQueryable<PlanningEventInfo> AllInDateRange(DateTime startDate, DateTime endDate, string currency) =>
            this
            .All(currency)
            .Where(e => e.OccurredAt >= startDate && e.OccurredAt < endDate);

        public Task DeleteScheduleEvents(int scheduleId) => db.GetTable<PlanningEvent>()
            .Where(e => e.ScheduleId == scheduleId)
            .DeleteAsync();

        public void Insert(IEnumerable<PlanningEvent> planningEvents)
        {
            db.BulkCopy(new BulkCopyOptions()
            {
                BulkCopyType = BulkCopyType.MultipleRows,
                    NotifyAfter = 0
            }, planningEvents);
        }

        public Task<PlanningEvent> LastPlanningEvent(int scheduleId) => db.GetTable<PlanningEvent>()
            .Where(e => e.ScheduleId == scheduleId)
            .OrderByDescending(e => e.Ordinal)
            .FirstOrDefaultAsync();
    }
}