using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.RoatpCourseManagement.Application.NationalAchievementRatesLookup.Queries;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.Api.Controllers
{
    [ApiController]
    public class NationalAchievementRatesLookupController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<NationalAchievementRatesLookupController> _logger;

        public NationalAchievementRatesLookupController(IMediator mediator, ILogger<NationalAchievementRatesLookupController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Route("lookup/national-achievement-rates")]
        public async Task<IActionResult> GetNationalAcheivementRates()
        {
            _logger.LogInformation($"Outer API: Trying to get national acheivement rates");

            var result = await _mediator.Send(new NationalAchievementRatesLookupQuery());

            if (result == null) return BadRequest();

            return Ok(result);
        }
    }
}
