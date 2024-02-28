using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading.Tasks;
using System;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.UpdateWhatInterestsYou;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.WhatInterestsYou;

namespace SFA.DAS.FindAnApprenticeship.Api.Controllers
{
    [ApiController]
    [Route("applications/{applicationId}/what-interests-you")]
    public class WhatInterestsYouController(IMediator mediator, ILogger<WhatInterestsYouController> logger)
        : Controller
    {
        [HttpGet("")]
        public async Task<IActionResult> GetWhatInterestsYou([FromRoute] Guid applicationId, [FromQuery] Guid candidateId)
        {
            try
            {
                var result = await mediator.Send(new GetWhatInterestsYouQuery
                {
                    ApplicationId = applicationId,
                    CandidateId = candidateId
                });

                return Ok((GetWhatInterestsYouApiResponse)result);
            }
            catch (Exception e)
            {
                logger.LogError(e, "GetWhatInterestsYou : An error occurred");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost("")]
        public async Task<IActionResult> PostWhatInterestsYou([FromRoute] Guid applicationId, [FromBody] PostWhatInterestsYouApiRequest request)
        {
            try
            {
                await mediator.Send(new UpdateWhatInterestsYouCommand
                {
                    ApplicationId = applicationId,
                    CandidateId = request.CandidateId,
                    AnswerText = request.AnswerText,
                    IsComplete = request.IsComplete
                });

                return Created();
            }
            catch (Exception e)
            {
                logger.LogError(e, "PostWhatInterestsYou : An error occurred");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
