using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using FluentValidation;

using LinqToDB;

using Mapster;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RdErp.Planning.Service.Controllers
{
    [ApiController]
    [Authorize]
    [Route("/api/plan")]
    public class PlanController : Controller
    {
        private readonly IPlanManagementService planManagementService;

        public PlanController(IPlanManagementService planManagementService)
        {
            this.planManagementService = planManagementService
                ??
                throw new ArgumentNullException(nameof(planManagementService));
        }

        [HttpGet]
        public async Task<ActionResult> All([FromQuery] ListRequest request) =>
            Ok(await planManagementService.All(request ?? new ListRequest()));

        [HttpGet("{id}")]
        public async Task<ActionResult> Get([FromRoute] int id)
        {
            var(plan, schedules) = await planManagementService.GetPlan(id);
            if (plan == null)
            {
                return NotFound();
            }

            return Ok(new PlanData
            {
                Plan = plan,
                    Schedules = schedules
            });
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] PlanData request)
        {
            try
            {
                var(plan, schedule) = await planManagementService.Save(request.Plan, request.Schedules);

                return Ok(new PlanData
                {
                    Plan = plan,
                        Schedules = schedule
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToValidationResult().Errors);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            await planManagementService.DeletePlan(id);
            return NoContent();
        }

        [HttpPost("regenerate-events")]
        public async Task<ActionResult> RegenerateEventsForAllPlans()
        {
            await planManagementService.RegeneratePlanningEvents();
            return NoContent();
        }

        [HttpPost("{id}/regenerate-events")]
        public async Task<ActionResult> RegenerateEventsForAllPlans([FromRoute] int id)
        {
            await planManagementService.RegeneratePlanningEvents(id);
            return NoContent();
        }
    }

    public class PlanData
    {
        public Plan Plan { get; set; }

        public ScheduleDetails[] Schedules { get; set; }
    }
}