using System;
using System.Linq;
using System.Threading.Tasks;

using LinqToDB;
using LinqToDB.Data;

namespace RdErp.Planning
{
    public class Linq2DbScheduleAttributeDataAccessor : IScheduleAttributeDataAccessor
    {
        private readonly DataConnection db;

        public Linq2DbScheduleAttributeDataAccessor(DataConnection db)
        {
            this.db = db
                ??
                throw new ArgumentNullException(nameof(db));
        }

        public IQueryable<ScheduleAttribute> GetByPlan(int planId) => db
            .GetTable<ScheduleAttribute>()
            .Where(a => db.GetTable<Schedule>()
                .Any(s => s.PlanId == planId && s.Id == a.ScheduleId));

        public Task DeleteBySchedule(int scheduleId) => db
            .GetTable<ScheduleAttribute>()
            .Where(a => a.ScheduleId == scheduleId)
            .DeleteAsync();

        public async Task Merge(int scheduleId, ScheduleAttribute[] attributes)
        {
            var existingIds = attributes.Select(a => a.Id)
                .Where(id => id != 0)
                .ToArray();

            await GetBySchedule(scheduleId)
                .Where(s => !existingIds.Contains(s.Id))
                .DeleteAsync();

            foreach (var a in attributes)
            {
                a.ScheduleId = scheduleId;
                if (a.Id == 0)
                {
                    a.Id = await db.InsertWithInt32IdentityAsync(a);
                }
                else
                {
                    await db.UpdateAsync(a);
                }
            }
        }

        public IQueryable<ScheduleAttribute> GetBySchedule(int scheduleId) => db
            .GetTable<ScheduleAttribute>()
            .Where(s => s.ScheduleId == scheduleId);
    }
}