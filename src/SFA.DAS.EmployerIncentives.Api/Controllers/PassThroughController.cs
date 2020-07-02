using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace SFA.DAS.EmployerIncentives.Api.Controllers
{
    [ApiController]
    public class PassThroughController : ControllerBase
    {
        private readonly ILogger<PassThroughController> _logger;

        public PassThroughController(ILogger<PassThroughController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("account/{accountId}/legal-entities")]
        public ActionResult<IEnumerable<string>> Get(long accountId)
        {
            return new string[] { $"value1 - for {accountId}", $"value2 - for {accountId}" };
        }
    }
}
