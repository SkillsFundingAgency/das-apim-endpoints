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
    public class ApprenticeshipController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IInternalApiClient<ApprenticeCommitmentsConfiguration> _client;

        public ApprenticeshipController(IMediator mediator, IInternalApiClient<ApprenticeCommitmentsConfiguration> client)
        {
            _mediator = mediator;
            _client = client;
        }

        [HttpPost]
        [Route("/apprenticeships")]
        public async Task<IActionResult> AddApprenticeship(CreateApprenticeshipCommand request)
        {
            await _mediator.Send(request);
            return Accepted();
        }

        [HttpPost]
        [Route("/apprenticeships/update")]
        public async Task<IActionResult> UpdateApprenticeship(UpdateApprenticeshipCommand request)
        {
            await _mediator.Send(request);
            return Accepted();
        }

        [HttpGet("/apprentices/{apprenticeId}/apprenticeships/{apprenticeshipId}")]
        public async Task<IActionResult> GetApprenticeship(Guid apprenticeId, long apprenticeshipId)
        {
            var response = await _client.Get<ApprenticeshipResponse>(new GetApprenticeshipRequest(apprenticeId, apprenticeshipId));

            if (response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }

        [HttpPost("/apprentices/{apprenticeId}/apprenticeships/{apprenticeshipId}/trainingproviderconfirmation")]
        [HttpPost("/apprentices/{apprenticeId}/apprenticeships/{apprenticeshipId}/{commitmentStatementId}/trainingproviderconfirmation")]
        public async Task<IActionResult> TrainingProviderConfirmation(
            Guid apprenticeId, long apprenticeshipId, long commitmentStatementId,
            [FromBody] TrainingProviderConfirmationRequestData request)
        {
            await _client.Post(
                new TrainingProviderConfirmationRequest(
                    apprenticeId, apprenticeshipId, commitmentStatementId, request.TrainingProviderCorrect));

            return Ok();
        }

        [HttpPost("/apprentices/{apprenticeId}/apprenticeships/{apprenticeshipId}/employerconfirmation")]
        [HttpPost("/apprentices/{apprenticeId}/apprenticeships/{apprenticeshipId}/{commitmentStatementId}/employerconfirmation")]
        public async Task<IActionResult> EmployerConfirmation(
            Guid apprenticeId, long apprenticeshipId, long commitmentStatementId,
            [FromBody] EmployerConfirmationRequestData request)
        {
            await _client.Post(
                new EmployerConfirmationRequest(
                    apprenticeId, apprenticeshipId, commitmentStatementId, request.EmployerCorrect));

            return Ok();
        }

        [HttpPost("/apprentices/{apprenticeId}/apprenticeships/{apprenticeshipId}/apprenticeshipdetailsconfirmation")]
        [HttpPost("/apprentices/{apprenticeId}/apprenticeships/{apprenticeshipId}/{commitmentStatementId}/apprenticeshipdetailsconfirmation")]
        public async Task<IActionResult> ApprenticeshipDetailsConfirmation(
            Guid apprenticeId, long apprenticeshipId, long commitmentStatementId,
            [FromBody] ApprenticeshipDetailsConfirmationRequestData request)
        {
            await _client.Post(
                new ApprenticeshipDetailsConfirmationRequest(
                    apprenticeId, apprenticeshipId, commitmentStatementId, request.ApprenticeshipDetailsCorrect));

            return Ok();
        }

        [HttpPost("/apprentices/{apprenticeId}/apprenticeships/{apprenticeshipId}/rolesandresponsibilitiesconfirmation")]
        [HttpPost("/apprentices/{apprenticeId}/apprenticeships/{apprenticeshipId}/{commitmentStatementId}/rolesandresponsibilitiesconfirmation")]
        public async Task<IActionResult> RolesAndResponsibilitiesConfirmation(
            Guid apprenticeId, long apprenticeshipId, long commitmentStatementId,
            [FromBody] RolesAndResponsibilitiesConfirmationRequestData request)
        {
            await _client.Post(
                new RolesAndResponsibilitiesConfirmationRequest(
                    apprenticeId, apprenticeshipId, commitmentStatementId, request.RolesAndResponsibilitiesCorrect));
            
            return Ok();
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

        [HttpPost("/apprentices/{apprenticeId}/apprenticeships/{apprenticeshipId}/howapprenticeshipwillbedeliveredconfirmation")]
        [HttpPost("/apprentices/{apprenticeId}/apprenticeships/{apprenticeshipId}/{commitmentStatementId}/howapprenticeshipwillbedeliveredconfirmation")]
        public async Task<IActionResult> HowApprenticeshipWillBeDeliveredConfirmation(
            Guid apprenticeId, long apprenticeshipId, long commitmentStatementId,
            [FromBody] HowApprenticeshipWillBeDeliveredRequestData request)
        {
            await _client.Post(
                new HowApprenticeshipWillBeDeliveredRequest(
                    apprenticeId, apprenticeshipId, commitmentStatementId, request.HowApprenticeshipDeliveredCorrect));

            return Ok();
        }

        [HttpPost("/apprentices/{apprenticeId}/apprenticeships/{apprenticeshipId}/apprenticeshipconfirmation")]
        [HttpPost("/apprentices/{apprenticeId}/apprenticeships/{apprenticeshipId}/{commitmentStatementId}/apprenticeshipconfirmation")]
        public async Task<IActionResult> ApprenticeshipConfirmation(
            Guid apprenticeId, long apprenticeshipId, long commitmentStatementId,
            [FromBody] ApprenticeshipConfirmationRequestData request)
        {
            await _client.Post(
                new ApprenticeshipConfirmationRequest(
                    apprenticeId, apprenticeshipId, commitmentStatementId, request.ApprenticeshipCorrect));

            return Ok();
        }
    }
}