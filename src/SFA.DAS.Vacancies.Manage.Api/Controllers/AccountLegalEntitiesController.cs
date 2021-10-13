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
                    return new StatusCodeResult((int) HttpStatusCode.BadRequest);
                }
                
                var id = accountIdentifier.Split("|")[1];

                switch (accountIdentifier.Split("|")[0])
                {
                    case "Employer":
                        var employerQueryResponse = await _mediator.Send(new GetLegalEntitiesForEmployerQuery
                            {EncodedAccountId = id});
                        return Ok((GetAccountLegalEntitiesListResponse) employerQueryResponse);
                    case "Provider":
                        var providerQueryResponse = await _mediator.Send(new GetProviderAccountLegalEntitiesQuery
                            {Ukprn = Convert.ToInt32(id)});
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