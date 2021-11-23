using System;
using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json;

using static System.String;

namespace RdErp.Planning.EventGeneration
{
    public class EachDaysOfWeekEventDateGenerator : AbstractEventDateGenerator<EachDaysOfWeekEventDateGeneratorSettings>
        {
            public override string Name => "each-days-of-week";

            protected override IEnumerable<DateTime> GenerateCore(
                DateTime startDate,
                EachDaysOfWeekEventDateGeneratorSettings settings)
            {
                var daysOfWeek = new HashSet<DayOfWeek>(settings.WeekDays ?? Enumerable.Empty<DayOfWeek>());

                var nextEventDate = startDate.StartOfWeek(settings.StartOfWeek).Date;

                while (true)
                {
                    for (var i = 0; i < 7; i++)
                    {
                        var eventDate = nextEventDate.AddDays(i);

                        if (daysOfWeek.Contains(eventDate.DayOfWeek)
                            && eventDate >= startDate)
                        {

                            yield return eventDate;
                        }
                    }

                    nextEventDate = nextEventDate.AddDays(7 * (int) settings.WeekFrequency);
                }
            }
        }

    public enum WeekFrequency
    {
        EachWeek = 1,
        EachTwoWeeks = 2,
        EachThreeWeeks = 3
    }

    public class EachDaysOfWeekEventDateGeneratorSettings
    {
        /// <summary>
        /// Gets the days of week to generate event at.
        /// </summary>
        public DayOfWeek[] WeekDays { get; set; }

        public WeekFrequency WeekFrequency { get; set; }

        public DayOfWeek StartOfWeek { get; set; } = DayOfWeek.Monday;
    }
}