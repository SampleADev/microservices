using System;
using System.Linq;
using System.Threading.Tasks;

namespace RdErp.Planning
{
    public interface IPlanManagementService
    {
        Task<PageResult<Plan>> All(ListRequest request);

        Task < (Plan, ScheduleDetails[]) > GetPlan(int planId);

        Task<PlanningEventInfo[]> GetGeneratedPlanEvents(int planId);

        Task DeletePlan(int planId);

        /// <summary>
        /// Creates or updates a plan and optionally schedules.
        ///
        /// If <paramref name="schedules">schedules<paramref> is null,
        ///     just modifies the plan and leaves schedules untouched.
        /// If <paramref name="schedules">schedules</paramref> array is provided,
        ///     replaces the schedules:
        ///     * modifies schedules with matched identifier
        ///     * creates schedules with zero identifier
        ///     * removes rest of schedules
        /// </summary>
        Task < (Plan plan, ScheduleDetails[] schedules) > Save(Plan plan, ScheduleDetails[] schedules);

        Task RegeneratePlanningEvents(int planId);

        Task RegeneratePlanningEvents();
    }
}