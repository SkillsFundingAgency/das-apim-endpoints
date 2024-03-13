using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeApp.Application.Queries.Details;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeApp.Api.Controllers
{
    [ApiController]
    public class ApprenticeDetailsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ApprenticeDetailsController(IMediator mediator)
            => _mediator = mediator;

        [HttpGet("/apprentices/{id}/details")]
        public async Task<IActionResult> GetApprenticeDetails(Guid id)
        { 
            var result = await _mediator.Send(new GetApprenticeDetailsQuery { ApprenticeId = id });

            if (result.ApprenticeDetails.Apprentice == null)
                return NotFound();            

            return Ok(result.ApprenticeDetails);
        }
    }
}
