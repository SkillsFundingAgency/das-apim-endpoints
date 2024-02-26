using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using SFA.DAS.EarlyConnect.Api.Models;
using SFA.DAS.EarlyConnect.Application.Queries.GetLEPSDataWithUsers;
using Newtonsoft.Json;
using SFA.DAS.EarlyConnect.Api.Mappers;
using SFA.DAS.EarlyConnect.Application.Commands.CreateLogData;
using SFA.DAS.EarlyConnect.Application.Commands.CreateStudentData;
using SFA.DAS.EarlyConnect.Application.Commands.UpdateLogData;
using SFA.DAS.EarlyConnect.Application.Commands.CreateStudentFeedback;

namespace SFA.DAS.EarlyConnect.Api.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("/early-connect/leps-data/")]
    public class LepsDataController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<LepsDataController> _logger;
        private readonly string DataSource = "LEPS";

        public LepsDataController(IMediator mediator, ILogger<LepsDataController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Route("")]
        public async Task<IActionResult> GetLepsDataWithUsers()
        {
            try
            {
                var result = await _mediator.Send(new GetLEPSDataWithUsersQuery { });

                return Ok((GetLEPSDataListWithUsersResponse)result);
            }
            catch (Exception e)
            {
                var errorMessage = (e as SharedOuterApi.Exceptions.ApiResponseException)?.Error;

                _logger.LogError(e, "Error getting leps data with users ");

                return BadRequest($"Error getting leps data with users. {(errorMessage != null ? $"\nErrorInfo: {errorMessage}" : "")}");
            }
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [Route("student-feedback")]
        public async Task<IActionResult> CreateStudentFeedback(CreateStudentFeedbackPostRequest request)
        {
            int logId = 0;

            try
            {
                var ipAddress = (HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault() ?? HttpContext.Connection.RemoteIpAddress?.ToString());

                if (!string.IsNullOrEmpty(ipAddress) && ipAddress.Length > 50)
                {
                    ipAddress = ipAddress.Substring(0, 50);
                }

                logId = await CreateLog(StudentDataUploadStatus.InProgress, request, ipAddress);

                var response = await _mediator.Send(new CreateStudentFeedbackCommand
                {
                    StudentFeedbackList = request.MapFromCreateStudentFeedbackRequest(logId),
                });

                await UpdateLog(logId, StudentDataUploadStatus.Completed, response.Message);

                return CreatedAtAction(nameof(CreateStudentFeedback), null);
            }
            catch (Exception e)
            {
                var apiException = (e as SharedOuterApi.Exceptions.ApiResponseException);
                var status = apiException?.Status;
                var errorMessage = apiException?.Error;
                _logger.LogError(e, "Error posting student feedback");

                if (logId > 0) await UpdateLog(logId, StudentDataUploadStatus.Error, $"Error posting student feedback. {(errorMessage != null ? $"\nErrorInfo: {errorMessage}" : "")}\nMessage: {e.Message}\nStackTrace: {e.StackTrace}");

                if (status.Equals(HttpStatusCode.InternalServerError))
                {
                    return Problem();
                }

                return BadRequest(errorMessage);
            }
        }

        private async Task<int> CreateLog(StudentDataUploadStatus status, CreateStudentFeedbackPostRequest request, string ipAddress)
        {
            var actionName = ControllerContext.ActionDescriptor.ActionName;
            var createLogRequest = new CreateLogPostRequest
            {
                RequestType = actionName,
                RequestSource = DataSource,
                RequestIP = ipAddress,
                Payload = JsonConvert.SerializeObject(request),
                Status = status.ToString()
            };

            var response = await _mediator.Send(new CreateLogDataCommand
            {
                Log = LogMapper.MapFromLogCreateRequest(createLogRequest)
            });

            return response.LogId;
        }

        private async Task UpdateLog(int logId, StudentDataUploadStatus status, string message = null)
        {
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
