using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerProfiles.Api.Models;
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
        private readonly ILogger<AccountUsersController> _logger;

        public AccountUsersController(IMediator mediator, ILogger<AccountUsersController> logger)
        {
            _mediator = mediator;
            _logger = logger;
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
                _logger.LogError(e, "Error occurred while upserting user account for userId: {UserId}", userId);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}