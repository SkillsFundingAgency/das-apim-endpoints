using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.DigitalCertificates.Application.Queries.GetSharings;
using System;
using System.Diagnostics;
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

        public async Task<IActionResult> GetCertificateSharingDetails([FromQuery] Guid user, [FromQuery] Guid certificateId, [FromQuery] int limit = 10)
        {
            try
            {
                var result = await _mediator.Send(new GetCertificateSharingDetailsQuery { UserId = user, CertificateId = certificateId, Limit = limit });
                if (result != null)
                {
                    return Ok(result.Response);
                }

                throw new UnreachableException($"GetCertificateSharingDetailsQueryHandler returned null for user: {user}, certificateId: {certificateId}");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error attempting to retrieve sharings {UserId}", user);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
