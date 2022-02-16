using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApprenticeFeedback.Api.ApiRequests;
using SFA.DAS.ApprenticeFeedback.Application.Commands.CreateApprentice;
using SFA.DAS.ApprenticeFeedback.Application.Queries.GetApprentice;
using SFA.DAS.SharedOuterApi.Infrastructure;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeFeedback.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class ApprenticeController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ApprenticeController> _logger;

        public ApprenticeController(IMediator mediator, ILogger<ApprenticeController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var result = await _mediator.Send(new GetApprenticeQuery { ApprenticeId = id });
               
                if (result == null)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddApprentice(CreateApprentice request)
        {
            try
            {
                var result = await _mediator.Send(new CreateApprenticeCommand
                {
                    ApprenticeId = request.ApprenticeId,
                    ApprenticeshipId = request.ApprenticeshipId,
                    FirstName = request.FirstName,
                    EmailAddress = request.EmailAddress,
                    Status = request.Status
                });

                return Created("", result);
            }
            catch (HttpRequestContentException e)
            {
                _logger.LogError(e, $"Error attempting to get create apprentice for ApprenticeId: {request.ApprenticeId}");

                return StatusCode((int)e.StatusCode, e.ErrorContent);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to get create apprentice for ApprenticeId: {request.ApprenticeId}");

                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
