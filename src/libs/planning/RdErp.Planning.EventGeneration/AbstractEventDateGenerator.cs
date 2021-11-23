using System;
using System.Collections.Generic;

using Newtonsoft.Json;

using static System.String;

namespace RdErp.Planning.EventGeneration
{
    public abstract class AbstractEventDateGenerator<TSettings> : IEventDateGenerator
    {
        public abstract string Name { get; }

        public IEnumerable<DateTime> Generate(DateTime startDate, object settings)
        {
            switch (settings)
            {
                case TSettings s:
                    return GenerateCore(startDate, s);
                default:
                    throw new ArgumentException("Invalid settings type", nameof(settings));
            }
        }

        public object ParseSettings(string jsonSerializedSettings)
        {
            if (IsNullOrWhiteSpace(jsonSerializedSettings))
            {
                throw new ArgumentNullException(nameof(jsonSerializedSettings));
            }

            var settings = JsonConvert.DeserializeObject<TSettings>(
                jsonSerializedSettings
            );

            if (settings == null)
            {
                throw new ArgumentException("Invalid settings");
            }

            this.ValidateSettings(settings);

            return settings;
        }

        protected virtual void ValidateSettings(TSettings settings) { }

        protected abstract IEnumerable<DateTime> GenerateCore(DateTime startDate, TSettings settings);
    }
}