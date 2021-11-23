using System;
using System.Linq;
using System.Threading.Tasks;

namespace RdErp.Planning
{
    public interface IScheduleAttributeDataAccessor
    {
        IQueryable<ScheduleAttribute> GetByPlan(int planId);

        IQueryable<ScheduleAttribute> GetBySchedule(int scheduleId);

        Task Merge(int scheduleId, ScheduleAttribute[] attributes);

        Task DeleteBySchedule(int scheduleId);
    }
}