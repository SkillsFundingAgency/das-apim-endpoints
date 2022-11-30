using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;

namespace SFA.DAS.Approvals.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class TestController : Controller
    {
        
        [HttpGet]
        public IActionResult Index()
        {
            var result = string.Empty;
            var claims = this.User.Identity as ClaimsIdentity;

            result = this.User.Identity.AuthenticationType + Environment.NewLine;
            foreach (var claim in claims.Claims)
            {
                result += $"Type:{claim.Type} - value:{claim.Value} - subject:{claim.Subject} -Issuer:{claim.Issuer} - originalIs:{claim.OriginalIssuer}" + Environment.NewLine;
            }

            return Ok(result);
        }

        [HttpGet("auth")]
        [Authorize]
        public IActionResult Auth()
        {
            return Ok(this.User.Identity.IsAuthenticated);
        }
    }
}
