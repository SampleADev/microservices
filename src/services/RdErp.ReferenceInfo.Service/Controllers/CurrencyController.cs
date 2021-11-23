using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using RdErp.ReferenceInfo.Currency;

namespace RdErp.ReferenceInfo.Service
{
    [ApiController]
    [Authorize]
    [Route("/api/currency")]
    public class CurrencyController : Controller
    {
        private readonly IExchangeRateService exchangeRateService;

        public CurrencyController(
            IExchangeRateService exchangeRateService
        )
        {
            this.exchangeRateService = exchangeRateService
                ??
                throw new ArgumentNullException(nameof(exchangeRateService));
        }

        [HttpPost("refresh-exchange-rates")]
        public async Task<ActionResult> RefreshExchangeRate()
        {
            await exchangeRateService.RefreshExchangeRates(null);

            return Ok();
        }
    }
}