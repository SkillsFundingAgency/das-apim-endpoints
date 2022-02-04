using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApimDeveloper.Api.ApiRequests;
using SFA.DAS.ApimDeveloper.Api.ApiResponses;
using SFA.DAS.ApimDeveloper.Application.Users.Commands.ActivateUser;
using SFA.DAS.ApimDeveloper.Application.Users.Commands.AuthenticateUser;
using SFA.DAS.ApimDeveloper.Application.Users.Commands.ChangePassword;
using SFA.DAS.ApimDeveloper.Application.Users.Commands.CreateUser;
using SFA.DAS.ApimDeveloper.Application.Users.Commands.SendEmailToChangePassword;
using SFA.DAS.ApimDeveloper.Application.Users.Queries.GetUser;
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

        [HttpPut]
        [Route("{id}/activate")]
        public async Task<IActionResult> ActivateAccount([FromRoute] Guid id)
        {
            try
            {
                await _mediator.Send(new ActivateUserCommand
                {
                    Id = id
                });
                
                return NoContent();
            }
            catch (HttpRequestContentException e)
            {
                return StatusCode((int) e.StatusCode, e.ErrorContent);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unable to activate user");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("authenticate")]
        public async Task<IActionResult> AuthenticateUser(AuthenticateUserRequest request)
        {
            try
            {
                var result = await _mediator.Send(new AuthenticateUserCommand
                {
                    Email = request.Email,
                    Password = request.Password
                });

                var model = (UserAuthenticationApiResponse)result;

                if (model == null)
                {
                    return NotFound();
                }

                return Ok(model);
            }
            catch (HttpRequestContentException e)
            {
                return StatusCode((int) e.StatusCode, e.ErrorContent);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unable to get user");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
        
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetUser([FromQuery]string email)
        {
            try
            {
                var result = await _mediator.Send(new GetUserQuery
                {
                    Email = email
                });

                var model = (UserApiResponse)result;

                if (model == null)
                {
                    return NotFound();
                }

                return Ok(model);
            }
            catch (HttpRequestContentException e)
            {
                return StatusCode((int) e.StatusCode, e.ErrorContent);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unable to get user");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
        
        [HttpPost]
        [Route("{id}/send-change-password-email")]
        public async Task<IActionResult> SendChangePasswordEmail([FromRoute] Guid id, [FromBody] SendChangePasswordEmailRequest request)
        {
            try
            {
                await _mediator.Send(new SendEmailToChangePasswordCommand
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    ChangePasswordUrl = request.ChangePasswordUrl
                });

                return Created("", new {id});
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Unable to send change password email for userid: [{id}]");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
        
        [HttpPut]
        [Route("{id}/change-password")]
        public async Task<IActionResult> ChangePassword([FromRoute] Guid id, [FromBody] ChangePasswordRequest request)
        {
            try
            {
                await _mediator.Send(new ChangePasswordCommand
                {
                    Id = id,
                    Password = request.Password
                });

                return NoContent();
            }
            catch (HttpRequestContentException e)
            {
                return StatusCode((int) e.StatusCode, e.ErrorContent);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Unable to change password for userid: [{id}]");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}