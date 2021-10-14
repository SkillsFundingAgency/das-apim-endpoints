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
        public async Task<IActionResult> GetList([FromHeader(Name = "x-request-context-account-identifier")] string accountIdentifier)
        {
            try
            {
                if (accountIdentifier.Split("|").Length != 2)
                {
                    return new BadRequestObjectResult("Account Identifier is not in the correct format.");
                }
                
                var id = accountIdentifier.Split("|")[1];

                switch (accountIdentifier.Split("|")[0])
                {
                    case "Employer":
                        var employerQueryResponse = await _mediator.Send(new GetLegalEntitiesForEmployerQuery
                            {EncodedAccountId = id.ToUpper()});
                        return Ok((GetAccountLegalEntitiesListResponse) employerQueryResponse);
                    case "Provider":

                        if (int.TryParse(id, out var providerId))
                        {
                            var providerQueryResponse = await _mediator.Send(new GetProviderAccountLegalEntitiesQuery
                                {Ukprn = providerId});
                            return Ok((GetAccountLegalEntitiesListResponse) providerQueryResponse);    
                        }
                        return new BadRequestObjectResult("Provider Id is not numeric");
                        
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