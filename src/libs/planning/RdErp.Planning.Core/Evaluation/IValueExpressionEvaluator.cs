using System;

namespace RdErp.Planning
{
    public interface IValueExpressionEvaluatorFactory
    {
        Func<IValueEvaluationContext, decimal> CreateForSchedule(ScheduleDetails schedule);
    }

    public interface IValueEvaluationContext
    {
        int Ordinal { get; }

        DateTimeOffset OccurredAt { get; }
    }

    public class ValueEvaluationContext : IValueEvaluationContext
    {
        public int Ordinal { get; set; }

        public DateTimeOffset OccurredAt { get; set; }

        public ScheduleDetails Schedule { get; set; }
    }
}