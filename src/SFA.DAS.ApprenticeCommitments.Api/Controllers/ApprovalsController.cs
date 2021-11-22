using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeCommitments.Apis.InnerApi;
using SFA.DAS.ApprenticeCommitments.Application.Commands.ChangeRegistration;
using SFA.DAS.ApprenticeCommitments.Application.Commands.CreateRegistration;
using SFA.DAS.ApprenticeCommitments.Application.Services;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeCommitments.Api.Controllers
{
    [ApiController]
    public class ApprovalsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ResponseReturningApiClient _client;

        public ApprovalsController(IMediator mediator, ResponseReturningApiClient client)
            => (_mediator, _client) = (mediator, client);

        [HttpPost("/approvals")]
        public async Task<ActionResult<CreateRegistrationResponse>> ApprovalCreated(CreateRegistrationCommand request)
            => await _mediator.Send(request);

        [HttpPut]
        [Route("/approvals")]
        public async Task ApprovalUpdated(ChangeRegistrationCommand request)
            => await _mediator.Send(request);

        [HttpPost("/approvals/stopped")]
        public Task<IActionResult> StopRegistration([FromBody] StopRegistrationCommand request)
            => _client.Post("approvals/stopped", request);
    }
}