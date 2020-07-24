using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerIncentives.Api.Extensions;
using SFA.DAS.EmployerIncentives.Models.PassThrough;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerIncentives.Api.Controllers
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IPassThroughApiClient _client;

        public AccountController(IPassThroughApiClient client)
        {
            _client = client;
        }

        [HttpGet]
        [Route("/accounts/{accountId}/legalentities")]
        public async Task<IActionResult> GetLegalEntities(long accountId)
        {
            var innerApiResponse = await _client.Get($"/accounts/{accountId}/legalentities");

            return this.CreateObjectResult(innerApiResponse);
        }

        [HttpPost]
        [Route("/accounts/{accountId}/legalentities")]
        public async Task<IActionResult> AddLegalEntity(long accountId, LegalEntityRequest request)
        {
            var innerApiResponse = await _client.Post($"/accounts/{accountId}/legalentities", request);

            return this.CreateObjectResult(innerApiResponse);
        }

        [HttpDelete("/accounts/{accountId}/legalentities/{accountLegalEntityId}")]
        public async Task<IActionResult> RemoveLegalEntity(long accountId, long accountLegalEntityId)
        {
            var innerApiResponse = await _client.Delete($"/accounts/{accountId}/legalentities/{accountLegalEntityId}");

            return this.CreateObjectResult(innerApiResponse);
        }
    }
}
