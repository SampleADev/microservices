using System;
using System.Linq;
using System.Threading.Tasks;

using FluentValidation;

using LinqToDB;

using Mapster;

namespace RdErp.Planning
{
    public class ScheduleManagementService : IScheduleManagementService
    {
        private readonly IValidator<ScheduleDetails> scheduleDetailsValidator;
        private readonly ITransactionManager transactionManager;
        private readonly IScheduleDataAccessor scheduleDataAccessor;
        private readonly IScheduleAttributeDataAccessor scheduleAttributeDataAccessor;
        private readonly IPlanningEventManagementService planningEventManagementService;

        public ScheduleManagementService(
            IValidator<ScheduleDetails> scheduleDetailsValidator,
            ITransactionManager transactionManager,
            IScheduleDataAccessor scheduleDataAccessor,
            IScheduleAttributeDataAccessor scheduleAttributeDataAccessor,
            IPlanningEventManagementService planningEventManagementService
        )
        {
            this.scheduleDetailsValidator = scheduleDetailsValidator
                ??
                throw new ArgumentNullException(nameof(scheduleDetailsValidator));
            this.transactionManager = transactionManager
                ??
                throw new ArgumentNullException(nameof(transactionManager));
            this.scheduleDataAccessor = scheduleDataAccessor
                ??
                throw new ArgumentNullException(nameof(scheduleDataAccessor));
            this.scheduleAttributeDataAccessor = scheduleAttributeDataAccessor
                ??
                throw new ArgumentNullException(nameof(scheduleAttributeDataAccessor));
            this.planningEventManagementService = planningEventManagementService
                ??
                throw new ArgumentNullException(nameof(planningEventManagementService));
        }

        public IQueryable<Schedule> All() => scheduleDataAccessor.Find();

        public async Task<ScheduleDetails[]> GetByPlan(int planId)
        {
            var schedules = await scheduleDataAccessor
                .Find()
                .Where(s => s.PlanId == planId)
                .ToArrayAsync();

            var attributes = await scheduleAttributeDataAccessor
                .GetByPlan(planId)
                .ToArrayAsync();

            return schedules.Select(s => s.Adapt<ScheduleDetails>())
                .Select(a =>
                {
                    a.Attributes = attributes.Where(i => i.ScheduleId == a.Id)
                        .ToArray();
                    return a;
                })
                .ToArray();
        }

        public async Task<ScheduleDetails> GetById(int scheduleId)
        {
            var schedule = await scheduleDataAccessor.GetById(scheduleId);
            if (schedule == null)
            {
                return null;
            }

            var attributes = await scheduleAttributeDataAccessor
                .GetBySchedule(scheduleId)
                .ToArrayAsync();

            var result = schedule.Adapt<ScheduleDetails>();
            result.Attributes = attributes;

            return result;
        }

        public Task DeleteByPlan(int planId) =>
            transactionManager.InTransaction(() => scheduleDataAccessor.DeleteByPlan(planId));

        public async Task Save(int planId, ScheduleDetails[] schedules)
        {
            if (planId == 0) throw new ArgumentNullException(nameof(planId));
            if (schedules == null) throw new ArgumentNullException(nameof(schedules));

            foreach (var schedule in schedules)
            {
                schedule.PlanId = planId;
            }

            await scheduleDetailsValidator.ValidateExtAsync(
                schedules,
                "schedules"
            ).AndThrow();

            await transactionManager.InTransaction(async() =>
            {
                var existing = await scheduleDataAccessor
                    .ByPlan(planId)
                    .ToArrayAsync();

                var schedulesToRegenerateEvents = schedules.Where(
                        s => s.Id == 0
                        || ShouldRegenerateEvents(s, existing.FirstOrDefault(a => a.Id == s.Id))
                    )
                    .Select(s => s.Id)
                    .ToArray();

                await scheduleDataAccessor.Merge(planId, schedules);

                foreach (var item in schedules)
                {
                    await scheduleAttributeDataAccessor.Merge(item.Id, item.Attributes);
                }

                var regenerateEventSchedules = (await GetByPlan(planId))
                    .Where(s => schedulesToRegenerateEvents.Contains(s.Id))
                    .ToArray();

                foreach (var item in regenerateEventSchedules)
                {
                    await planningEventManagementService
                        .RegeneratePlanningEvents(item);
                }
            });
        }

        public async Task RegenerateScheduleEvents(int scheduleId)
        {
            var schedule = await GetById(scheduleId);

            if (schedule != null)
            {
                await planningEventManagementService
                    .RegeneratePlanningEvents(schedule);
            }
        }

        private bool ShouldRegenerateEvents(Schedule a, Schedule b)
        {
            if (a == null || b == null)
            {
                return true;
            }

            return a.Currency != b.Currency
                || a.EndDate != b.EndDate
                || a.ScheduleRule != b.ScheduleRule
                || a.ScheduleSettings != b.ScheduleSettings
                || a.ValueExpression != b.ValueExpression
                || a.StartDate != b.StartDate;
        }
    }
}