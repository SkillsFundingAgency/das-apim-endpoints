using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerIncentives.Api.Models;
using SFA.DAS.EmployerIncentives.Application.Commands.AddLegalEntity;
using SFA.DAS.EmployerIncentives.Application.Commands.RemoveLegalEntity;
using SFA.DAS.EmployerIncentives.Application.Commands.SignAgreement;
using SFA.DAS.EmployerIncentives.Application.Commands.UpdateVendorRegistrationFormCaseStatus;
using SFA.DAS.EmployerIncentives.Application.Queries.GetApplications;
using SFA.DAS.EmployerIncentives.Application.Queries.GetLegalEntities;
using SFA.DAS.EmployerIncentives.Application.Queries.GetLegalEntity;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Api.Controllers
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("/accounts/{accountId}/legalentities")]
        public async Task<IActionResult> GetLegalEntities(long accountId)
        {
            var queryResult = await _mediator.Send(new GetLegalEntitiesQuery
            {
                AccountId = accountId
            });

            var response = queryResult.AccountLegalEntities.Select(c => (AccountLegalEntityDto)c).ToArray();

            return Ok(response);
        }

        [HttpGet]
        [Route("/accounts/{accountId}/legalentities/{accountLegalEntityId}")]
        public async Task<IActionResult> GetLegalEntity(long accountId, long accountLegalEntityId)
        {
            var result = await _mediator.Send(new GetLegalEntityQuery
            {
                AccountId = accountId,
                AccountLegalEntityId = accountLegalEntityId
            });

            var response = (AccountLegalEntityDto)result.AccountLegalEntity;

            return Ok(response);
        }

        [HttpPut]
        [Route("/accounts/{accountId}/legalentities")]
        public async Task<IActionResult> AddLegalEntity(long accountId, LegalEntityRequest request)
        {
            await _mediator.Send(new CreateAccountLegalEntityCommand
            {
                AccountId = accountId,
                OrganisationName = request.OrganisationName,
                LegalEntityId = request.LegalEntityId,
                AccountLegalEntityId = request.AccountLegalEntityId
            });

            return Created("", null);
        }

        [HttpDelete("/accounts/{accountId}/legalentities/{accountLegalEntityId}")]
        public async Task<IActionResult> RemoveLegalEntity(long accountId, long accountLegalEntityId)
        {
            await _mediator.Send(new RemoveAccountLegalEntityCommand
            {
                AccountId = accountId,
                AccountLegalEntityId = accountLegalEntityId
            });

            return Accepted();
        }

        [HttpPatch("/accounts/{accountId}/legalentities/{accountLegalEntityId}")]
        public async Task<IActionResult> SignAgreement(long accountId, long accountLegalEntityId, SignAgreementRequest request)
        {
            await _mediator.Send(new SignAgreementCommand
            {
                AccountId = accountId,
                AccountLegalEntityId = accountLegalEntityId,
                AgreementVersion = request.AgreementVersion,
                LegalEntityId = request.LegalEntityId,
                LegalEntityName = request.LegalEntityName
            });

            return NoContent();
        }

        [HttpGet("/accounts/{accountId}/legalentity/{accountLegalEntityId}/applications")]
        public async Task<IActionResult> GetApplications(long accountId, long accountLegalEntityId)
        {
            var queryResult = await _mediator.Send(new GetApplicationsQuery { AccountId = accountId, AccountLegalEntityId = accountLegalEntityId });

            if (queryResult?.ApprenticeApplications == null)
            {
                return NotFound();
            }

            return Ok(queryResult);
        }
        
        [HttpPut("/accounts/{accountId}/legalentity/{accountLegalEntityId}/vrfcasestatus")]
        public async Task<IActionResult> UpdateVrfCaseStatus(long accountId, long accountLegalEntityId, string vrfCaseStatus)
        {
            await _mediator.Send(new UpdateVendorRegistrationCaseStatusCommand(accountId, accountLegalEntityId, vrfCaseStatus));

            return NoContent();
        }
    }
}