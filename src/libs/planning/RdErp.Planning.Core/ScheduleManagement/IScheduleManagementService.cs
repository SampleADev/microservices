using System;
using System.Linq;
using System.Threading.Tasks;

namespace RdErp.Planning
{
    public interface IScheduleManagementService
    {
        IQueryable<Schedule> All();

        Task<ScheduleDetails[]> GetByPlan(int planId);

        Task Save(int planId, ScheduleDetails[] schedules);

        Task DeleteByPlan(int planId);
    }
}