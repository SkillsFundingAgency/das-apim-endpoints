using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Recruit.Api.Models;
using SFA.DAS.Recruit.Api.Models.Users;
using SFA.DAS.Recruit.Application.Queries.GetUserAccounts;
using SFA.DAS.Recruit.Application.User.Commands.UpsertUser;
using SFA.DAS.Recruit.Application.User.Queries.GetUserByEmail;
using System;
using System.Threading.Tasks;
using SFA.DAS.Recruit.Application.User.Queries.GetUserByDfeUserId;
using SFA.DAS.Recruit.Application.User.Queries.GetUserByIdamsId;
using SFA.DAS.Recruit.Application.User.Queries.GetUsersByEmployerAccountId;
using SFA.DAS.Recruit.Application.User.Queries.GetUsersByProviderUkprn;

namespace SFA.DAS.Recruit.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class UsersController(IMediator mediator, ILogger<UsersController> logger) : ControllerBase
    {
        [HttpPost]
        [Route("by/email")]
        public async Task<IActionResult> GetByEmailId([FromBody] GetUserRequest request)
        {
            try
            {
                var queryResult = await mediator.Send(new GetUserByEmailQuery(request.Email, request.UserType));

                return Ok(queryResult);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error getting user by email");
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("by/idams/{idams}")]
        public async Task<IActionResult> GetByIdams([FromRoute] string idams)
        {
            try
            {
                var queryResult = await mediator.Send(new GetUserByIdamsIdQuery(idams));

                return Ok(queryResult);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error getting user by idams id");
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("by/dfEUserId/{dfEUserId}")]
        public async Task<IActionResult> GetByDfEUserId([FromRoute] string dfEUserId)
        {
            try
            {
                var queryResult = await mediator.Send(new GetUserByDfeUserIdQuery(dfEUserId));

                return Ok(queryResult);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error getting user by DfEUserId");
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("by/employerAccountId/{employerAccountId:long}")]
        public async Task<IActionResult> GetByEmployerAccountId([FromRoute] long employerAccountId)
        {
            try
            {
                var queryResult = await mediator.Send(new GetUsersByEmployerAccountIdQuery(employerAccountId));

                return Ok(queryResult);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error getting users by employerAccountId");
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("by/ukprn/{ukprn:long}")]
        public async Task<IActionResult> GetByUkprn([FromRoute] long ukprn)
        {
            try
            {
                var queryResult = await mediator.Send(new GetUsersByProviderUkprnQuery(ukprn));

                return Ok(queryResult);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error getting users by ukprn");
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("{userId}/accounts")]
        public async Task<IActionResult> GetAccountsByUserId(string userId)
        {
            try
            {
                var queryResult = await mediator.Send(new GetUserAccountsQuery {UserId = userId});

                var returnModel = new GetAccountsResponse
                {
                    HashedAccountIds = queryResult.HashedAccountIds
                };

                return Ok(returnModel);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error getting account by id");
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("{id}")]
        public async Task<IActionResult> UpsertUser([FromRoute] Guid id, [FromBody]UserDto userDto)
        {
            try
            {
                await mediator.Send(new UpsertUserCommand()
                {
                    User = (InnerApi.Recruit.Requests.UserDto)userDto,
                    Id = id
                });
                return Created();
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error occured while upserting user");
                return new StatusCodeResult(500);
            }
        }
    }
}