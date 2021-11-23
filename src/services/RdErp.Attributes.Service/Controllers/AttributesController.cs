using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using FluentValidation;

using LinqToDB;

using Mapster;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RdErp.Attributes.Service.Controllers
{
    [ApiController]
    [Authorize]
    [Route("/api/attributes")]
    public class AttributesController : Controller
    {
        private readonly ICustomAttributeService customAttributeService;

        public AttributesController(ICustomAttributeService customAttributeService)
        {
            this.customAttributeService = customAttributeService
                ??
                throw new ArgumentNullException(nameof(customAttributeService));
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var attributes = await customAttributeService.GetAttributeValueList(null);
            return Ok(attributes);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CustomAttributeValue[] attributes)
        {
            var result = await customAttributeService.GetAttributeValueList(attributes);
            return Ok(result);
        }
    }

}