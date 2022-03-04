using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Models;
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

        /// <summary>
        /// GET list of Account Legal Entities.
        /// </summary>
        /// <remarks>
        /// Get a list of Account Legal Entities that are connected to your subscription. The AccountLegalEntityPublicHashedId is required for Vacancy creation.
        /// If you are a provider only Accounts that have given permission for you to act on there behalf will show in the list. If you are an employer then only
        /// legal entities that have a signed agreement will be in the list.
        /// </remarks>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(GetAccountLegalEntitiesListResponse), (int) HttpStatusCode.OK)]
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
                            {EncodedAccountId = account.AccountHashedId});
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