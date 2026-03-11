using MediatR;
using SFA.DAS.Aodp.Application.Commands.Application.Review;
using SFA.DAS.Aodp.Application.Queries.Application.Review;
using SFA.DAS.Aodp.Models;

namespace SFA.DAS.Aodp.Application.Commands.Application.Application;

public class BulkApplicationActionCommandHandler
    : IRequestHandler<BulkApplicationActionCommand, BaseMediatrResponse<BulkApplicationActionCommandResponse>>
{
    private readonly IMediator _mediator;

    public BulkApplicationActionCommandHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<BaseMediatrResponse<BulkApplicationActionCommandResponse>> Handle(
        BulkApplicationActionCommand request,
        CancellationToken cancellationToken)
    {
        var response = new BaseMediatrResponse<BulkApplicationActionCommandResponse>
        {
            Value = new BulkApplicationActionCommandResponse()
        };

        var errors = new List<BulkApplicationActionErrorDto>();
        var updatedCount = 0;

        foreach (var applicationReviewId in request.ApplicationReviewIds)
        {
            var succeeded = false;

            var application = await _mediator.Send(
                new GetApplicationForReviewByIdQuery(applicationReviewId), cancellationToken);

            switch (request.ActionType)
            {
                case BulkApplicationActionType.ShareWithOfqual:
                case BulkApplicationActionType.ShareWithSkillsEngland:

                    var reviewUserType = request.ActionType == BulkApplicationActionType.ShareWithOfqual
                        ? "Ofqual"
                        : "SkillsEngland";

                    var sharingResult = await _mediator.Send(new UpdateApplicationReviewSharingCommand
                    {
                        ApplicationReviewId = applicationReviewId,
                        ApplicationReviewUserType = reviewUserType,
                        ShareApplication = true,
                        UserType = request.UserType,
                        SentByName = request.SentByName,
                        SentByEmail = request.SentByEmail
                    }, cancellationToken);
                    succeeded = sharingResult.Success;
                    break;

                case BulkApplicationActionType.Unlock:

                    var unlockResult = await _mediator.Send(new CreateApplicationMessageCommand
                    {
                        ApplicationId = application.Value.Id,
                        MessageType = ApplicationMessageType.UnlockApplication.ToString(),
                        MessageText = $"Bulk Action {ApplicationMessageType.UnlockApplication}",
                        SentByEmail = request.SentByEmail,
                        SentByName = request.SentByName,
                        UserType = request.UserType
                    }, cancellationToken);
                    succeeded = unlockResult.Success;
                    break;
                default:
                    errors.Add(
                        CreateError(
                            applicationReviewId, application.Value, BulkApplicationActionErrorType.InvalidAction));
                    continue;
            }

            if (!succeeded)
            {
                errors.Add(
                    CreateError(
                        applicationReviewId, application.Value, BulkApplicationActionErrorType.UpdateFailed));
                continue;
            }

            updatedCount++;
        }

        response.Value.RequestedCount = request.ApplicationReviewIds.Count;
        response.Value.UpdatedCount = updatedCount;
        response.Value.ErrorCount = errors.Count;
        response.Value.Errors = errors;

        response.Success = true;
        return response;
    }

    private static BulkApplicationActionErrorDto CreateError(
        Guid applicationReviewId,
        GetApplicationForReviewByIdQueryResponse applicationResponse,
        BulkApplicationActionErrorType errorType)
    {
        return new BulkApplicationActionErrorDto
        {
            ApplicationReviewId = applicationReviewId,
            AwardingOrganisation = applicationResponse.AwardingOrganisation,
            Qan = applicationResponse.Qan,
            Title = applicationResponse.Name,
            ReferenceNumber = applicationResponse.Reference,
            ErrorType = errorType
        };
    }
}
