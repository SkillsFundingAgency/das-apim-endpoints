using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Approvals.Api.Models;
using SFA.DAS.Approvals.Application.ProviderPermissions.Queries;

namespace SFA.DAS.Approvals.Api.Controllers;

[ApiController]
[Route("[controller]/")]
public class ProviderPermissionsController(ISender mediator) : Controller
{
    [HttpGet]
    [Route("has-permission")]
    public async Task<IActionResult> HasPermission([FromQuery] long? ukPrn, [FromQuery] long? accountLegalEntityId, [FromQuery] string operation)
    {
        try
        {
            var result = await mediator.Send(new GetHasPermissionQuery(ukPrn, accountLegalEntityId, operation));
            return Ok(new ProviderPermissionResponse { HasPermission = result });
        }
        catch (Exception)
        {
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }
}