using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Applications.GetApplications;
using System;
using System.Net;
using System.Threading.Tasks;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using SFA.DAS.FindAnApprenticeship.Domain.Models;

namespace SFA.DAS.FindAnApprenticeship.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApplicationsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ApplicationsController> _logger;

        public ApplicationsController(IMediator mediator, ILogger<ApplicationsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType<GetApplicationsApiResponse>((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Index([FromQuery] Guid candidateId, [FromQuery] ApplicationStatus status)
        {
            try
            {
                var result = await _mediator.Send(new GetApplicationsQuery
                {
                    CandidateId = candidateId,
                    Status = status
                });

                return Ok(GetApplicationsApiResponse.From(result));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Get Applications : An error occurred");
                return new StatusCodeResult((int) HttpStatusCode.InternalServerError);
            }
        }
    }
}
