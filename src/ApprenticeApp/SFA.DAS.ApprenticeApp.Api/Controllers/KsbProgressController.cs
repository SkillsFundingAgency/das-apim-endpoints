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




    }
}
