using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindApprenticeshipTraining.Api.ApiRequests;
using SFA.DAS.FindApprenticeshipTraining.Api.Models;
using SFA.DAS.FindApprenticeshipTraining.Application.Shortlist.Commands.CreateShortlistForUser;
using SFA.DAS.FindApprenticeshipTraining.Application.Shortlist.Queries.GetShortlistForUser;
using SFA.DAS.SharedOuterApi.Infrastructure;

namespace SFA.DAS.FindApprenticeshipTraining.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class ShortlistController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<LocationsController> _logger;

        public ShortlistController (IMediator mediator, ILogger<LocationsController> logger)
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

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateShortlistForUser(CreateShortListRequest shortlistRequest)
        {
            try
            {
                await _mediator.Send(new CreateShortlistForUserCommand
                {
                    Lat = shortlistRequest.Lat,
                    Lon = shortlistRequest.Lon,
                    Ukprn = shortlistRequest.Ukprn,
                    LocationDescription = shortlistRequest.LocationDescription,
                    StandardId = shortlistRequest.StandardId,
                    SectorSubjectArea = shortlistRequest.SectorSubjectArea,
                    ShortlistUserId = shortlistRequest.ShortlistUserId
                });

                return Created("", null);
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
    }
}