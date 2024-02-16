using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.RoatpCourseManagement.Application.UkrlpData;

namespace SFA.DAS.RoatpCourseManagement.Api.Controllers
{
    [ApiController]
    public class UkrlpDataController : ControllerBase
    {
        private readonly ILogger<UkrlpDataController> _logger;
        private readonly IMediator _mediator;

        public UkrlpDataController(IMediator mediator, ILogger<UkrlpDataController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        [Route("lookup/providers-address")]
        public async Task<IActionResult> GetProvidersData(UkrlpDataQuery query)
        {
            if (query.ProvidersUpdatedSince == null && !query.Ukprns.Any())
            {
                _logger.LogWarning("No parameter entered for providers address lookup");
                return BadRequest();
            }

            if (query.ProvidersUpdatedSince != null && query.Ukprns.Any())
            {
                _logger.LogWarning("Two parameters entered for providers address lookup");
                return BadRequest();
            }

            _logger.LogInformation("Request to retrieve course directory data received");

            var response = await _mediator.Send(query);

            if (response.Results == null || !response.Success)
            {
                _logger.LogWarning("No results returned from ukrlp data handler");
                return NotFound();
            }

            return Ok(response.Results);
        }
    }
}
