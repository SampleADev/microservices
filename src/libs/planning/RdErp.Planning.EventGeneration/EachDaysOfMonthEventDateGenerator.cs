using System;
using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json;

using static System.String;

namespace RdErp.Planning.EventGeneration
{
    public class EachDaysOfMonthEventDateGenerator : AbstractEventDateGenerator<EachDaysOfMonthEventDateGeneratorSettings>
        {
            public override string Name => "each-days-of-month";

            protected override IEnumerable<DateTime> GenerateCore(
                DateTime startDate,
                EachDaysOfMonthEventDateGeneratorSettings settings)
            {
                var days = (settings.Days ?? new int[] { })
                    .OrderBy(i => i)
                    .Distinct()
                    .ToArray();

                var firstMonth = new DateTime(
                    startDate.Year,
                    startDate.Month,
                    1,
                    startDate.Hour,
                    startDate.Minute,
                    startDate.Second
                ).Date;

                while (true)
                {
                    foreach (var day in days)
                        if (DateTime.DaysInMonth(firstMonth.Year, firstMonth.Month) >= day)
                        {
                            var date = new DateTime(
                                firstMonth.Year,
                                firstMonth.Month,
                                day,
                                firstMonth.Hour,
                                firstMonth.Minute,
                                firstMonth.Second
                            );

                            if (date >= startDate)
                            {
                                yield return date;
                            }
                        }

                    firstMonth = firstMonth.AddMonths(1);
                }
            }
        }

    public class EachDaysOfMonthEventDateGeneratorSettings
    {
        public int[] Days { get; set; }
    }
}