using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateSharingEmailAccess;

namespace SFA.DAS.DigitalCertificates.Api.Controllers
{
    [ApiController]
    [Route("sharingemailaccess/")]
    public class SharingEmailAccessController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<SharingEmailAccessController> _logger;

        public SharingEmailAccessController(IMediator mediator, ILogger<SharingEmailAccessController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSharingEmailAccess([FromBody] CreateSharingEmailAccessCommand command)
        {
            try
            {
                await _mediator.Send(command);

                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error attempting to create sharing email access for {SharingEmailId}", command?.SharingEmailId);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
