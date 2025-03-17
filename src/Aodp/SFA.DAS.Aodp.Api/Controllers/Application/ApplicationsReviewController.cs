using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Aodp.Application.Commands.Application.Review;
using SFA.DAS.Aodp.Application.Queries.Application.Application;
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

    [HttpGet("/api/application-reviews/{applicationReviewId}")]
    [ProducesResponseType(typeof(GetApplicationForReviewByIdQueryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetApplicationForReviewById(Guid applicationReviewId)
    {
        return await SendRequestAsync(new GetApplicationForReviewByIdQuery(applicationReviewId));
    }

    [HttpGet("/api/application-reviews/{applicationReviewId}/feedback/{userType}")]
    [ProducesResponseType(typeof(GetFeedbackForApplicationReviewByIdQueryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetFeedbackForApplicationReviewById(Guid applicationReviewId, string userType)
    {
        return await SendRequestAsync(new GetFeedbackForApplicationReviewByIdQuery(applicationReviewId, userType));
    }

    [HttpPut("/api/application-reviews/{applicationReviewId}/save-qfau-outcome")]
    [ProducesResponseType(typeof(EmptyResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SaveFundingOfferOutcome(SaveQfauFundingReviewOutcomeCommand command, Guid applicationReviewId)
    {
        command.ApplicationReviewId = applicationReviewId;
        return await SendRequestAsync(command);
    }

    [HttpPut("/api/application-reviews/{applicationReviewId}/save-qfau-offers")]
    [ProducesResponseType(typeof(EmptyResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SaveFundingOffers(SaveQfauFundingReviewOffersCommand command, Guid applicationReviewId)
    {
        command.ApplicationReviewId = applicationReviewId;

        return await SendRequestAsync(command);
    }

    [HttpPut("/api/application-reviews/{applicationReviewId}/save-qfau-offer-details")]
    [ProducesResponseType(typeof(EmptyResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SaveFundingOfferDetails(SaveQfauFundingReviewOffersDetailsCommand command, Guid applicationReviewId)
    {
        command.ApplicationReviewId = applicationReviewId;

        return await SendRequestAsync(command);
    }

    [HttpGet("/api/application-reviews/{applicationReviewId}/details")]
    [ProducesResponseType(typeof(GetApplicationDetailsByIdQueryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetApplicationDetailsByIdAsync(Guid applicationReviewId)
    {
        var query = new GetApplicationDetailsByIdQuery(applicationReviewId);
        return await SendRequestAsync(query);
    }
}
 