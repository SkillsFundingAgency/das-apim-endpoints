using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeApp.Application.Commands;
using SFA.DAS.ApprenticeApp.Application.Queries.KsbProgress;
using SFA.DAS.ApprenticeApp.Models;

namespace SFA.DAS.ApprenticeApp.Api.Controllers
{
    [ApiController]
    public class KsbProgressController : ControllerBase
    {
        private readonly IMediator _mediator;

        public KsbProgressController(IMediator mediator)
            => _mediator = mediator;

        // gets the ksb types
        [HttpPost("/apprentices/{id}/ksbs")]
        public async Task<IActionResult> AddUpdateKsbProgress(Guid id, ApprenticeKsbProgressData data)
        {
            await _mediator.Send(new AddUpdateKsbProgressCommand
            {
                ApprenticeshipId = id,
                Data = data
            });

            return Ok();
        }

        // remove the ksb to task association
        [HttpDelete("/apprentices/{id}/ksbs/{ksbProgressId}/taskid/{taskId}")]
        public async Task<IActionResult> RemoveTaskToKsbProgress(Guid id, int ksbProgressId, int taskId)
        {
            await _mediator.Send(new RemoveTaskToKsbProgressCommand
            {
                TaskId = taskId,
                KsbProgressId = ksbProgressId,
                ApprenticeshipId = id
            });

            return Ok();
        }

        [HttpGet("/apprentices/{apprenticeshipIdentifier}/ksbs")]
        public async Task<IActionResult> GetKsbsByApprenticeshipIdAndGuidListQuery(Guid apprenticeshipIdentifier, [FromQuery] Guid[] guids)
        {
            var queryResult = await _mediator.Send(new GetKsbsByApprenticeshipIdAndGuidListQuery
            {
                ApprenticeshipId = apprenticeshipIdentifier,
                Guids = guids
            });

            return Ok(queryResult.KSBProgresses);
        }
    }
}
