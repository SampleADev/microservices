using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

namespace RdErp.Project.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WhoAmIController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Get()
        {
            return "Project Service v0.0.1";
        }
    }
}