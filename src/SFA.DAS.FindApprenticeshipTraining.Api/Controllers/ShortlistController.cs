using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindApprenticeshipTraining.Api.ApiRequests;
using SFA.DAS.FindApprenticeshipTraining.Api.Models;
using SFA.DAS.FindApprenticeshipTraining.Application.Shortlist.Commands.CreateShortlistForUser;
using SFA.DAS.FindApprenticeshipTraining.Application.Shortlist.Commands.DeleteShortlistForUser;
using SFA.DAS.FindApprenticeshipTraining.Application.Shortlist.Commands.DeleteShortlistItemForUser;
using SFA.DAS.FindApprenticeshipTraining.Application.Shortlist.Queries.GetExpiredShortlists;
using SFA.DAS.FindApprenticeshipTraining.Application.Shortlist.Queries.GetShortlistForUser;
using SFA.DAS.SharedOuterApi.Infrastructure;

namespace SFA.DAS.FindApprenticeshipTraining.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class ShortlistController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ShortlistController> _logger;

        public ShortlistController (IMediator mediator, ILogger<ShortlistController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Route("users/{userId}")]
        public async Task<IActionResult> GetAllForUser(Guid userId)
        {
            try
            {
                var result = await _mediator.Send(new GetShortlistForUserQuery {ShortlistUserId = userId});

                var response = new GetShortlistForUserResponse
                {
                    Shortlist = result.Shortlist.Select(item => (GetShortlistItem)item)
                };
                return Ok(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to get shortlist for user:{userId}");
                return BadRequest();
            }
        }
        
        [HttpGet]
        [Route("expired")]
        public async Task<IActionResult> GetExpiredShortlistUserIds([FromQuery]uint expiryInDays)
        {
            try
            {
                var result = await _mediator.Send(new GetExpiredShortlistsQuery {ExpiryInDays = expiryInDays});

                return Ok(new {result.UserIds});
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting list of expired shortlists");
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateShortlistForUser(CreateShortListRequest shortlistRequest)
        {
            try
            {
                var result = await _mediator.Send(new CreateShortlistForUserCommand
                {
                    Lat = shortlistRequest.Lat,
                    Lon = shortlistRequest.Lon,
                    Ukprn = shortlistRequest.Ukprn,
                    LocationDescription = shortlistRequest.LocationDescription,
                    StandardId = shortlistRequest.StandardId,
                    ShortlistUserId = shortlistRequest.ShortlistUserId
                });

                return Created("", result);
            }
            catch (HttpRequestContentException e)
            {
                return StatusCode((int) e.StatusCode, e.ErrorContent);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error creating shortlist item");
                return BadRequest();
            }
        }

        [HttpDelete]
        [Route("users/{userId}/items/{id}")]
        public async Task<IActionResult> DeleteShortlistItemForUser(Guid id, Guid userId)
        {
            try
            {
                await _mediator.Send(new DeleteShortlistItemForUserCommand
                {
                    Id = id,
                    UserId = userId
                });
                return Accepted();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error deleting shortlist item");
                return BadRequest();
            }
        }
        
        [HttpDelete]
        [Route("users/{userId}")]
        public async Task<IActionResult> DeleteShortlistForUser(Guid userId)
        {
            try
            {
                await _mediator.Send(new DeleteShortlistForUserCommand
                {
                    UserId = userId
                });
                return Accepted();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error deleting shortlist");
                return BadRequest();
            }
        }

    }
}