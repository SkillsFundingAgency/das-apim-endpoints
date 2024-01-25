using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EarlyConnect.Api.Mappers;
using SFA.DAS.EarlyConnect.Api.Models;
using System.Net;
using SFA.DAS.EarlyConnect.Application.Commands.CreateLogData;
using SFA.DAS.EarlyConnect.Application.Commands.UpdateLogData;

namespace SFA.DAS.EarlyConnect.Api.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("/early-connect/api-log/")]
    public class LogDataController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<LogDataController> _logger;

        public LogDataController(IMediator mediator, ILogger<LogDataController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [Route("add")]
        public async Task<IActionResult> CreateLog(CreateLogPostRequest request)
        {
            try
            {
                var response = await _mediator.Send(new CreateLogDataCommand
                {
                    Log = request.MapFromLogCreateRequest()
                });

                var model = new CreateLogPostResponse()
                {
                    LogId = response.LogId
                };

                return CreatedAtAction(nameof(CreateLog), model);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error posting student data");
                return BadRequest();
            }
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [Route("update")]
        public async Task<IActionResult> UpdateLog([FromBody] UpdateLogPostRequest request)
        {
            try
            {
                await _mediator.Send(new UpdateLogDataCommand
                {
                    Log = request.MapFromLogUpdateRequest()
                });

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error posting log data");
                return BadRequest();
            }
        }
    }
}
