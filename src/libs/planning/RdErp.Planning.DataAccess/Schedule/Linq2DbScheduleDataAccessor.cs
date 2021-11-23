using System;
using System.Linq;
using System.Threading.Tasks;

using LinqToDB;
using LinqToDB.Data;

namespace RdErp.Planning
{
    public class Linq2DbScheduleDataAccessor : IScheduleDataAccessor
    {
        private readonly DataConnection db;

        public Linq2DbScheduleDataAccessor(DataConnection db)
        {
            this.db = db
                ??
                throw new ArgumentNullException(nameof(db));
        }

        public async Task DeleteByPlan(int planId)
        {
            await db.GetTable<PlanningEvent>()
                .Where(e => db.GetTable<Schedule>()
                    .Any(s => s.PlanId == planId && e.ScheduleId == s.Id))
                .DeleteAsync();

            await db.GetTable<Schedule>()
                .Where(s => s.PlanId == planId)
                .DeleteAsync();
        }

        public IQueryable<Schedule> Find() => db.GetTable<Schedule>();

        public IQueryable<Schedule> ByPlan(int planId) => db.GetTable<Schedule>().Where(s => s.PlanId == planId);

        public Task<Schedule> GetById(int id) => db.GetTable<Schedule>()
            .FirstOrDefaultAsync(e => e.Id == id);

        public async Task Merge(int planId, Schedule[] schedules)
        {
            var existing = await db.GetTable<Schedule>()
                .Where(e => e.PlanId == planId)
                .ToArrayAsync();

            foreach (var itemToDelete in existing.Where(e => !schedules.Any(a => a.Id == e.Id)))
            {
                await db.GetTable<PlanningEvent>()
                    .Where(e => e.ScheduleId == itemToDelete.Id)
                    .DeleteAsync();

                await db.GetTable<ScheduleAttribute>()
                    .Where(e => e.ScheduleId == itemToDelete.Id)
                    .DeleteAsync();

                await db.DeleteAsync(itemToDelete);
            }

            foreach (var item in schedules)
            {
                if (item.Id == 0)
                {
                    item.Id = await db.InsertWithInt32IdentityAsync(item);
                }
                else
                {
                    await db.UpdateAsync(item);
                }
            }
        }
    }
}