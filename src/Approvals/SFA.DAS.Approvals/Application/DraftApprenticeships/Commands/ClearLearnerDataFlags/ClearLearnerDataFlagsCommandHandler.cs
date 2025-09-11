using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Infrastructure;

namespace SFA.DAS.Approvals.Application.DraftApprenticeships.Commands.ClearLearnerDataFlags;

public class ClearLearnerDataFlagsCommandHandler(
    ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> commitmentsApiClient,
    ILogger<ClearLearnerDataFlagsCommandHandler> logger)
    : IRequestHandler<ClearLearnerDataFlagsCommand, Unit>
{
    public async Task<Unit> Handle(ClearLearnerDataFlagsCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Clearing learner data flags for draft apprenticeship {DraftApprenticeshipId} in cohort {CohortId}", 
            request.DraftApprenticeshipId, request.CohortId);

        // Get the current draft apprenticeship to preserve all existing values
        var draftApprenticeshipResponse = await commitmentsApiClient.GetWithResponseCode<GetDraftApprenticeshipResponse>(
            new GetDraftApprenticeshipRequest(request.CohortId, request.DraftApprenticeshipId));

        if (!string.IsNullOrEmpty(draftApprenticeshipResponse.ErrorContent))
        {
            logger.LogError("Failed to get draft apprenticeship for clearing flags. Status: {StatusCode}, Error: {Error}", 
                draftApprenticeshipResponse.StatusCode, draftApprenticeshipResponse.ErrorContent);
            return Unit.Value;
        }

        var draftApprenticeship = draftApprenticeshipResponse.Body;

        // Create an update request with all existing values but clear the learner data flags
        // Note: Since the flags are currently commented out in the model, this won't actually clear them
        // but it provides the structure for when they're uncommented
        var updateRequest = new UpdateDraftApprenticeshipRequest
        {
            UserInfo = new UserInfo { UserId = "System", UserEmail = "system@das.gov.uk", UserDisplayName = "System" },
            RequestingParty = SFA.DAS.Approvals.Application.Shared.Enums.Party.Provider,
            
            // Preserve all existing values
            FirstName = draftApprenticeship.FirstName,
            LastName = draftApprenticeship.LastName,
            Email = draftApprenticeship.Email,
            DateOfBirth = draftApprenticeship.DateOfBirth,
            Uln = draftApprenticeship.Uln,
            CourseCode = draftApprenticeship.CourseCode,
            CourseOption = draftApprenticeship.TrainingCourseOption,
            DeliveryModel = draftApprenticeship.DeliveryModel,
            Cost = draftApprenticeship.Cost,
            TrainingPrice = draftApprenticeship.TrainingPrice,
            EndPointAssessmentPrice = draftApprenticeship.EndPointAssessmentPrice,
            EmploymentPrice = draftApprenticeship.EmploymentPrice,
            StartDate = draftApprenticeship.StartDate,
            ActualStartDate = draftApprenticeship.ActualStartDate,
            EndDate = draftApprenticeship.EndDate,
            EmploymentEndDate = draftApprenticeship.EmploymentEndDate,
            Reference = draftApprenticeship.ProviderReference,
            ReservationId = draftApprenticeship.ReservationId,
            IgnoreStartDateOverlap = false,
            IsOnFlexiPaymentPilot = draftApprenticeship.IsOnFlexiPaymentPilot,
            MinimumAgeAtApprenticeshipStart = 16,
            MaximumAgeAtApprenticeshipStart = 120
        };

        // Update the draft apprenticeship to clear the flags
        var updateResponse = await commitmentsApiClient.PutWithResponseCode<NullResponse>(
            new PutUpdateDraftApprenticeshipRequest(request.CohortId, request.DraftApprenticeshipId, updateRequest));

        if (!string.IsNullOrEmpty(updateResponse.ErrorContent))
        {
            logger.LogWarning("Failed to clear learner data flags. Status: {StatusCode}, Error: {Error}", 
                updateResponse.StatusCode, updateResponse.ErrorContent);
        }
        else
        {
            logger.LogInformation("Successfully cleared learner data flags for draft apprenticeship {DraftApprenticeshipId}", 
                request.DraftApprenticeshipId);
        }

        return Unit.Value;
    }
}
