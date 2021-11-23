using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using RdErp.ReferenceInfo.Currency;
using RdErp.ReferenceInfo.Financial;

namespace RdErp.ReferenceInfo.Service
{
    [ApiController]
    [Authorize]
    [Route("/api/lists")]
    public class RefListController : Controller
    {
        private readonly ICurrencyListService currencyListService;
        private readonly IFinancialRefService financialListService;

        public RefListController(ICurrencyListService currencyListService,
            IFinancialRefService financialListService)
        {
            this.currencyListService = currencyListService
                ??
                throw new ArgumentNullException(nameof(currencyListService));
            this.financialListService = financialListService
                ??
                throw new ArgumentNullException(nameof(financialListService));
        }

        [HttpGet("currencies")]
        public async Task<ActionResult> Currencies()
        {
            var list = await currencyListService.GetCurrencyCodes();
            return Ok(list);
        }

        [HttpGet("cost-centers")]
        public async Task<ActionResult> CostCenters() => Ok(await financialListService.GetCostCenters());

        [HttpGet("transaction-codes")]
        public async Task<ActionResult> TransactionCodes() => Ok(await financialListService.GetTransactionCodes());

    }
}