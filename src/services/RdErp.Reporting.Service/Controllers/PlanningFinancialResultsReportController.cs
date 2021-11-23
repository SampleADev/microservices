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
    [Route("/api/planning-financial-results")]
    public class PlanningFinancialResultsReportController : Controller
    {
        private readonly IPlanningFinancialResultsReportService service;

        public PlanningFinancialResultsReportController(IPlanningFinancialResultsReportService service)
        {
            this.service = service
                ??
                throw new ArgumentNullException(nameof(service));
        }

        [HttpGet("{planFactSplitDate}/{currency?}")]
        public async Task<ActionResult> PlanningFinancialResultsReport([FromRoute] DateTime planFactSplitDate, [FromRoute] string currency)
        {
            if (String.IsNullOrWhiteSpace(currency))
            {
                currency = "UAH";
            }

            return Ok(await service.PlanningFinancialResultsReport(currency, planFactSplitDate));
        }
    }

}