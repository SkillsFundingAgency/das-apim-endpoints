using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Vacancies.Manage.Api.Models;
using SFA.DAS.Vacancies.Manage.Application.EmployerAccounts.Queries.GetLegalEntitiesForEmployer;
using SFA.DAS.Vacancies.Manage.Application.Providers.Queries.GetProviderAccountLegalEntities;

namespace SFA.DAS.Vacancies.Manage.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class AccountLegalEntitiesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AccountLegalEntitiesController> _logger;

        public AccountLegalEntitiesController (IMediator mediator, ILogger<AccountLegalEntitiesController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetList([FromHeader(Name = "x-request-context-subscription-name")] string accountIdentifier)
        {
            try
            {
                var account = new AccountIdentifier(accountIdentifier);
                
                if (account.AccountType == AccountType.Provider && account.Ukprn == null)
                {
                    return new BadRequestObjectResult("Account Identifier is not in the correct format.");
                }
                
                switch (account.AccountType)
                {
                    case AccountType.Employer:
                        var employerQueryResponse = await _mediator.Send(new GetLegalEntitiesForEmployerQuery
                            {EncodedAccountId = account.AccountPublicHashedId});
                        return Ok((GetAccountLegalEntitiesListResponse) employerQueryResponse);
                    case AccountType.Provider:
                        var providerQueryResponse = await _mediator.Send(new GetProviderAccountLegalEntitiesQuery
                            {Ukprn = account.Ukprn.Value});
                        return Ok((GetAccountLegalEntitiesListResponse) providerQueryResponse);    
                    default:
                        return new StatusCodeResult((int) HttpStatusCode.Forbidden);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Unable to get account legal entities for {accountIdentifier}", e);
                return new StatusCodeResult((int) HttpStatusCode.InternalServerError);
            }
        }
    }
}