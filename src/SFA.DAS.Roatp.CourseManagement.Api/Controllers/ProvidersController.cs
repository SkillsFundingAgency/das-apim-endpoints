using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Roatp.CourseManagement.Application.Provider;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.Roatp.CourseManagement.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProvidersController : ControllerBase
    {
        private readonly ILogger<ProvidersController> _logger;
        private readonly IMediator _mediator;

        public ProvidersController(ILogger<ProvidersController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{ukprn}")]
        public async Task<IActionResult> GetProvider(int ukprn)
        {
            if (ukprn <= 0)
            {
                _logger.LogInformation("Invalid ukprn number {ukprn} ukprn number has to be a positive number", ukprn);
                return BadRequest();
            }

            _logger.LogInformation("Get provider for ukprn number {registrationNumber}", ukprn);
            try
            {
                var result = await _mediator.Send(new GetProviderQuery(ukprn));

                if (result.Provider == null)
                {
                    _logger.LogInformation("provider not found for ukprn number {ukprn}", ukprn);
                    return NotFound();
                }

                _logger.LogInformation("provider found for ukprn number {ukprn}", ukprn);
                return Ok(result.Provider);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred trying to retrieve provider for ukprn number {ukprn}");
                return BadRequest();
            }
        }
    }
}
