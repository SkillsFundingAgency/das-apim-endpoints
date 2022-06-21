using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Roatp.Application.Charities.Queries;

namespace SFA.DAS.Roatp.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CharitiesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CharitiesController> _logger;

        public CharitiesController(IMediator mediator, ILogger<CharitiesController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Route("{registrationNumber}")]
        public async Task<IActionResult> GetCharity(int registrationNumber)
        {
            if (registrationNumber <= 0)
            {
                _logger.LogInformation("Invalid registration number {registrationNumber} Registration number has to be a positive number", registrationNumber);
                return BadRequest();
            }

            var result = await _mediator.Send(new GetCharityQuery(registrationNumber));

            if (result.Charity == null)
            {
                _logger.LogInformation("Charity not found for registration number {registrationNumber}", registrationNumber);
                return NotFound();
            }

            _logger.LogInformation("Charity found for registration number {registrationNumber} with name {charityName}", registrationNumber, result.Charity.Name);
            return Ok(result.Charity);
        }
    }
}