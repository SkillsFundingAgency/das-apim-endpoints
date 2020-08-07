using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerIncentives.Api.Models;
using SFA.DAS.EmployerIncentives.Application.Commands.AddLegalEntity;
using SFA.DAS.EmployerIncentives.Application.Commands.RemoveLegalEntity;
using SFA.DAS.EmployerIncentives.Application.Queries.GetLegalEntities;
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

        [HttpPost]
        [Route("/accounts/{accountId}/legalentities")]
        public async Task<IActionResult> AddLegalEntity(long accountId, LegalEntityRequest request)
        {
            var queryResult = await _mediator.Send(new CreateAccountLegalEntityCommand
            {
                AccountId = accountId,
                OrganisationName = request.OrganisationName,
                LegalEntityId = request.LegalEntityId,
                AccountLegalEntityId = request.AccountLegalEntityId
            });

            var response = new CreatedAccountLegalEntityResponse
            {
                AccountLegalEntity = queryResult.AccountLegalEntity
            };

            return Created("", response);
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
    }
}