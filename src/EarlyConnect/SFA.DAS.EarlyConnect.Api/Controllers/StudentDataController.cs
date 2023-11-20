﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EarlyConnect.Api.Mappers;
using SFA.DAS.EarlyConnect.Api.Models;
using System.Net;
using Newtonsoft.Json;
using SFA.DAS.EarlyConnect.Application.Commands.CreateLogData;
using SFA.DAS.EarlyConnect.Application.Commands.CreateStudentData;
using SFA.DAS.EarlyConnect.Application.Commands.UpdateLogData;

namespace SFA.DAS.EarlyConnect.Api.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("/early-connect/student-data/")]
    public class StudentDataController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<StudentDataController> _logger;

        public StudentDataController(IMediator mediator, ILogger<StudentDataController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [Route("add")]
        public async Task<IActionResult> CreateStudentData(CreateStudentDataPostRequest request)
        {
            int logId = 0;

            try
            {
                var ipAddress = (HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault() ?? HttpContext.Connection.RemoteIpAddress?.ToString());

                if (!string.IsNullOrEmpty(ipAddress) && ipAddress.Length > 15)
                {
                    ipAddress = ipAddress.Substring(0, 15);
                }

                logId = await CreateLog(StudentDataUploadStatus.InProgress, request, ipAddress);

                await _mediator.Send(new CreateStudentDataCommand
                {
                    StudentDataList = request.MapFromCreateStudentDataRequest(logId),
                });

                await UpdateLog(logId, StudentDataUploadStatus.Completed);

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error posting student data");

                if (logId > 0) await UpdateLog(logId, StudentDataUploadStatus.Error, $"{e.Message} - {e.StackTrace}");

                return BadRequest();
            }
        }

        private async Task<int> CreateLog(StudentDataUploadStatus status, CreateStudentDataPostRequest request, string ipAddress)
        {
            var actionName = ControllerContext.ActionDescriptor.ActionName;
            var createLogRequest = new CreateLogPostRequest
            {
                RequestType = actionName,
                RequestSource = "UCAS",
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

        private async Task UpdateLog(int logId, StudentDataUploadStatus status, string error = null)
        {
            var updateLog = new UpdateLogPostRequest
            {
                LogId = logId,
                Status = status.ToString(),
                Error = error
            };

            await _mediator.Send(new UpdateLogDataCommand
            {
                Log = LogMapper.MapFromLogUpdateRequest(updateLog)
            });
        }
    }
}
