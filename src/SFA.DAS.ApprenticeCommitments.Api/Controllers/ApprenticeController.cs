using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeCommitments.Apis.InnerApi;
using SFA.DAS.ApprenticeCommitments.Application.Commands.ChangeEmailAddress;
using SFA.DAS.ApprenticeCommitments.Application.Commands.CreateApprenticeship;
using SFA.DAS.ApprenticeCommitments.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Threading.Tasks;
using SFA.DAS.ApprenticeCommitments.Application.Commands.UpdateApprenticeship;

namespace SFA.DAS.ApprenticeCommitments.Api.Controllers
{
    [ApiController]
    public class ApprenticeController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IInternalApiClient<ApprenticeCommitmentsConfiguration> _client;

        public ApprenticeController(IMediator mediator, IInternalApiClient<ApprenticeCommitmentsConfiguration> client)
        {
            _mediator = mediator;
            _client = client;
        }

        [HttpPost("/apprentices/{apprenticeId}/email")]
        public async Task<IActionResult> ChangeApprenticeEmailAddress(
            Guid apprenticeId,
            ApprenticeEmailAddressRequest request)
        {
            await _mediator.Send(
                new ChangeEmailAddressCommand(apprenticeId, request.Email));
            return Ok();
        }
    }
}