using System;
using System.Linq;
using System.Threading.Tasks;

namespace RdErp.Planning
{
    public interface IScheduleDataAccessor
    {
        IQueryable<Schedule> Find();

        IQueryable<Schedule> ByPlan(int planId);

        Task<Schedule> GetById(int id);

        Task Merge(int planId, Schedule[] schedules);

        Task DeleteByPlan(int planId);
    }
}