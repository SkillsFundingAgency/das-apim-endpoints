using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Apim.Shared.Exceptions;
using SFA.DAS.Assessor.Api.Models.Users;
using SFA.DAS.Assessor.Application.Commands.CreateOrUpdateUser;
using SFA.DAS.Assessor.Application.Queries.GetUser;

namespace SFA.DAS.Assessor.Api.Controllers
{
    [ApiController]
    [Route("users")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UsersController> _logger;

        public UsersController(
            IMediator mediator,
            ILogger<UsersController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(typeof(CreateOrUpdateUserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateOrUpdateUser(
            [FromBody] CreateOrUpdateUserRequest request,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _mediator.Send(
                    (CreateOrUpdateUserCommand)request,
                    cancellationToken);

                return Ok((CreateOrUpdateUserResponse)result);
            }
            catch (ApiResponseException ex)
                when (ex.Status == HttpStatusCode.BadRequest)
            {
                return new ContentResult
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ContentType = "application/json",
                    Content = ex.Error
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error attempting to create or update user.");

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{dfeSignInUserId}")]
        [ProducesResponseType(typeof(GetUserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUser(
            [FromRoute, Required, StringLength(255)] string dfeSignInUserId,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _mediator.Send(new GetUserQuery
                {
                    DfeSignInUserId = dfeSignInUserId
                }, cancellationToken);

                if (result.User == null)
                {
                    return NotFound();
                }

                return Ok((GetUserResponse)result.User);
            }
            catch (ApiResponseException ex)
                when (ex.Status == HttpStatusCode.BadRequest)
            {
                return new ContentResult
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ContentType = "application/json",
                    Content = ex.Error
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error attempting to retrieve user.");

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}