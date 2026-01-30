using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateSharing;
using SFA.DAS.DigitalCertificates.Application.Queries.GetSharingById;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateSharingEmail;
using SFA.DAS.DigitalCertificates.Application.Commands.DeleteSharing;
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

        [HttpGet("{sharingId}")]
        public async Task<IActionResult> GetSharingById([FromRoute] Guid sharingId, [FromQuery] int? limit = null)
        {
            try
            {
                var result = await _mediator.Send(new GetSharingByIdQuery { SharingId = sharingId, Limit = limit });
                return Ok(result.Response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error attempting to retrieve sharing {SharingId}", sharingId);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost("{sharingId}/email")]
        public async Task<IActionResult> CreateSharingEmail([FromRoute] Guid sharingId, [FromBody] CreateSharingEmailCommand command)
        {
            try
            {
                command.SharingId = sharingId;
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error attempting to create sharing email for sharing {SharingId}", sharingId);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpDelete("{sharingId}")]
        public async Task<IActionResult> DeleteSharing([FromRoute] Guid sharingId)
        {
            try
            {
                await _mediator.Send(new DeleteSharingCommand { SharingId = sharingId });
                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error attempting to delete sharing {SharingId}", sharingId);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
