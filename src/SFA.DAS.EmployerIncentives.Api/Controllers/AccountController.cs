using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerIncentives.Api.Extensions;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.EmployerIncentives.Models.PassThrough;

namespace SFA.DAS.EmployerIncentives.Api.Controllers
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IApiPassThroughService _passThroughService;

        public AccountController(IApiPassThroughService client)
        {
            _passThroughService = client;
        }

        [HttpGet]
        [Route("/accounts/{accountId}/legalentities")]
        public async Task<IActionResult> GetLegalEntities(long accountId)
        {
            var innerApiResponse = await _passThroughService.GetAsync($"/accounts/{accountId}/legalentities");

            return this.CreateObjectResult(innerApiResponse);
        }

        [HttpPost]
        [Route("/accounts/{accountId}/legalentities")]
        public async Task<IActionResult> AddLegalEntity(long accountId, LegalEntityRequest request)
        {
            var innerApiResponse = await _passThroughService.PostAsync($"/accounts/{accountId}/legalentities", request);

            return this.CreateObjectResult(innerApiResponse);
        }

        [HttpDelete("/accounts/{accountId}/legalentities/{accountLegalEntityId}")]
        public async Task<IActionResult> RemoveLegalEntity(long accountId, long accountLegalEntityId)
        {
            var innerApiResponse = await _passThroughService.DeleteAsync($"/accounts/{accountId}/legalentities/{accountLegalEntityId}");

            return this.CreateObjectResult(innerApiResponse);
        }
    }
}
