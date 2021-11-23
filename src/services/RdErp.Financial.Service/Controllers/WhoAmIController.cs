using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using RdErp.Financial;

namespace RdErp.Financial.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WhoAmIController : ControllerBase
    {
        public WhoAmIController(IFinancialTransactionService financialTransactionService)
        {
            if (financialTransactionService == null)
            {
                throw new ArgumentNullException(nameof(financialTransactionService));
            }
        }

        [HttpGet]
        public ActionResult<string> Get()
        {
            return "Financial Service v0.0.1";
        }
    }
}