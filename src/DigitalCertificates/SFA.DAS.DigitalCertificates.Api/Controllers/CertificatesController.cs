using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.DigitalCertificates.Application.Queries.GetFrameworkCertificate;
using SFA.DAS.DigitalCertificates.Application.Queries.GetStandardCertificate;

namespace SFA.DAS.DigitalCertificates.Api.Controllers
{
    [ApiController]
    [Route("certificates/")]
    public class CertificatesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CertificatesController> _logger;

        public CertificatesController(IMediator mediator, ILogger<CertificatesController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStandardCertificate([FromRoute] Guid id)
        {
            try
            {
                var result = await _mediator.Send(new GetStandardCertificateQuery(id));

                return result == null ? NotFound() : Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error attempting to retrieve certificate {CertificateId}", id);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("{id}/framework")]
        public async Task<IActionResult> GetFrameworkCertificate([FromRoute] Guid id)
        {
            try
            {
                var result = await _mediator.Send(new GetFrameworkCertificateQuery(id));

                return result == null ? NotFound() : Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error attempting to retrieve framework certificate {FrameworkLearnerId}", id);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
