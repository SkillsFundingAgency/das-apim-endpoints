using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.Extensions;

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

        [HttpGet("Call/Commitment")]
        public async Task<IActionResult> CallCommitmment([FromServices] ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> v2ApiClient)
        {
            var result = await v2ApiClient.GetWithResponseCode<ResponseRole>(new CallTestCommitmentApi());

            if (ApiResponseErrorChecking.IsSuccessStatusCode(result.StatusCode))
            {
                return Ok(result.Body);
            }

            return BadRequest(result);
        }
    }

    public class ResponseRole
    {
        public string Role { get; set; }
    }

    public class CallTestCommitmentApi : IGetApiRequest
    {
        public string GetUrl => $"api/Test/role";
    }

}
