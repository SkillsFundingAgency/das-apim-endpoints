using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateSharingAccess;

namespace SFA.DAS.DigitalCertificates.Api.Controllers
{
    [ApiController]
    [Route("sharingaccess/")]
    public class SharingAccessController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<SharingAccessController> _logger;

        public SharingAccessController(IMediator mediator, ILogger<SharingAccessController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSharingAccess([FromBody] CreateSharingAccessCommand command)
        {
            try
            {
                await _mediator.Send(command);

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error attempting to log sharing access for {SharingId}", command?.SharingId);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }

}
