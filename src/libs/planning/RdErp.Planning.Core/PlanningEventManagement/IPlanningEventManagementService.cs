using System;
using System.Threading.Tasks;

namespace RdErp.Planning
{
    public interface IPlanningEventManagementService
    {
        Task RegeneratePlanningEvents(ScheduleDetails schedule);

        Task<PlanningEventInfo[]> AllByPlan(int planId);

        Task<PlanningEventInfo[]> AllByMonth(DateTimeOffset month, string currency);
    }
}