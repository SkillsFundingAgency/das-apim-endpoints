using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EarlyConnect.Api.Mappers;
using SFA.DAS.EarlyConnect.Api.Models;
using System.Net;
using SFA.DAS.EarlyConnect.Application.Commands.CreateLog;
using SFA.DAS.EarlyConnect.Application.Commands.UpdateLog;

namespace SFA.DAS.EarlyConnect.Api.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("/api/log/")]
    public class LogController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<LogController> _logger;

        public LogController(IMediator mediator, ILogger<LogController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [Route("add")]
        public async Task<IActionResult> CreateLog(CreateLogPostRequest request)
        {
            try
            {
                await _mediator.Send(new CreateLogCommand
                {
                    Log = request.MapFromLogCreateRequest()
                });

                return Ok();
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
                await _mediator.Send(new UpdateLogCommand
                {
                    Log = request.MapFromLogUpdateRequest()
                });

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error posting student data");
                return BadRequest();
            }
        }
    }
}
