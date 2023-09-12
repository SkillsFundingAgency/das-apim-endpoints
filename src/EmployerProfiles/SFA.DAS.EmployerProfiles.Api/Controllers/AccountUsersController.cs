using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerProfiles.Api.Models;
using SFA.DAS.EmployerProfiles.Application.AccountUsers.Queries;
using System;
using System.Net;
using System.Threading.Tasks;
using SFA.DAS.EmployerProfiles.Application.AccountUsers.Commands;

namespace SFA.DAS.EmployerProfiles.Api.Controllers
{
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

        /// <summary>
        /// Endpoint to add & update the employer information.
        /// </summary>
        /// <param name="userId">Email address/Gov Unique Identifier.</param>
        /// <param name="request">model UpsertAccountRequest.</param>
        /// <returns>UpsertUserApiResponse.</returns>
        [HttpPut]
        [Route("{userId}/upsert-user")]
        public async Task<IActionResult> UpsertUserAccount(string userId, [FromBody] UpsertAccountRequest request)
        {
            try
            {
                var result = await _mediator.Send(new UpsertAccountCommand
                {
                    UserId = userId,
                    GovIdentifier = request.GovIdentifier,
                    Email = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    CorrelationId = request.CorrelationId
                });

                return Ok((UpsertUserApiResponse)result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}