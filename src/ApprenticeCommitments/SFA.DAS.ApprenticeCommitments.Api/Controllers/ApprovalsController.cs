using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeCommitments.Apis.InnerApi;
using SFA.DAS.ApprenticeCommitments.Application.Commands.CreateApproval;
using SFA.DAS.ApprenticeCommitments.Application.Commands.UpdateApproval;
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
        public async Task<ActionResult<CreateApprovalResponse>> ApprovalCreated(CreateApprovalCommand request)
            => await _mediator.Send(request);

        [HttpPut]
        [Route("/approvals")]
        public async Task ApprovalUpdated(UpdateApprovalCommand request)
            => await _mediator.Send(request);

        [HttpPost("/approvals/stopped")]
        public Task<IActionResult> StopRegistration([FromBody] StopApprovalCommand request)
            => _client.Post("approvals/stopped", request);

        [HttpGet("/approvals/{commitmentsApprenticeshipId}/registration")]
        public Task<IActionResult> GetApprovalsRegistration(long commitmentsApprenticeshipId)
            => _client.Get($"approvals/{commitmentsApprenticeshipId}/registration");
    }
}