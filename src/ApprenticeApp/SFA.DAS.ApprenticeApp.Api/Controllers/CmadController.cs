using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeApp.Application.Commands.Cmad;
using SFA.DAS.ApprenticeApp.Application.Queries.Cmad.GetCommitmentsApprenticeshipById;
using SFA.DAS.ApprenticeApp.Application.Queries.Cmad.GetRegistrationsByAccountDetails;
using SFA.DAS.ApprenticeApp.Application.Queries.Cmad.GetRevisionById;
using SFA.DAS.ApprenticeApp.Models;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeApp.Api.Controllers
{
    [ApiController]
    public class CmadController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CmadController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("/registrations")]
        public async Task<IActionResult> GetRegistrationsByAccountDetails(string firstName, string lastName, DateTime dateOfBirth)
        {
            var result = await _mediator.Send(
                new GetRegistrationsByAccountDetailsQuery
                {
                    FirstName = firstName,
                    LastName = lastName,
                    DateOfBirth = dateOfBirth
                });

            return Ok(result.Registrations);
        }

        [HttpGet("/revisionsById")]
        public async Task<IActionResult> GetRevisionsById(Guid apprenticeId, long apprenticeshipId, long revisionId)
        {
            var result = await _mediator.Send(
                new GetRevisionsByIdQuery
                {
                    ApprenticeId = apprenticeId,
                    ApprenticeshipId = apprenticeshipId,
                    RevisionId = revisionId
                });

            return Ok(result.Revision);
        }

        [HttpGet("commitments-apprenticeships/{apprenticeshipId}")]
        public async Task<IActionResult> GetCommitmentsApprenticeshipById(long apprenticeshipId)
        {
            var result = await _mediator.Send(new GetCommitmentsApprenticeshipByIdQuery
            {
                ApprenticeshipId = apprenticeshipId
            });

            return Ok(result);
        }

        [HttpPost("/apprenticeships")]
        public async Task<IActionResult> CreateApprenticeship(Guid registrationId, Guid apprenticeId, string lastName, DateTime dateOfBirth)
        {
            var result = await _mediator.Send(new CreateApprenticeshipFromRegistrationCommand
            {
                RegistrationId = registrationId,
                ApprenticeId = apprenticeId,
                LastName = lastName,
                DateOfBirth = dateOfBirth
            });

            return Ok();
        }

        [HttpPost("/apprentices/{id}/MyApprenticeship")]
        public async Task<IActionResult> CreateMyApprenticeship(Guid id, CreateMyApprenticeshipData data)
        {
            var result = await _mediator.Send(new CreateMyApprenticeshipCommand { ApprenticeId = id, Data = data });            

            return string.IsNullOrEmpty(result.ErrorContent)
                ? StatusCode((int)result.StatusCode)
                : StatusCode((int)result.StatusCode, result.ErrorContent);
        }
    }
}
