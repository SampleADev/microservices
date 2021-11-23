using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using FluentValidation;

using LinqToDB;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RdErp.Planning.Service.Controllers
{
    [ApiController]
    [Authorize]
    [Route("/api/planning-events")]
    public class PlanningEventController : Controller
    {
        private readonly IPlanningEventGenerationService eventGenerationService;
        private readonly IPlanManagementService planManagementService;
        private readonly IPlanningEventManagementService eventManagementService;

        public PlanningEventController(IPlanningEventGenerationService eventGenerationService,
            IPlanManagementService planManagementService,
            IPlanningEventManagementService eventManagementService)
        {
            this.eventGenerationService = eventGenerationService
                ??
                throw new ArgumentNullException(nameof(eventGenerationService));
            this.planManagementService = planManagementService
                ??
                throw new ArgumentNullException(nameof(planManagementService));
            this.eventManagementService = eventManagementService
                ??
                throw new ArgumentNullException(nameof(eventManagementService));
        }

        [HttpGet("{planId}")]
        public async Task<ActionResult> GetEvents([FromRoute] int planId)
        {
            var events = await planManagementService.GetGeneratedPlanEvents(planId);
            return Ok(events);
        }

        [HttpGet("at-month/{month}/{currency}")]
        public async Task<ActionResult> GetAllEventsAtMonth([FromRoute] DateTime month, [FromRoute] string currency)
        {
            var events = await eventManagementService.AllByMonth(month, currency);
            return Ok(events);
        }

        [HttpPost("generate-test-events")]
        public async Task<ActionResult> CreateTestEvents([FromBody] CreateTestEventModel input)
        {
            try
            {

                var events = await eventGenerationService.GenerateEvents(
                    input.Schedule, -1, input.StartDate
                );

                return Ok(
                    events
                    .TakeWhile(e => e.OccurredAt < (input.EndDate ?? input.StartDate.AddYears(1)))
                    .ToArray()
                );

            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToValidationResult().Errors);
            }
        }

    }

    public class CreateTestEventModel
    {
        public ScheduleDetails Schedule { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

    }
}