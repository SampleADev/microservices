using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RdErp.Planning
{
    public interface IPlanningEventDataAccessor
    {
        Task DeleteScheduleEvents(int scheduleId);

        void Insert(IEnumerable<PlanningEvent> planningEvents);

        Task<PlanningEvent> LastPlanningEvent(int scheduleId);

        IQueryable<PlanningEventInfo> AllByPlan(int planId, string currency);

        IQueryable<PlanningEventInfo> AllInDateRange(DateTime startDate, DateTime endDate, string currency);
    }
}