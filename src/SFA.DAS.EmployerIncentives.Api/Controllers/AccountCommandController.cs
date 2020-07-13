using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.EmployerIncentives.Models.PassThrough;

namespace SFA.DAS.EmployerIncentives.Api.Controllers
{
    [ApiController]
    public class AccountCommandController : ControllerBase
    {
        private readonly IEmployerIncentivesCommandPassThroughService _passThroughService;

        public AccountCommandController(IEmployerIncentivesCommandPassThroughService client)
        {
            _passThroughService = client;
        }

        [HttpPost]
        [Route("/accounts/{accountId}/legalentities")]
        public async Task<IActionResult> AddLegalEntity(long accountId, LegalEntityRequest request)
        {
            var innerApiResponse = await _passThroughService.PostAsync($"/accounts/{accountId}/legalentities", request);

            return StatusCode((int)innerApiResponse.StatusCode, innerApiResponse.Json?.RootElement);
        }

        [HttpDelete("/accounts/{accountId}/legalentities/{accountLegalEntityId}")]
        public async Task<IActionResult> RemoveLegalEntity(long accountId, long accountLegalEntityId)
        {
            var innerApiResponse = await _passThroughService.DeleteAsync($"/accounts/{accountId}/legalentities/{accountLegalEntityId}");

            return StatusCode((int)innerApiResponse.StatusCode, innerApiResponse.Json);
        }
    }
}
