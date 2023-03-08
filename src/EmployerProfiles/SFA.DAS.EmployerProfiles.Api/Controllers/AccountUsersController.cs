using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerProfiles.Api.Models;
using SFA.DAS.EmployerProfiles.Application.AccountUsers.Commands.UpsertEmployer;
using SFA.DAS.EmployerProfiles.Application.AccountUsers.Queries;
using SFA.DAS.SharedOuterApi.Infrastructure;

namespace SFA.DAS.EmployerProfiles.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class AccountUsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountUsersController (IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpGet]
        [Route("{userId}/accounts")]
        public async Task<IActionResult> GetUserAccounts(string userId, [FromQuery]string email)
        {
            try
            {
                var result = await _mediator.Send(new GetAccountsQuery
                {
                    UserId = userId,
                    Email = email
                });

                return Ok((UserAccountsApiResponse) result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new StatusCodeResult((int) HttpStatusCode.InternalServerError);
            }
        }

        [HttpPut]
        [Route("{id}/upsert-user")]
        public async Task<IActionResult> UpsertUserAccount([FromQuery] Guid id, [FromBody] UpsertAccountRequest request)
        {
            try
            {
                var result = await _mediator.Send(new UpsertAccountCommand
                {
                    Id = id,
                    Email = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    GovIdentifier = request.GovIdentifier
                });

                return Ok(result);
            }
            catch (HttpRequestContentException e)
            {
                return StatusCode((int)e.StatusCode, e.ErrorContent);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}