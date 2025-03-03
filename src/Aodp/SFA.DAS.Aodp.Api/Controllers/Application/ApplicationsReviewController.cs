using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Aodp.Application.Commands.Application.Review;
using SFA.DAS.Aodp.Application.Queries.Application.Review;

namespace SFA.DAS.Aodp.Api.Controllers.Application;

[ApiController]
[Route("api/[controller]")]
public class ApplicationsReviewController : BaseController
{
    public ApplicationsReviewController(IMediator mediator, ILogger<ApplicationsReviewController> logger) : base(mediator, logger)
    { }

    [HttpPost("/api/application-reviews")]
    [ProducesResponseType(typeof(GetApplicationsForReviewQueryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetApplicationReviews(GetApplicationsForReviewQuery query)
    {
        return await SendRequestAsync(query);
    }

    [HttpPut("/api/application-reviews/{applicationReviewId}/share")]
    [ProducesResponseType(typeof(EmptyResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateApplicationReviewSharing(Guid applicationReviewId, UpdateApplicationReviewSharingCommand command)
    {
        command.ApplicationReviewId = applicationReviewId;
        return await SendRequestAsync(command);
    }
}
