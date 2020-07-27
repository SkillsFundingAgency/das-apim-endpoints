using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerIncentives.Api.Extensions;
using SFA.DAS.EmployerIncentives.Models.PassThrough;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerIncentives.Api.Controllers
{
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly IPassThroughApiClient _client;

        public JobController(IPassThroughApiClient client)
        {
            _client = client;
        }

        [HttpPut]
        [Route("/jobs")]
        public async Task<IActionResult> AddJob([FromBody] JobRequest request)
        {
            var innerApiResponse = await _client.Put($"/jobs", request);

            return this.CreateObjectResult(innerApiResponse);
        }
    }
}
