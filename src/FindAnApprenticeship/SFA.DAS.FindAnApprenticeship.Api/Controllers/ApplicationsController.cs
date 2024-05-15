using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading.Tasks;
using System;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Applications.GetApplications;
using SFA.DAS.FindAnApprenticeship.Models;

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
        public async Task<IActionResult> Index ([FromQuery] Guid candidateId, [FromQuery] ApplicationStatus status)
        {
            try
            {
                var result = await _mediator.Send(new GetApplicationsQuery
                {
                    CandidateId = candidateId,
                    Status = status
                });

                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Get Applications : An error occurred");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
