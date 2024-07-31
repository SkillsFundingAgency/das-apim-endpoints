using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeApp.Application.Commands.ApprenticeAccounts;
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

        // gets the ksb types
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




    }
}
