using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using FluentValidation;

using LinqToDB;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RdErp.Financial.Service.Controllers
{
    [ApiController]
    [Authorize]
    [Route("/api/trans")]
    public class FinancialTransactionController : Controller
    {
        private readonly IFinancialTransactionService financialTransactionService;

        public FinancialTransactionController(IFinancialTransactionService financialTransactionService)
        {
            this.financialTransactionService = financialTransactionService
                ??
                throw new ArgumentNullException(nameof(financialTransactionService));
        }

        [HttpGet]
        public async Task<ActionResult> All([FromQuery] ListRequest request)
        {
            return Ok(await financialTransactionService.All(request ?? new ListRequest()));
        }

        [HttpGet("at-month/{month}/{currency}")]
        public async Task<ActionResult> AtMonth([FromRoute] DateTime month, [FromRoute] string currency)
        {
            return Ok(await financialTransactionService.AtMonth(month, currency));
        }

        [HttpGet("{transactionId}")]
        public async Task<ActionResult> Get([FromRoute] int transactionId)
        {
            var transaction = await financialTransactionService.Get(transactionId);
            if (transaction != null)
            {
                return Ok(transaction);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] PostTransactionModel transaction)
        {
            if (transaction == null || transaction.Transaction == null || transaction.Attributes == null)
            {
                return BadRequest();
            }

            try
            {
                var id = await financialTransactionService.Register(
                    transaction.Transaction,
                    transaction.Attributes
                );

                var registeredTransaction = await financialTransactionService.Get(id);

                return Ok(registeredTransaction);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToValidationResult().Errors);
            }
        }
    }

    public class PostTransactionModel
    {
        public FinancialTransaction Transaction { get; set; }

        public FinancialTransactionAttribute[] Attributes { get; set; }
    }
}