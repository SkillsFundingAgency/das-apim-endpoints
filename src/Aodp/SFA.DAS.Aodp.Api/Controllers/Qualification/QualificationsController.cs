using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Aodp.Application.Queries.Qualifications;

namespace SFA.DAS.AODP.Api.Controllers.Qualification
{
    [ApiController]
    [Route("api/qualifications")]
    public class QualificationsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<QualificationsController> _logger;

        public QualificationsController(IMediator mediator, ILogger<QualificationsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetQualifications([FromQuery] string status)
        {
            status = status?.Trim().ToLower();

            if (string.IsNullOrEmpty(status))
            {
                _logger.LogWarning("Qualification status is missing.");
                return BadRequest(new { message = "Qualification status cannot be empty." });
            }

            IActionResult response = status switch
            {
                "new" => await HandleNewQualifications(),
                //Add more cases for other statuses
                _ => BadRequest(new { message = $"Invalid status: {status}" })
            };

            return response;
        }

        private async Task<IActionResult> HandleNewQualifications()
        {
            var result = await _mediator.Send(new GetNewQualificationsQuery());

            if (result == null || !result.Success || result.Value == null)
            {
                _logger.LogWarning("No new qualifications found.");
                return NotFound(new { message = "No new qualifications found" });
            }

            return Ok(result);
        }

        [HttpGet("{qualificationReference}")]
        public async Task<IActionResult> GetQualificationDetails(string qualificationReference)
        {
            if (string.IsNullOrWhiteSpace(qualificationReference))
            {
                _logger.LogWarning("Qualification reference is empty");
                return BadRequest(new { message = "Qualification reference cannot be empty" });
            }

            var result = await _mediator.Send(new GetQualificationDetailsQuery { QualificationReference = qualificationReference });

            if (!result.Success || result.Value == null)
            {
                _logger.LogWarning(result.ErrorMessage);
                return NotFound(new { message = $"No details found for qualification reference: {qualificationReference}" });
            }

            return Ok(result);
        }
    }
}
