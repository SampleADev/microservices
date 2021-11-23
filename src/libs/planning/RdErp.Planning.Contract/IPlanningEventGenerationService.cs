using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RdErp.Planning
{
    public interface IPlanningEventGenerationService
    {
        Task<IEnumerable<PlanningEvent>> GenerateEvents(
            ScheduleDetails schedule,
            int lastEventOrdinal,
            DateTime startDate
        );
    }
}