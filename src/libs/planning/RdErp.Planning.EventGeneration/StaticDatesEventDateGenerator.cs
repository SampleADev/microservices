using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using Newtonsoft.Json;

using static System.String;

namespace RdErp.Planning.EventGeneration
{
    public class StaticDatesEventDateGenerator : AbstractEventDateGenerator<StaticDatesEventDateGeneratorSettings>
        {
            public override string Name => "static-dates";

            protected override IEnumerable<DateTime> GenerateCore(
                DateTime startDate,
                StaticDatesEventDateGeneratorSettings settings)
            {
                var intervals = (settings.Dates ?? new string[] { })
                    .Select(d => DateTime.ParseExact(d, "yyyy-MM-dd", CultureInfo.GetCultureInfo("en-US")))
                    .OrderBy(i => i)
                    .Distinct()
                    .ToArray();

                foreach(var date in intervals)
                {
                    yield return date.Date;
                }
            }
        }

    public class StaticDatesEventDateGeneratorSettings
    {
        public string[] Dates { get; set; }
    }
}