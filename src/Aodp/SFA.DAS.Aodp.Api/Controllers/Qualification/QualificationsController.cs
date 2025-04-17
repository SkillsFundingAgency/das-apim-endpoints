using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Aodp.Api.Controllers;
using SFA.DAS.AODP.Application.Commands.Qualification;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ReferenceData;
using SFA.DAS.Aodp.Application.Commands.Application.Qualifications;
using SFA.DAS.Aodp.Application.Commands.Application.Review;
using SFA.DAS.Aodp.Application.Commands.Qualification;
using SFA.DAS.Aodp.Application.Queries.Application.Review;
using SFA.DAS.Aodp.Application.Queries.Qualifications;
using SFA.DAS.Aodp.Application.Commands.Qualification;
using SFA.DAS.AODP.Application.Queries.Qualifications;

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
        [ProducesResponseType(typeof(BaseMediatrResponse<GetChangedQualificationsQueryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetQualifications(
            [FromQuery] string? status,
            [FromQuery] int? skip,
            [FromQuery] int? take,
            [FromQuery] string? name,
            [FromQuery] string? organisation,
            [FromQuery] string? qan,
            [FromQuery] string? processStatusFilter)
        {
            var validationResult = ValidateQualificationParams(status, skip, take, name, organisation, qan, processStatusFilter);

            if (validationResult.IsValid)
            {
                if (validationResult.ParsedStatus == "new")
                {
                    var query = new GetNewQualificationsQuery()
                    {
                        Name = name,
                        Organisation = organisation,
                        QAN = qan,
                        Skip = skip,
                        Take = take,
                        ProcessStatusFilter = processStatusFilter
                    };
                    return await SendRequestAsync(query);
                }
                else if (validationResult.ParsedStatus == "changed")
                {
                    var query = new GetChangedQualificationsQuery()
                    {
                        Name = name,
                        Organisation = organisation,
                        QAN = qan,
                        Skip = skip,
                        Take = take,
                        ProcessStatusFilter = processStatusFilter
                    };
                    return await SendRequestAsync(query);
                }
                else
                {
                    return BadRequest(new { message = $"Invalid status: {validationResult.ParsedStatus}" });
                }
            }
            else
            {
                return BadRequest(new { message = validationResult.ErrorMessage });
            }

        }

        [HttpGet("{qualificationReference}/detail")]
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

            return await SendRequestAsync(new GetQualificationDetailsQuery { QualificationReference = qualificationReference });
        }

        [HttpGet("{qualificationReference}/detailwithversions")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetQualificationDetailWithVersions(string? qualificationReference)
        {
            if (string.IsNullOrWhiteSpace(qualificationReference))
            {
                _logger.LogWarning("Qualification reference is empty");
                return BadRequest(new { message = "Qualification reference cannot be empty" });
            }
            return await SendRequestAsync(new GetQualificationDetailWithVersionsQuery { QualificationReference = qualificationReference });
        }

        [HttpGet("{qualificationReference}/qualificationversions/{version}")]
        [ProducesResponseType(typeof(BaseMediatrResponse<GetQualificationDetailsQueryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetQualificationDetails(string? qualificationReference, int? version)
        {
            if (string.IsNullOrWhiteSpace(qualificationReference))
            {
                _logger.LogWarning("Qualification reference is empty");
                return BadRequest(new { message = "No version specified" });
            }

            if (version is null | version == 0)
            {
                _logger.LogWarning("No version specified");
                return BadRequest(new { message = "No version specified" });
            }

            return await SendRequestAsync(new GetQualificationVersionQuery { QualificationReference = qualificationReference, Version = version });
        }

        [HttpPost("qualificationdiscussionhistory")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddQualificationDiscussionHistory([FromBody] AddQualificationDiscussionHistoryCommand qualificationDiscussionHistory)
        {
            return await SendRequestAsync(qualificationDiscussionHistory);
        }

        [HttpGet("processstatuses")]
        [ProducesResponseType(typeof(GetProcessStatusesQueryResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetProcessingStatuses()
        {
            return await SendRequestAsync(new GetProcessStatusesQuery());
        }

        [HttpGet("{qualificationReference}/qualificationdiscussionhistories")]
        [ProducesResponseType(typeof(BaseMediatrResponse<GetDiscussionHistoriesForQualificationQueryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetDiscussionHistoriesForQualification(string qualificationReference)
        {
            if (string.IsNullOrWhiteSpace(qualificationReference))
            {
                _logger.LogWarning("Qualification reference is empty");
                return BadRequest(new { message = "Qualification reference cannot be empty" });
            }
            return await SendRequestAsync(new GetDiscussionHistoriesForQualificationQuery { QualificationReference = qualificationReference });
        }

        [HttpPost("qualificationstatus")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateQualificationStatus([FromBody] UpdateQualificationStatusCommand qualificationStatus)
        {
            return await SendRequestAsync(qualificationStatus);
        }

        [HttpGet("export")]
        [ProducesResponseType(typeof(BaseMediatrResponse<GetQualificationsExportResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetQualificationExportData([FromQuery] string? status)
        {
            if (string.IsNullOrWhiteSpace(status))
            {
                _logger.LogWarning("Status parameter is empty");
                return BadRequest(new { message = "Status parameter cannot be empty" });
            }

            IActionResult response = status.ToLower() switch
            {
                "new" => await HandleNewQualificationCSVExport(),
                "changed" => await HandleChangedQualificationCSVExport(),
                _ => BadRequest(new { message = "Status parameter not a valid value" })
            };

            return response;
        }

        [HttpGet("/api/qualifications/{qualificationReference}/QualificationVersions")]
        [ProducesResponseType(typeof(BaseMediatrResponse<GetQualificationVersionsForQualificationByReferenceQueryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetQualificationVersionsForQualificationByReference(string qualificationReference)
        {
            return await SendRequestAsync(new GetQualificationVersionsForQualificationByReferenceQuery(qualificationReference));
        }

        [HttpGet("/api/qualifications/{qualificationVersionId}/feedback")]
        [ProducesResponseType(typeof(GetFeedbackForQualificationFundingByIdQueryResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetFeedbackForQualificationFundingById(Guid qualificationVersionId)
        {
            return await SendRequestAsync(new GetFeedbackForQualificationFundingByIdQuery(qualificationVersionId));
        }

        [HttpPut("/api/qualifications/{qualificationVersionId}/save-qualification-funding-offers-outcome")]
        [ProducesResponseType(typeof(EmptyResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SaveQualificationFundingOffersOutcome(SaveQualificationFundingOffersOutcomeCommand command, Guid qualificationVersionId)
        {
            command.QualificationVersionId = qualificationVersionId;
            return await SendRequestAsync(command);
        }

        [HttpPut("/api/qualifications/{qualificationVersionId}/save-qualification-funding-offers")]
        [ProducesResponseType(typeof(EmptyResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SaveQualificationFundingOffers(SaveQualificationFundingOffersCommand command, Guid qualificationVersionId)
        {
            command.QualificationVersionId = qualificationVersionId;
            return await SendRequestAsync(command);
        }

        [HttpPut("/api/qualifications/{qualificationVersionId}/save-qualification-funding-offers-details")]
        [ProducesResponseType(typeof(EmptyResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SaveFundingOfferDetails(SaveQualificationFundingOffersDetailsCommand command, Guid qualificationVersionId)
        {
            command.QualificationVersionId = qualificationVersionId;
            return await SendRequestAsync(command);
        }

        [HttpPut("/api/qualifications/{qualificationVersionId}/funding-offers-history-note")]
        [ProducesResponseType(typeof(EmptyResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateQualificationDiscussionHistoryNoteForFundingOffers(CreateQualificationDiscussionHistoryNoteForFundingOffersCommand command, Guid qualificationVersionId)
        {
            command.QualificationVersionId = qualificationVersionId;
            return await SendRequestAsync(command);
        }
        private async Task<IActionResult> HandleNewQualificationCSVExport()
        {
            return await SendRequestAsync(new GetNewQualificationsExportQuery());
        }

        private async Task<IActionResult> HandleChangedQualificationCSVExport()
        {
            return await SendRequestAsync(new GetChangedQualificationsExportQuery());
        }

        private ParamValidationResult ValidateQualificationParams(string? status, int? skip, int? take, string? name, string? organisation, string? qan, string? processStatusFilter)
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
                result.ParsedStatus = status;
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

            if (!string.IsNullOrWhiteSpace(processStatusFilter))
            {
                var procStatusIdStrings = processStatusFilter.Split(',').Select(v => v.Trim());
                try
                {
                    var ids = procStatusIdStrings.Select(s => Guid.Parse(s)).ToList();
                }
                catch
                {
                    result.IsValid = false;
                    result.ErrorMessage = "Process status filter param is invalid.";
                }
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
            public string? ParsedStatus { get; set; }
        }
    }
}


