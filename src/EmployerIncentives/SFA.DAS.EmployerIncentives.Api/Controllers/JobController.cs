using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerIncentives.Application.Commands.AddJobRequest;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Infrastructure;

namespace SFA.DAS.EmployerIncentives.Api.Controllers
{
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly IMediator _mediator;

        public JobController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPut]
        [Route("/jobs")]
        public async Task<IActionResult> AddJob([FromBody] JobRequest request)
        {
            try
            {
                await _mediator.Send(new AddJobCommand(request.Type, request.Data), CancellationToken.None);

                return NoContent();
            }
            catch (HttpRequestContentException requestException)
            {
                return BadRequest(requestException.ErrorContent);
            }
        }
    }
}
