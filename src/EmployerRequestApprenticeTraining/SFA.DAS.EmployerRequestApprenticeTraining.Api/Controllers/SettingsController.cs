using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetSettings;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Api.Controllers
{
    [ApiController]
    [Route("settings/")]
    public class SettingsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<SettingsController> _logger;

        public SettingsController(IMediator mediator, ILogger<SettingsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await _mediator.Send(new GetSettingsQuery());

                if (result != null)
                {
                    return Ok(result);
                }

                return NotFound();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to retrieve settings");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
