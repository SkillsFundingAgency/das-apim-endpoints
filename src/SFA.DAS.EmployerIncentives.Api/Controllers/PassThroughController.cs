using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.EmployerIncentives.Models.PassThrough;

namespace SFA.DAS.EmployerIncentives.Api.Controllers
{
    [ApiController]
    public class PassThroughController : ControllerBase
    {
        private readonly ILogger<PassThroughController> _logger;
        private readonly IEmployerIncentivesPassThroughService _client;

        public PassThroughController(ILogger<PassThroughController> logger, IEmployerIncentivesPassThroughService client)
        {
            _logger = logger;
            _client = client;
        }

        [HttpPost]
        [Route("account/{accountId}/legalentities")]
        public async Task<ActionResult> Get(long accountId, LegalEntityRequest request)
        {
            var innerApiResponse = await _client.AddLegalEntity(accountId, request);

            return StatusCode((int) innerApiResponse.StatusCode, innerApiResponse.Content);
        }

        [HttpDelete("/accounts/{accountId}/legalentities/{accountLegalEntityId}")]
        public async Task<ActionResult> RemoveLegalEntity(long accountId, long accountLegalEntityId)
        {
            var innerApiResponse = await _client.RemoveLegalEntity(accountId, accountLegalEntityId);

            return StatusCode((int)innerApiResponse.StatusCode, innerApiResponse.Content);
        }
    }
}
