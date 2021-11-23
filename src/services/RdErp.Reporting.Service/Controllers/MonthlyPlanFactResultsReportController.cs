using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using FluentValidation;

using LinqToDB;

using Mapster;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using RdErp.Reporting.FinancialResults;

namespace RdErp.Reporting.Service.Controllers
{
    [ApiController]
    [Authorize]
    [Route("/api/monthly-plan-fact-results")]
    public class MonthlyPlanFactResultsReportController : Controller
    {
        private readonly IMonthlyPlanFactResultsReportService service;

        public MonthlyPlanFactResultsReportController(IMonthlyPlanFactResultsReportService service) => this.service = service
            ??
            throw new ArgumentNullException(nameof(service));

        [HttpGet("{currency?}")]
        public async Task<ActionResult> MonthlyPlanFactResultsReport([FromRoute] DateTime planFactSplitDate, [FromRoute] string currency)
        {
            if (String.IsNullOrWhiteSpace(currency))
            {
                currency = "UAH";
            }

            return Ok(await service.MonthlyPlanFactResultsReport(currency));
        }
    }

}