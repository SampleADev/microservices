using System;
using System.Dynamic;

using DynamicExpresso;

namespace RdErp.Planning
{
    public class DynamicExpressoEvaluatorFactory : IValueExpressionEvaluatorFactory
    {
        public Func<IValueEvaluationContext, decimal> CreateForSchedule(ScheduleDetails schedule)
        {
            var interpreter = new Interpreter(InterpreterOptions.Default);

            interpreter.SetVariable("schedule", schedule, typeof(Schedule));

            return interpreter.ParseAs<Func<IValueEvaluationContext, decimal>>(
                    schedule.ValueExpression, "context"
                )
                .Compile<Func<IValueEvaluationContext, decimal>>();
        }

    }
}