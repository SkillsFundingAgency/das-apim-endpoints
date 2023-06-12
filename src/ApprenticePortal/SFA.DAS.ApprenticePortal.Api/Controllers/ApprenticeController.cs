using MediatR;
using Microsoft.AspNetCore.Mvc;
using RestEase;
using SFA.DAS.ApprenticePortal.Application.Queries.ApprenticeAccounts;
using System;
using System.Threading.Tasks;
using SFA.DAS.ApprenticePortal.Application.Commands.ApprenticeAccounts;

namespace SFA.DAS.ApprenticePortal.Api.Controllers
{
    [ApiController]
    public class ApprenticeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ApprenticeController(IMediator mediator)
            => _mediator = mediator;

        [HttpGet]
        [Route("/apprentices/{id}")]
        public async Task<IActionResult> GetApprentice([Path] Guid id)
        {
            var queryResult = await _mediator.Send(new GetApprenticeQuery
            {
                ApprenticeId = id
            });

            if (queryResult.Apprentice == null)
                return NotFound();

            return Ok(queryResult.Apprentice);
        }

        [HttpPatch("/apprentices/{apprenticeId}")]
        public async Task<IActionResult> UpdateApprentice([Path] Guid apprenticeId, [Body] object patch)
        {
            await _mediator.Send(new ApprenticePatchCommand
            {
                ApprenticeId = apprenticeId,
                Patch = patch
            });

            return NoContent();
        }

    }
}
