using System;
using System.Linq;
using System.Threading.Tasks;

using LinqToDB;

namespace RdErp.Planning
{
    public class PlanningEventManagementService : IPlanningEventManagementService
    {
        private readonly IPlanningEventDataAccessor planningEventDataAccessor;
        private readonly IPlanningEventGenerationService planningEventGenerator;
        private readonly ITransactionManager transactionManager;

        public PlanningEventManagementService(
            IPlanningEventDataAccessor planningEventDataAccessor,
            IPlanningEventGenerationService planningEventGenerator,
            ITransactionManager transactionManager)
        {
            this.planningEventDataAccessor = planningEventDataAccessor
                ??
                throw new ArgumentNullException(nameof(planningEventDataAccessor));
            this.planningEventGenerator = planningEventGenerator
                ??
                throw new ArgumentNullException(nameof(planningEventGenerator));
            this.transactionManager = transactionManager
                ??
                throw new ArgumentNullException(nameof(transactionManager));
        }

        public Task<PlanningEventInfo[]> AllByMonth(DateTimeOffset month, string currency)
        {
            var startOfTheMonth = new DateTime(month.Year, month.Month, 1);
            var endOfTheMonth = startOfTheMonth.AddMonths(1);

            return planningEventDataAccessor.AllInDateRange(startOfTheMonth, endOfTheMonth, currency)
                .ToArrayAsync();
        }

        public async Task<PlanningEventInfo[]> AllByPlan(int planId) =>
            await planningEventDataAccessor.AllByPlan(planId, "UAH").ToArrayAsync();

        public Task RegeneratePlanningEvents(ScheduleDetails schedule)
        {
            if (schedule == null) throw new ArgumentNullException(nameof(schedule));

            if (schedule.Id == 0) throw new ArgumentException("Schedule should be saved before generating events", nameof(schedule));

            return transactionManager.InTransaction(async() =>
            {
                await planningEventDataAccessor.DeleteScheduleEvents(schedule.Id);
                var eventSequence = (await planningEventGenerator.GenerateEvents(
                    schedule,
                    0,
                    schedule.StartDate
                )).TakeWhile(e => e.OccurredAt < (schedule.EndDate ?? schedule.StartDate.AddYears(1).AddDays(1)));

                planningEventDataAccessor.Insert(eventSequence);
            });
        }
    }
}