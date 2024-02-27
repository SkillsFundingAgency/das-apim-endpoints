using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerAccounts.Api.Models;
using SFA.DAS.EmployerAccounts.Application.Commands.AddProviderDetailsFromInvitation;
using SFA.DAS.EmployerAccounts.Application.Queries.AccountUsers.Queries;

namespace SFA.DAS.EmployerAccounts.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class AccountUsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountUsersController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        [Route("{userId}/accounts")]
        public async Task<IActionResult> GetUserAccounts(string userId, [FromQuery] string email)
        {
            try
            {
                var result = await _mediator.Send(new GetAccountsQuery
                {
                    UserId = userId,
                    Email = email
                });

                return Ok((UserAccountsApiResponse)result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("{userId}/add-provider-details-from-invitation")]
        public async Task<IActionResult> AddProviderDetailsFromInvitation(string userId, [FromBody] AddProviderDetailsPostRequest request)
        {
            try
            {
                var result = await _mediator.Send(new AddProviderDetailsFromInvitationCommand
                {
                    UserId = userId,
                    CorrelationId = request.CorrelationId,
                    AccountId = request.AccountId,
                    Email = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName
                });

                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}