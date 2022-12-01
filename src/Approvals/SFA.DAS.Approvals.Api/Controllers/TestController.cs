using Microsoft.AspNetCore.Mvc;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.Extensions;

namespace SFA.DAS.Approvals.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class TestController : Controller
    {

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
