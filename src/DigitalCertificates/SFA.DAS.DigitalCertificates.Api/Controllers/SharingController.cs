using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateSharing;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SFA.DAS.DigitalCertificates.Api.Controllers
{
    [ApiController]
    [Route("sharing/")]
    public class SharingController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<SharingController> _logger;

        public SharingController(IMediator mediator, ILogger<SharingController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSharing([FromBody] CreateSharingCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error attempting to create sharing for user {UserId}, certificate {CertificateId}", command.UserId, command.CertificateId);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
