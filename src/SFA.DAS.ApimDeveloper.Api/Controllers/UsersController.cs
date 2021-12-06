using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApimDeveloper.Api.ApiRequests;
using SFA.DAS.ApimDeveloper.Application.Users.Commands.CreateUser;
using SFA.DAS.SharedOuterApi.Infrastructure;

namespace SFA.DAS.ApimDeveloper.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UsersController> _logger;

        public UsersController (IMediator mediator, ILogger<UsersController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
        [HttpPost]
        [Route("{id}")]
        public async Task<IActionResult> CreateUser([FromRoute]Guid id, [FromBody]CreateUserRequest request)
        {
            try
            {
                await _mediator.Send(new CreateUserCommand
                {
                    Id = id,
                    Email = request.Email,
                    Password = request.Password,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    ConfirmationEmailLink = request.ConfirmationEmailLink
                });

                return new CreatedResult("", null);
            }
            catch (HttpRequestContentException e)
            {
                return StatusCode((int) e.StatusCode, e.ErrorContent);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unable to create user");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

    }
}