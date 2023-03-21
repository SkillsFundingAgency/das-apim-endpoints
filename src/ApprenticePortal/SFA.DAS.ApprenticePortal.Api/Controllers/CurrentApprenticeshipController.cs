using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticePortal.Application.ApprenticeAccounts.Queries;
using SFA.DAS.ApprenticePortal.Application.Homepage.Queries;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticePortal.Api.Controllers
{
    [ApiController]
    public class CurrentApprenticeshipController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CurrentApprenticeshipController(IMediator mediator)
            => _mediator = mediator;

        [HttpPost("/apprentices/{id}/apprenticeships/current")]
        public async Task<IActionResult> Post(Guid id, [FromBody] object request)
        {
            var result = await _mediator.Send(new GetApprenticeQuery { ApprenticeId = id });

            if (result.Apprentice == null)
                return NotFound();

            return Ok(result.Apprentice);
        }
    }
}
