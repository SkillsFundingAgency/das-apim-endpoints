using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.LearnerData.Application;
using SFA.DAS.LearnerData.Requests;

namespace SFA.DAS.LearnerData.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LearnersController(IMediator mediator) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post(IEnumerable<LearnerDataRequest> dataRequests)
        {
            await mediator.Send(new ProcessLearnersCommand { Learners = dataRequests });

            return Accepted();
        }
    }
}
