using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticePortal.Application.ApprenticeAccounts.Queries;
using SFA.DAS.ApprenticePortal.Application.Homepage.Queries;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticePortal.Api.Controllers
{
    [ApiController]
    public class ApprenticeHomepageController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ApprenticeHomepageController(IMediator mediator)
            => _mediator = mediator;

        [HttpGet("/apprentices/{id}/homepage")]
        public async Task<IActionResult> GetHomepageApprentice(Guid id)
        { 
            var result = await _mediator.Send(new GetApprenticeHomepageQuery { ApprenticeId = id });

            if (result.ApprenticeHomepage.Apprentice == null)
                return NotFound();            

            return Ok(result.ApprenticeHomepage);
        }
    }
}
