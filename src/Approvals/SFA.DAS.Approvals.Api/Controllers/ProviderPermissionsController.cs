using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.Application.ProviderPermissions.Queries;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ProviderRelationships;

namespace SFA.DAS.Approvals.Api.Controllers;

[ApiController]
[Route("[controller]/")]
public class ProviderPermissionsController(ISender mediator, ILogger<ProviderPermissionsController> logger) : Controller﻿
{
    [HttpGet]
    [Route("has-relationship-with-permission")]
    public async Task<IActionResult> HasRelationshipWithPermission([FromQuery] long ukprn, CancellationToken cancellationToken)
    {
        try
        {
            var result = await mediator.Send(new GetHasRelationshipWithPermissionQuery(ukprn), cancellationToken);
            return Ok(new GetHasRelationshipWithPermissionResponse { HasPermission = result });
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "{Method} threw an exception.", nameof(HasRelationshipWithPermission));
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpGet]
    [Route("account-provider-legal-entities")]
    public async Task<IActionResult> AccountProviderLegalEntities([FromQuery] int ukprn, CancellationToken cancellationToken)
    {
        try
        {
            var result =
                await mediator.Send(new GetAccountProviderLegalEntitiesQuery(ukprn), cancellationToken);
            return Ok(result);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "{Method} threw an exception.", nameof(AccountProviderLegalEntities));
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }
}
