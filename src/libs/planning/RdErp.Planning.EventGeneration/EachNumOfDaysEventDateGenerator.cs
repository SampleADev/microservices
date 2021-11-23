using System;
using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json;

using static System.String;

namespace RdErp.Planning.EventGeneration
{
    public class EachNumOfDaysEventDateGenerator : AbstractEventDateGenerator<EachNumOfDaysEventDateGeneratorSettings>
        {
            public override string Name => "each-num-of-days";

            protected override IEnumerable<DateTime> GenerateCore(
                DateTime startDate,
                EachNumOfDaysEventDateGeneratorSettings settings)
            {
                var intervals = (settings.Intervals ?? new int[] { })
                    .OrderBy(i => i)
                    .Distinct()
                    .ToArray();

                var nextEventDate = startDate.Date;

                while (true)
                {
                    foreach (var interval in intervals)
                    {
                        nextEventDate = nextEventDate.AddDays(interval);

                        yield return nextEventDate;
                    }
                }
            }
        }

    public class EachNumOfDaysEventDateGeneratorSettings
    {
        /// <summary>
        /// Gets the intervals to sequentially add to previous event date.
        /// </summary>
        public int[] Intervals { get; set; }
    }
}