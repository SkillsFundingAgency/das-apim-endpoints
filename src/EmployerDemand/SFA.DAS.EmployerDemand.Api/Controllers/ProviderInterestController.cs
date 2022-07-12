using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerDemand.Api.ApiRequests;
using SFA.DAS.EmployerDemand.Application.ProviderInterest.Commands.CreateProviderInterests;
using SFA.DAS.SharedOuterApi.Infrastructure;

namespace SFA.DAS.EmployerDemand.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class ProviderInterestController : ControllerBase
    {
        private readonly ILogger<ProviderInterestController> _logger;
        private readonly IMediator _mediator;

        public ProviderInterestController(
            ILogger<ProviderInterestController> logger,
            IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateProviderInterests(CreateProviderInterestsRequest request)
        {
            try
            {
                var commandResult = await _mediator.Send((CreateProviderInterestsCommand)request);

                return Created("", commandResult);
            }
            catch (HttpRequestContentException e)
            {
                return StatusCode((int) e.StatusCode, e.ErrorContent);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error creating provider interests");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}