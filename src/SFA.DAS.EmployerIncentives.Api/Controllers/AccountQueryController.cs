using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerIncentives.Api.Extensions;
using SFA.DAS.EmployerIncentives.Interfaces;

namespace SFA.DAS.EmployerIncentives.Api.Controllers
{
    [ApiController]
    public class AccountQueryController : ControllerBase
    {
        private readonly IEmployerIncentivesQueryPassThroughService _passThroughService;

        public AccountQueryController(IEmployerIncentivesQueryPassThroughService client)
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
    }
}