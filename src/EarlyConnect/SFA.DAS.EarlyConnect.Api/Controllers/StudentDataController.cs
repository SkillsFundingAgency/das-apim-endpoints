using MediatR;
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
    [Route("/early-connect/student-data")]
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
            int LogId = 0;

            try
            {
                var actionName = ControllerContext.ActionDescriptor.ActionName;
                var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
                var jsonPayload = JsonConvert.SerializeObject(request);

                CreateLogPostRequest createLogPostRequest = new CreateLogPostRequest
                {
                    RequestType = actionName,
                    RequestSource = "UCAS",
                    RequestIP = ipAddress,
                    Payload = jsonPayload,
                    Status = StudentDataUploadStatus.InProgress.ToString()
                };

                var response = await _mediator.Send(new CreateLogDataCommand
                {
                    Log = LogMapper.MapFromLogCreateRequest(createLogPostRequest)
                });

                LogId = response.LogId;

                await _mediator.Send(new CreateStudentDataCommand
                {
                    StudentDataList = request.MapFromCreateStudentDataRequest(LogId),
                });

                UpdateLogPostRequest updateLog = new UpdateLogPostRequest
                {
                    LogId = LogId,
                    Status = StudentDataUploadStatus.Completed.ToString()
                };

                await _mediator.Send(new UpdateLogDataCommand
                {
                    Log = LogMapper.MapFromLogUpdateRequest(updateLog)
                });

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error posting student data");

                UpdateLogPostRequest updateLog = new UpdateLogPostRequest
                {
                    LogId = LogId,
                    Status = StudentDataUploadStatus.Error.ToString(),
                    Error = e.Message
                };

                await _mediator.Send(new UpdateLogDataCommand
                {
                    Log = LogMapper.MapFromLogUpdateRequest(updateLog)
                });

                return BadRequest();
            }
        }
        //[HttpPost]
        //[ProducesResponseType((int)HttpStatusCode.OK)]
        //[Route("")]
        //public async Task<IActionResult> Post(CreateStudentDataPostRequest request)
        //{
        //    try
        //    {
        //        var actionName = ControllerContext.ActionDescriptor.DisplayName;
        //        var ipAddress = HttpContext.Connection.RemoteIpAddress.ToString();
        //        var jsonPayload = JsonConvert.SerializeObject(request);

        //        await _mediator.Send(new CreateStudentDataCommand
        //        {
        //            StudentDataList = request.MapFromCreateStudentDataRequest(),
        //            RequestIP = ipAddress,
        //            Payload = jsonPayload,
        //            RequestType = actionName,
        //            RequestSource = "UCAS"
        //        });

        //        return Ok();
        //    }
        //    catch (Exception e)
        //    {
        //        _logger.LogError(e, "Error posting student data");
        //        return BadRequest();
        //    }
        //}
    }
}
