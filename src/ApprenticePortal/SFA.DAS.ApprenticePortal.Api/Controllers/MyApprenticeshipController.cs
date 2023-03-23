using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using SFA.DAS.ApprenticePortal.Application.ApprenticeAccounts.Commands;

namespace SFA.DAS.ApprenticePortal.Api.Controllers
{
    [ApiController]
    public class MyApprenticeshipController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MyApprenticeshipController(IMediator mediator)
            => _mediator = mediator;

        [HttpPost("/apprentices/{id}/my-apprenticeship")]
        public async Task<IActionResult> Post(Guid id, [FromBody] MyApprenticeshipConfirmedRequest request)
        {
            var result = await _mediator.Send(new AddOrUpdateMyApprenticeshipCommand
            {
                ApprenticeId = id, CommitmentsApprenticeshipId = request.CommitmentsApprenticeshipId,
                CommitmentsApprovedOn = request.ApprovedOn
            });

            return Ok();
        }
    }

    public class MyApprenticeshipConfirmedRequest 
    {
        public long CommitmentsApprenticeshipId { get; set; }
        public DateTime ApprovedOn { get; set; }
    }
}
