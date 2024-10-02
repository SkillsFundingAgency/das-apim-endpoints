using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Applications.GetApplications;
using System;
using System.Net;
using System.Threading.Tasks;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Applications.GetSubmittedApplications;
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
        public async Task<IActionResult> Index([FromQuery] Guid candidateId, [FromQuery] ApplicationStatus status)
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
                return new StatusCodeResult((int) HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("submitted")]
        public async Task<IActionResult> GetSubmittedApplications([FromQuery] Guid candidateId)
        {
            try
            {
                var result = await _mediator.Send(new GetSubmittedApplicationsQuery(candidateId)
                {
                    CandidateId = candidateId,
                });

                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Get Submitted Applications : An error occurred");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
