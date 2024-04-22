using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RestEase;
using SFA.DAS.ApprenticeApp.Application.Commands.ApprenticeAccounts;
using SFA.DAS.ApprenticeApp.Application.Commands.ApprenticeSubscriptions;
using SFA.DAS.ApprenticeApp.Application.Queries.ApprenticeAccounts;

namespace SFA.DAS.ApprenticeApp.Api.Controllers
{
    [ApiController]
    public class ApprenticeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ApprenticeController(IMediator mediator)
        {
            _mediator = mediator;
        }

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

        [HttpPost("/apprentices/{id}/subscriptions")]
        public async Task<IActionResult> ApprenticeAddSubscription(Guid id, [FromBody] ApprenticeAddSubscriptionRequest request)
        {
            await _mediator.Send(new AddApprenticeSubscriptionCommand
            {
                ApprenticeId = id,
                Endpoint = request.Endpoint,
                PublicKey = request.PublicKey,
                AuthenticationSecret = request.AuthenticationSecret
            });

            return Ok();
        }

        [HttpDelete("/apprentices/{id}/subscriptions")]
        public async Task<IActionResult> ApprenticeDeleteSubscription(Guid id)
        {
            await _mediator.Send(new DeleteApprenticeSubscriptionCommand
            {
                ApprenticeId = id
            });
            
            return Ok();
        }

        public class ApprenticeAddSubscriptionRequest
        {
            public string Endpoint { get; set; }
            public string PublicKey { get; set; }
            public string AuthenticationSecret { get; set; }
        }
    }
}