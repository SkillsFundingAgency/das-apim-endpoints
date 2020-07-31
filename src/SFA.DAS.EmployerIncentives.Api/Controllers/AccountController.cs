using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerIncentives.Api.Models;
using SFA.DAS.EmployerIncentives.Application.Queries.GetLegalEntities;
using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.EmployerIncentives.Interfaces;

namespace SFA.DAS.EmployerIncentives.Api.Controllers
{
    [ApiController]
    public class AccountController :ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountController (IMediator mediator)
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
            
            var response = new AccountLegalEntitiesResponse
            {
                AccountLegalEntities = queryResult.AccountLegalEntities.Select(c=>(AccountLegalEntityDto)c).ToArray()
            };
            
            return Ok(response);
        }
        
    }
}