using System;
using System.Collections.Generic;

namespace RdErp.Planning.EventGeneration
{
    /// <summary>
    /// Generates a sequence of dates
    /// </summary>
    public interface IEventDateGenerator
    {
        string Name { get; }

        IEnumerable<DateTime> Generate(
            DateTime startDate,
            object settings
        );

        object ParseSettings(string jsonSerializedSettings);
    }
}