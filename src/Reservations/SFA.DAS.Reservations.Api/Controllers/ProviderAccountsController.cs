using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Reservations.Api.Models;
using SFA.DAS.Reservations.Application.ProviderAccounts.Queries;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;

namespace SFA.DAS.Reservations.Api.Controllers;

[ApiController]
[Route("[controller]/")]
public class ProviderAccountsController(IMediator mediator, ILogger<ProviderAccountsController> logger) : Controller
{
    [HttpGet]
    [Route("{ukprn}")]
    public async Task<IActionResult> GetProviderStatus([FromRoute] int ukprn)
    {
        try
        {
            var result = await mediator.Send(new GetRoatpV2ProviderQuery
            {
                Ukprn = ukprn
            });

            return Ok(new ProviderAccountResponse { CanAccessService = result });
        }
        catch (Exception)
        {
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpGet]
    [Route("{ukprn}/legalentities-with-create-cohort")]
    public async Task<IActionResult> GetAccountLegalEntitiesWithCreatCohort([FromRoute] int ukprn)
    {
        try
        {
            var request = new GetProviderAccountLegalEntitiesWithCreatCohortQuery(ukprn);
            var result = await mediator.Send(request);

            return Ok(result);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "GetAccountLegalEntities() threw an exception.");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }
}