using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.Index;

namespace SFA.DAS.FindAnApprenticeship.Api.Controllers
{
    [ApiController]
    [Route("[controller]s/{applicationId}")]
    public class ApplicationController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ApplicationController> _logger;

        public ApplicationController(IMediator mediator, ILogger<ApplicationController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromRoute] Guid applicationId, [FromQuery] Guid candidateId)
        {
            try
            {
                var result = await _mediator.Send(new GetIndexQuery
                    { CandidateId = candidateId, ApplicationId = applicationId });

                return Ok((GetIndexApiResponse)result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error getting application index {applicationId}", applicationId);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
  
        }
    }
}
