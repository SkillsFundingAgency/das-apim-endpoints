using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Aodp.Api.Controllers;
using SFA.DAS.Aodp.Application.Queries.Qualifications;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ReferenceData;

namespace SFA.DAS.AODP.Api.Controllers.Qualification
{
    [ApiController]
    [Route("api/qualifications")]
    public class QualificationsController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly ILogger<QualificationsController> _logger;

        public QualificationsController(IMediator mediator, ILogger<QualificationsController> logger) : base(mediator, logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(BaseMediatrResponse<GetNewQualificationsQueryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetQualifications([FromQuery] string? status,
            [FromQuery] int? skip,
            [FromQuery] int? take,
            [FromQuery] string? name,
            [FromQuery] string? organisation,
            [FromQuery] string? qan)
        {
            var validationResult = ValidateQualificationParams(status, skip, take, name, organisation, qan);

            if (validationResult.IsValid)
            {
                if (validationResult.ProcessedStatus == "new")
                {
                    var query = new GetNewQualificationsQuery()
                    {
                        Name = name,
                        Organisation = organisation,
                        QAN = qan,
                        Skip = skip,
                        Take = take
                    };
                    return await SendRequestAsync(query);
                }
                else if (validationResult.ProcessedStatus == "changed")
                {
                    var query = new GetChangedQualificationsQuery();
                    return await SendRequestAsync(query);
                }
                else
                {
                    return BadRequest(new { message = $"Invalid status: {validationResult.ProcessedStatus}" });
                }
            }
            else
            {
                return BadRequest(new { message = validationResult.ErrorMessage });
            }           
         
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
            IActionResult response = await HandleNewQualificationCSVExport();          

            return response;
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

        private ParamValidationResult ValidateQualificationParams(string? status, int? skip, int? take, string? name, string? organisation, string? qan)
        {
            var result = new ParamValidationResult() { IsValid = true };
            status = status?.Trim().ToLower();

            if (string.IsNullOrEmpty(status))
            {                
                result.IsValid = false;
                result.ErrorMessage = "Qualification status cannot be empty.";                
            }
            else
            {
                result.ProcessedStatus = status;
            }

            if (skip < 0)
            {                
                result.IsValid = false;
                result.ErrorMessage = "Skip param is invalid.";
            }

            if (take < 0)
            {
                result.IsValid = false;
                result.ErrorMessage = "Take param is invalid.";
            }

            if (!result.IsValid)
            {
                _logger.LogWarning(result.ErrorMessage);
            }

            return result;
        }

        private class ParamValidationResult
        {
            public bool IsValid { get; set; }
            public string? ErrorMessage { get; set; }
            public string? ProcessedStatus { get; set; }
        }
    }
}


