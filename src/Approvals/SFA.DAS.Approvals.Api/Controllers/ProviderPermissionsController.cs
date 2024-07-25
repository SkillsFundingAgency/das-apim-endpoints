using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.Application.ProviderPermissions.Queries;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ProviderRelationships;
using SFA.DAS.SharedOuterApi.Models.ProviderRelationships;

namespace SFA.DAS.Approvals.Api.Controllers;

[ApiController]
[Route("[controller]/")]
public class ProviderPermissionsController(ISender mediator, ILogger<ProviderPermissionsController> logger) : Controller﻿
{
    [HttpGet]
    [Route("has-permission")]
    public async Task<IActionResult> HasPermission([FromQuery] long? ukPrn, [FromQuery] long? accountLegalEntityId, [FromQuery] Operation operation)
    {
        try
        {
            var result = await mediator.Send(new GetHasPermissionQuery(ukPrn, accountLegalEntityId, operation));
            return Ok(new GetHasPermissionResponse { HasPermission = result });
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "{method} threw an exception.", nameof(HasPermission));
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpGet]
    [Route("account-provider-legal-entities")]
    public async Task<IActionResult> AccountProviderLegalEntities([FromQuery] int? ukprn, [FromQuery] Operation[] operations,
        [FromQuery] string accountHashedId)
    {
        try
        {
            var result =
                await mediator.Send(new GetAccountProviderLegalEntitiesQuery(ukprn, operations, accountHashedId));
            return Ok(new GetProviderAccountLegalEntitiesResponse { AccountProviderLegalEntities = result });
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "{method} threw an exception.", nameof(AccountProviderLegalEntities));
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }
}