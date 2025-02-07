using System.ComponentModel.DataAnnotations;
using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ToolsSupport.Api.Models.EmployerAccount;
using SFA.DAS.ToolsSupport.Application.Queries.EmployerAccount.GetEmployerAccountDetails;
using SFA.DAS.ToolsSupport.Models.Constants;

namespace SFA.DAS.ToolsSupport.Api.Controllers;

[Route("[controller]/")]
[ApiController]
public class EmployerAccountController(IMediator mediator, ILogger logger) : ControllerBase
{
    [HttpGet]
    [Route("{accountId}/accountdetails")]
    public async Task<IActionResult> GetAccountDetails(long accountId, [FromQuery, Required] AccountFieldSelection accountFieldSelection)
    {
        try
        {
            var result = await mediator.Send(new GetEmployerAccountDetailsQuery
            {
              AccountId = accountId,
              SelectedField = accountFieldSelection

            });

            return Ok((GetEmployerAccountDetailsResponse)result);
        }
        catch (Exception e)
        {
            logger.LogError("Error: GetAccountDetails: {error}", e);
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }
}