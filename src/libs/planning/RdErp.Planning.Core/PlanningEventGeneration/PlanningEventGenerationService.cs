using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using RdErp.Planning.EventGeneration;

namespace RdErp.Planning
{
    public class PlanningEventGenerationService : IPlanningEventGenerationService
    {
        private readonly IScheduleGeneratorValidator validator;
        private readonly IValueExpressionEvaluatorFactory valueExpressionEvaluatorFactory;
        private readonly IEnumerable<IEventDateGenerator> eventDateGenerators;

        public PlanningEventGenerationService(
            IScheduleGeneratorValidator validator,
            IValueExpressionEvaluatorFactory valueExpressionEvaluatorFactory,
            IEnumerable<IEventDateGenerator> eventDateGenerators)
        {
            this.validator = validator
                ??
                throw new ArgumentNullException(nameof(validator));

            this.valueExpressionEvaluatorFactory = valueExpressionEvaluatorFactory
                ??
                throw new ArgumentNullException(nameof(valueExpressionEvaluatorFactory));

            this.eventDateGenerators = eventDateGenerators
                ??
                throw new ArgumentNullException(nameof(eventDateGenerators));
        }

        public async Task<IEnumerable<PlanningEvent>> GenerateEvents(
            ScheduleDetails schedule,
            int lastEventOrdinal,
            DateTime startDate
        )
        {
            if (schedule == null) throw new ArgumentNullException(nameof(schedule));

            await validator
                .ValidateExtAsync(schedule, nameof(schedule))
                .AndThrow();

            var scheduler = eventDateGenerators
                .FirstOrDefault(e =>
                    StringComparer.OrdinalIgnoreCase.Equals(e.Name, schedule.ScheduleRule))
                ??
                throw new ArgumentException($"Can't find scheduler with name {schedule.ScheduleRule}.", nameof(schedule));

            var settings = scheduler.ParseSettings(schedule.ScheduleSettings);
            var amountEval = valueExpressionEvaluatorFactory.CreateForSchedule(schedule);

            return scheduler.Generate(startDate, settings)
                .Select((date, index) =>
                {
                    var ordinal = index + lastEventOrdinal + 1;

                    var context = new ValueEvaluationContext
                    {
                        OccurredAt = date,
                        Ordinal = ordinal,
                        Schedule = schedule
                    };

                    var amount = amountEval(context);

                    return new PlanningEvent
                    {
                        Amount = amount,
                            CreatedAt = DateTimeOffset.Now,
                            Currency = schedule.Currency,
                            IsChangedManually = false,
                            OccurredAt = date,
                            Ordinal = ordinal,
                            ScheduleId = schedule.Id,
                            CostCenterId = schedule.CostCenterId
                    };
                });
        }
    }
}