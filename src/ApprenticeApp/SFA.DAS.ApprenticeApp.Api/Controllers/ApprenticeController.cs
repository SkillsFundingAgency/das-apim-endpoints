using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RestEase;
using SFA.DAS.ApprenticeApp.Application.Commands.ApprenticeAccounts;
using SFA.DAS.ApprenticeApp.Application.Commands.ApprenticeSubscriptions;
using SFA.DAS.ApprenticeApp.Application.Queries.ApprenticeAccounts;
using SFA.DAS.SharedOuterApi.Apprentice.GovUK.Auth.Controllers;

namespace SFA.DAS.ApprenticeApp.Api.Controllers
{
    [ApiController]
    public class ApprenticeController(IMediator mediator) : ApprenticeControllerBase(mediator)
    {
        private readonly IMediator _mediator = mediator;


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

        [HttpPatch("/apprentices/{id}")]
        public async Task<IActionResult> UpdateApprentice([Path] Guid id, [Body] object patch)
        {
            await _mediator.Send(new ApprenticePatchCommand
            {
                ApprenticeId = id,
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
        public async Task<IActionResult> ApprenticeRemoveSubscription(Guid id, [FromBody] ApprenticeRemoveSubscriptionRequest request)
        {
            await _mediator.Send(new RemoveApprenticeSubscriptionCommand
            {
                ApprenticeId = id,
                Endpoint = request.Endpoint
            });
            
            return Ok();
        }

        [HttpDelete("/apprentice/{id}")]
        public async Task<IActionResult> DeleteApprenticeAccountById(Guid id)
        {
            var result = await _mediator.Send(new DeleteApprenticeAccountCommand { ApprenticeId = id });
            return Ok(result);
        }

        public class ApprenticeAddSubscriptionRequest
        {
            public string Endpoint { get; set; }
            public string PublicKey { get; set; }
            public string AuthenticationSecret { get; set; }
        }
        public class ApprenticeRemoveSubscriptionRequest
        {
            public string Endpoint { get; set; }
        }
    }
}