using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeCommitments.Application.Commands.ChangeRegistration;
using SFA.DAS.ApprenticeCommitments.Application.Commands.CreateRegistration;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeCommitments.Api.Controllers
{
    [ApiController]
    public class ApprovalsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ApprovalsController(IMediator mediator) => _mediator = mediator;

        [HttpPost("/approvals")]
        public async Task<ActionResult<CreateRegistrationResponse>> ApprovalCreated(CreateRegistrationCommand request)
            => await _mediator.Send(request);

        [HttpPut]
        [Route("/approvals")]
        public async Task ApprovalUpdated(ChangeRegistrationCommand request)
            => await _mediator.Send(request);
    }
}