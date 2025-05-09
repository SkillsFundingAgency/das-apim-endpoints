using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Applications.GetApplications;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Applications.GetApplicationsCount;
using SFA.DAS.FindAnApprenticeship.Domain.Models;

namespace SFA.DAS.FindAnApprenticeship.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApplicationsController(IMediator mediator, ILogger<ApplicationsController> logger) : Controller
    {
        [HttpGet]
        [ProducesResponseType<GetApplicationsApiResponse>((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Index([FromQuery] Guid candidateId, [FromQuery] ApplicationStatus status)
        {
            try
            {
                var result = await mediator.Send(new GetApplicationsQuery
                {
                    CandidateId = candidateId,
                    Status = status
                });

                return Ok(GetApplicationsApiResponse.From(result));
            }
            catch (Exception e)
            {
                logger.LogError(e, "Get Applications : An error occurred");
                return new StatusCodeResult((int) HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("count")]
        [ProducesResponseType<GetApplicationsCountQueryResult>((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Count([FromQuery] Guid candidateId, [FromQuery] ApplicationStatus status)
        {
            try
            {
                var result = await mediator.Send(new GetApplicationsCountQuery(candidateId, status));

                return Ok(result);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Get Applications count : An error occurred");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
