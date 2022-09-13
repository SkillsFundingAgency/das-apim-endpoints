using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.RoatpCourseManagement.Application.AddressLookup.Queries;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.Api.Controllers
{
    [ApiController]
    public class AddressLookupController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AddressLookupController> _logger;

        public AddressLookupController(IMediator mediator, ILogger<AddressLookupController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Route("lookup/addresses")]
        public async Task<IActionResult> GetAddresses([FromQuery] string postcode)
        {
            _logger.LogInformation($"Outer API: Trying to get locations for postcode: {postcode}");

            var result = await _mediator.Send(new AddresssLookupQuery(postcode));

            if (result == null) return BadRequest();

            return Ok(result);
        }
    }
}
