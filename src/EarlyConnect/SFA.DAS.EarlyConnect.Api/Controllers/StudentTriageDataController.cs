using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EarlyConnect.Api.Models;
using System.Net;
using SFA.DAS.EarlyConnect.Api.Mappers;
using SFA.DAS.EarlyConnect.Application.Commands.CreateOtherStudentTriageData;
using SFA.DAS.EarlyConnect.InnerApi.Requests;
using SFA.DAS.EarlyConnect.Application.Queries.GetStudentTriageDataBySurveyId;
using SFA.DAS.EarlyConnect.Application.Commands.ManageStudentTriageData;
using SFA.DAS.EarlyConnect.Application.Commands.CreateLogData;
using SFA.DAS.EarlyConnect.Application.Commands.UpdateLogData;

namespace SFA.DAS.EarlyConnect.Api.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("/early-connect/student-triage-data/")]
    public class StudentTriageDataController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<StudentTriageDataController> _logger;

        public StudentTriageDataController(IMediator mediator, ILogger<StudentTriageDataController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Route("survey-create")]
        public async Task<IActionResult> CreateStudentTriageData(CreateOtherStudentTriageDataPostRequest request)
        {
            try
            {
                var response = await _mediator.Send(new CreateOtherStudentTriageDataCommand
                {
                    StudentTriageData = new OtherStudentTriageData
                    {
                        Email = request.Email,
                        LepsCode = request.LepsCode
                    }
                });

                return CreatedAtAction(nameof(CreateStudentTriageData), (CreateOtherStudentTriageDataPostResponse)response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error posting student triage data");

                var apiException = (e as SharedOuterApi.Exceptions.ApiResponseException);
                var status = apiException?.Status;
                var errorMessage = apiException?.Error;

                if (status.Equals(HttpStatusCode.InternalServerError))
                {
                    return Problem();
                }

                return BadRequest(errorMessage);

            }
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Route("{surveyGuid}")]
        public async Task<IActionResult> ManageStudentTriageData(ManageStudentTriageDataPostRequest request, [FromRoute] Guid surveyGuid)
        {
            int logId = 0;

            try
            {
                logId = await CreateLog(StudentDataUploadStatus.InProgress, String.Empty, request);

                var response = await _mediator.Send(new ManageStudentTriageDataCommand
                {
                    StudentTriageData = request.MapFromManageStudentTriageDataRequest(),
                    SurveyGuid = surveyGuid
                });

                await UpdateLog(logId, StudentDataUploadStatus.Completed, request, response.Message);

                return CreatedAtAction(nameof(ManageStudentTriageData), (ManageStudentTriageDataPostResponse)response);

            }
            catch (Exception e)
            {
                var apiException = (e as SharedOuterApi.Exceptions.ApiResponseException);
                var status = apiException?.Status;
                var errorMessage = apiException?.Error;
                _logger.LogError(e, "Error posting student data");

                if (logId > 0) await UpdateLog(logId, StudentDataUploadStatus.Error, request, $"Error posting manage student triage data {(errorMessage != null ? $"\nErrorInfo: {errorMessage}" : "")}\nMessage: {e.Message}\nStackTrace: {e.StackTrace}");

                return BadRequest();
            }

        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Route("{surveyGuid}")]
        public async Task<IActionResult> GetStudentTriageData([FromRoute] Guid surveyGuid)
        {
            try
            {
                var result = await _mediator.Send(new GetStudentTriageDataBySurveyIdQuery { SurveyGuid = surveyGuid });

                return Ok((Models.GetStudentTriageDataBySurveyIdResponse)result);
            }
            catch (Exception e)
            {
                var errorMessage = (e as SharedOuterApi.Exceptions.ApiResponseException)?.Error;

                _logger.LogError(e, "Error getting student triage data ");

                return BadRequest($"Error getting student triage data. {(errorMessage != null ? $"\nErrorInfo: {errorMessage}" : "")}");
            }
        }

        private async Task<int> CreateLog(StudentDataUploadStatus status, string ipAddress, ManageStudentTriageDataPostRequest request)
        {
            if (request.StudentSurvey.DateCompleted == null)
            {
                return 0;
            }
            var actionName = ControllerContext.ActionDescriptor.ActionName;
            var createLogRequest = new CreateLogPostRequest
            {
                RequestType = actionName,
                RequestSource = request.DataSource,
                RequestIP = ipAddress,
                Payload = String.Empty,
                FileName = $"StudentId-{request.Id}|StudentSurveyId-{(request.StudentSurvey?.Id != null ? request.StudentSurvey.Id.ToString() : "")}",
                Status = status.ToString()
            };

            var response = await _mediator.Send(new CreateLogDataCommand
            {
                Log = LogMapper.MapFromLogCreateRequest(createLogRequest)
            });

            return response.LogId;
        }

        private async Task UpdateLog(int logId, StudentDataUploadStatus status, ManageStudentTriageDataPostRequest request, string message = null)
        {
            if (request.StudentSurvey.DateCompleted == null)
            {
                return;
            }
            var updateLog = new UpdateLogPostRequest
            {
                LogId = logId,
                Status = status.ToString(),
                Error = message
            };

            await _mediator.Send(new UpdateLogDataCommand
            {
                Log = LogMapper.MapFromLogUpdateRequest(updateLog)
            });
        }
    }
}
