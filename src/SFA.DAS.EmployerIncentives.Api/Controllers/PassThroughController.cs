using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.EmployerIncentives.Models.PassThrough;

namespace SFA.DAS.EmployerIncentives.Api.Controllers
{
    [ApiController]
    public class PassThroughController : ControllerBase
    {
        private readonly IEmployerIncentivesPassThroughService _passThroughService;

        public PassThroughController(IEmployerIncentivesPassThroughService client)
        {
            _passThroughService = client;
        }

        [HttpPost]
        [Route("account/{accountId}/legalentities")]
        public async Task<ActionResult> AddLegalEntity(long accountId, LegalEntityRequest request)
        {
            var innerApiResponse = await _passThroughService.AddLegalEntity(accountId, request);

            return StatusCode((int) innerApiResponse.StatusCode, innerApiResponse.Content);
        }

        [HttpDelete("/accounts/{accountId}/legalentities/{accountLegalEntityId}")]
        public async Task<ActionResult> RemoveLegalEntity(long accountId, long accountLegalEntityId)
        {
            var innerApiResponse = await _passThroughService.RemoveLegalEntity(accountId, accountLegalEntityId);

            return StatusCode((int)innerApiResponse.StatusCode, innerApiResponse.Content);
        }
    }
}
