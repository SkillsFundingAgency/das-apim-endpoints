using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApimDeveloper.Api.ApiResponses;
using SFA.DAS.ApimDeveloper.Application.EmployerAccounts.Queries;
using System;
using System.Net;
using System.Threading.Tasks;
using SFA.DAS.ApimDeveloper.Api.ApiRequests;
using SFA.DAS.ApimDeveloper.Application.EmployerAccounts.Commands;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.ApimDeveloper.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class AccountUsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AccountUsersController> _logger;

        public AccountUsersController(IMediator mediator, ILogger<AccountUsersController> logger)
        {
            _mediator = mediator;
            _logger = logger;
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
                    GovIdentifier = userId,
                    Email = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
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