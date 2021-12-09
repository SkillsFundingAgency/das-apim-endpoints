using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticePortal.Application.ApprenticeCommitments.Queries;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticePortal.Api.Controllers
{
    [ApiController]
    public class ApprenticeCommitmentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ApprenticeCommitmentsController(IMediator mediator)
            => _mediator = mediator;

        [HttpGet("/apprentices/{id}")]
        public async Task<IActionResult> GetApprentice(Guid id)
        {
            var result = await _mediator.Send(new GetApprenticeQuery { ApprenticeId = id });

            if (result.apprentice == null)
                return NotFound();

            return Ok(result.apprentice);
        }
    }
}
