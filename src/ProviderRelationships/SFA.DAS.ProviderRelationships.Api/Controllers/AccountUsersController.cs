using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ProviderRelationships.Api.Models;
using SFA.DAS.ProviderRelationships.Application.AccountUsers.Queries;

namespace SFA.DAS.ProviderRelationships.Api.Controllers;

[ApiController]
[Route("[controller]/")]
public class AccountUsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public AccountUsersController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    [Route("{userId}/accounts")]
    public async Task<IActionResult> GetUserAccounts(string userId, [FromQuery] string? email)
    {
        try
        {
            var result = await _mediator.Send(new GetAccountsQuery
            {
                UserId = userId,
                Email = email
            });

            return Ok((GetUserAccountsApiResponse) result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new StatusCodeResult((int) HttpStatusCode.InternalServerError);
        }
    }
}