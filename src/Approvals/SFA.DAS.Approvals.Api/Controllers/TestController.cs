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
                result += $"Issuer:{claim.Issuer} - value:{claim.Value} - subject:{claim.Subject} - originalIs:{claim.OriginalIssuer}" + Environment.NewLine;
            }

            return Ok(this.User.Identity);
        }

        [HttpGet("auth")]
        [Authorize]
        public IActionResult Auth()
        {
            return Ok(this.User.Identity.IsAuthenticated);
        }
    }
}
