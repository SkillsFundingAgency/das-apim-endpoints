using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.LearnerData.Application;
using SFA.DAS.LearnerData.Requests;
using System.Net;

namespace SFA.DAS.LearnerData.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LearnersController(IMediator mediator, ILogger<LearnersController> logger) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post(IEnumerable<LearnerDataRequest> dataRequests)
        {
            try
            {
                await mediator.Send(new ProcessLearnersCommand { Learners = dataRequests });
                return Accepted();
            }
            catch (Exception e)
            {
                logger.LogError(e, "Internal error occurred when processing learners list");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
