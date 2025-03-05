using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Aodp.Api.Controllers;
using SFA.DAS.Aodp.Application.Queries.Qualifications;

namespace SFA.DAS.AODP.Api.Controllers.Qualification
{
    [ApiController]
    [Route("api/qualifications")]
    public class QualificationsController : BaseController
    {
        public QualificationsController(IMediator mediator, ILogger<QualificationsController> logger) : base(mediator, logger)
        {
        }

        [HttpGet]
        [ProducesResponseType(typeof(BaseMediatrResponse<GetNewQualificationsQueryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetQualifications([FromQuery] string? status)
        {
            var validationResult = ProcessAndValidateStatus(status);
            if (!validationResult.IsValid)
            {
                return BadRequest(new { message = validationResult.ErrorMessage });
            }

            IActionResult response = validationResult.ProcessedStatus switch
            {
                "new" => await HandleNewQualifications(),
                "changed" => await HandleChangedQualifications(),
                _ => BadRequest(new { message = $"Invalid status: {validationResult.ProcessedStatus}" })
            };

            return response;
        }

        [HttpGet("{qualificationReference}")]
        [ProducesResponseType(typeof(BaseMediatrResponse<GetQualificationDetailsQueryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetQualificationDetails(string? qualificationReference)
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

        [HttpGet("export")]
        [ProducesResponseType(typeof(BaseMediatrResponse<GetNewQualificationsCsvExportResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetQualificationCSVExportData([FromQuery] string? status)
        {
            var validationResult = ProcessAndValidateStatus(status);
            if (!validationResult.IsValid)
            {
                return BadRequest(new { message = validationResult.ErrorMessage });
            }

            IActionResult response = validationResult.ProcessedStatus switch
            {
                "new" => await HandleNewQualificationCSVExport(),
                _ => BadRequest(new { message = $"Invalid status: {validationResult.ProcessedStatus}" })
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

        private async Task<IActionResult> HandleChangedQualifications()
        {
            var query = new GetChangedQualificationsQuery();
            return await SendRequestAsync(query);
        }

        private async Task<IActionResult> HandleNewQualificationCSVExport()
        {
            var result = await _mediator.Send(new GetNewQualificationsCsvExportQuery());

            if (result == null || !result.Success || result.Value == null)
            {
                _logger.LogWarning(result.ErrorMessage);
                return NotFound(new { message = result.ErrorMessage });
            }

            return Ok(result);
        }

        private StatusValidationResult ProcessAndValidateStatus(string? status)
        {
            status = status?.Trim().ToLower();

            if (string.IsNullOrEmpty(status))
            {
                _logger.LogWarning("Qualification status is missing.");
                return new StatusValidationResult
                {
                    IsValid = false,
                    ErrorMessage = "Qualification status cannot be empty."
                };
            }

            return new StatusValidationResult
            {
                IsValid = true,
                ProcessedStatus = status
            };
        }

        private class StatusValidationResult
        {
            public bool IsValid { get; set; }
            public string? ErrorMessage { get; set; }
            public string? ProcessedStatus { get; set; }
        }
    }
}


