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

        [HttpGet("/apprentices/{id}/apprenticeships")]
        public async Task<IActionResult> GetApprenticeApprenticeships(Guid id)
        {
            var result = await _mediator.Send(new GetApprenticeApprenticeshipsQuery { ApprenticeId = id });

            if (result.apprenticeships == null)
                return NotFound();

            return Ok(result);
        }
    }
}
