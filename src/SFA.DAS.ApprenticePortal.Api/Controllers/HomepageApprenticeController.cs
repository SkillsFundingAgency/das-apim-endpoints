using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticePortal.Application.ApprenticeHomePage.Queries;
using System;
using System.Threading.Tasks;


namespace SFA.DAS.ApprenticePortal.Api.Controllers
{
    [ApiController]
    public class HomepageApprenticeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public HomepageApprenticeController(IMediator mediator)
            => _mediator = mediator;

        [HttpGet("/homepage/apprentice/{id}")]
        public async Task<IActionResult> GetHomepageApprentice(Guid id)
        { 
            var result = await _mediator.Send(new GetHomepageApprenticeQuery { ApprenticeId = id });

            return Ok(result.homePageApprentice);
        }
    }
}
