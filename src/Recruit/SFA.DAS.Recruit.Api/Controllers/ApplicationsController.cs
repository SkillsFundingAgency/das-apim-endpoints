using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Recruit.Api.Models;
using SFA.DAS.Recruit.Application.Candidates.Queries.GetApplicationsById;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SFA.DAS.Recruit.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class ApplicationsController(IMediator mediator, ILogger<ApplicationsController> logger) : ControllerBase
    {
        [HttpPost]
        [Route("GetAll")]
        public async Task<IActionResult> GetApplications([FromBody] GetAllApplicationsByIdApiRequest request)
        {
            try
            {
                var result = await mediator.Send(new GetApplicationsByIdQuery(request.ApplicationIds,
                    request.IncludeDetails));
                return Ok(result);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error fetching get all applications by Id");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}