using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Recruit.Api.Models;
using SFA.DAS.Recruit.Application.Queries.GetUserAccounts;
using SFA.DAS.Recruit.Application.User.Commands.UpsertUser;

namespace SFA.DAS.Recruit.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IMediator mediator, ILogger<UsersController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Route("{userId}/accounts")]
        public async Task<IActionResult> GetAccountsByUserId(string userId)
        {
            try
            {
                var queryResult = await _mediator.Send(new GetUserAccountsQuery {UserId = userId});

                var returnModel = new GetAccountsResponse
                {
                    HashedAccountIds = queryResult.HashedAccountIds
                };

                return Ok(returnModel);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting account by id");
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("{id}")]
        public async Task<IActionResult> UpsertUser([FromRoute] Guid id, [FromBody]UserDto userDto)
        {
            try
            {
                await _mediator.Send(new UpsertUserCommand()
                {
                    User = (InnerApi.Requests.UserDto)userDto,
                    Id = id
                });
                return Created();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occured while upserting user");
                return new StatusCodeResult(500);
            }
        }
    }
}