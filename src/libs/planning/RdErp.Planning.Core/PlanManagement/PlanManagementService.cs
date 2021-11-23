using System;
using System.Linq;
using System.Threading.Tasks;

using FluentValidation;

using LinqToDB;

namespace RdErp.Planning
{
    public class PlanManagementService : IPlanManagementService
    {
        private readonly IScheduleManagementService scheduleService;
        private readonly IPlanningEventManagementService planningEventService;
        private readonly ITransactionManager transactionManager;
        private readonly IValidator<Plan> planValidator;
        private readonly IPlanDataAccessor planDataAccessor;

        public PlanManagementService(
            IScheduleManagementService scheduleService,
            IPlanningEventManagementService planningEventService,
            ITransactionManager transactionManager,
            IValidator<Plan> planValidator,
            IPlanDataAccessor planDataAccessor)
        {
            this.scheduleService = scheduleService
                ??
                throw new ArgumentNullException(nameof(scheduleService));
            this.planningEventService = planningEventService
                ??
                throw new ArgumentNullException(nameof(planningEventService));
            this.transactionManager = transactionManager
                ??
                throw new ArgumentNullException(nameof(transactionManager));
            this.planValidator = planValidator
                ??
                throw new ArgumentNullException(nameof(planValidator));
            this.planDataAccessor = planDataAccessor
                ??
                throw new ArgumentNullException(nameof(planDataAccessor));
        }

        public async Task DeletePlan(int planId)
        {
            await transactionManager.InTransaction(async() =>
            {
                await scheduleService.DeleteByPlan(planId);
                await planDataAccessor.Delete(planId);
            });
        }

        public async Task<PageResult<Plan>> All(ListRequest request)
        {
            var query = planDataAccessor.All();

            if (!String.IsNullOrWhiteSpace(request.Search))
            {
                var isNumber = Decimal.TryParse(request.Search, out decimal searchNumber);

                query = query.Where(t =>
                    t.Name.ToLower().Contains(request.Search.ToLower())
                    || (isNumber && (int) t.Id == (int) searchNumber)
                );
            }

            request.SortBy = String.IsNullOrWhiteSpace(request.SortBy)
                ? nameof(Plan.Id)
                : request.SortBy;

            return await query.ApplyListRequest(request,
                nameof(Plan.Id),
                nameof(Plan.Name),
                nameof(Plan.IsActive)
            );
        }

        public async Task<PlanningEventInfo[]> GetGeneratedPlanEvents(int planId)
        {
            return await this.planningEventService.AllByPlan(planId);
        }

        public async Task < (Plan, ScheduleDetails[]) > GetPlan(int planId)
        {
            var plan = await planDataAccessor.GetById(planId);
            var schedules = await scheduleService.GetByPlan(planId);

            return (plan, schedules);
        }

        public async Task RegeneratePlanningEvents(int planId)
        {
            var schedules = await scheduleService.GetByPlan(planId);

            foreach (var schedule in schedules.Where(s => s.IsActive))
            {
                await planningEventService.RegeneratePlanningEvents(schedule);
            }
        }

        public async Task RegeneratePlanningEvents()
        {
            var plans = await planDataAccessor.All()
                .Where(p => p.IsActive)
                .ToArrayAsync();

            foreach (var plan in plans)
            {
                await RegeneratePlanningEvents(plan.Id);
            }
        }

        public async Task < (Plan plan, ScheduleDetails[] schedules) > Save(Plan plan, ScheduleDetails[] schedules)
        {
            if (plan == null) throw new ArgumentNullException(nameof(plan));

            await planValidator.ValidateExtAsync(plan).AndThrow();

            await transactionManager.InTransaction(async() =>
            {
                if (plan.Id != 0)
                {
                    await planDataAccessor.Update(plan);
                }
                else
                {
                    plan.Id = await planDataAccessor.Insert(plan);
                }

                await scheduleService.Save(plan.Id, schedules);
            });

            await RegeneratePlanningEvents(plan.Id);

            return await GetPlan(plan.Id);
        }
    }
}