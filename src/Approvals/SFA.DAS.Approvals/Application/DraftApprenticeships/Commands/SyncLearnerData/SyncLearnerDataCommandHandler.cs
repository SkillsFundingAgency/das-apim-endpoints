using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.InnerApi.LearnerData;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.DraftApprenticeships.Commands.SyncLearnerData;

public class SyncLearnerDataCommandHandler(
    ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> commitmentsApiClient,
    IInternalApiClient<LearnerDataInnerApiConfiguration> learnerDataClient,
    ILogger<SyncLearnerDataCommandHandler> logger)
    : IRequestHandler<SyncLearnerDataCommand, SyncLearnerDataCommandResult>
{
    public async Task<SyncLearnerDataCommandResult> Handle(SyncLearnerDataCommand request, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Syncing learner data for draft apprenticeship {DraftApprenticeshipId} in cohort {CohortId}", 
                request.DraftApprenticeshipId, request.CohortId);

            var draftApprenticeshipResponse = await commitmentsApiClient.GetWithResponseCode<GetDraftApprenticeshipResponse>(
                new GetDraftApprenticeshipRequest(request.CohortId, request.DraftApprenticeshipId));

            if (draftApprenticeshipResponse.StatusCode != System.Net.HttpStatusCode.OK)
            {
                logger.LogWarning("Failed to get draft apprenticeship. Status: {StatusCode}, Error: {Error}", 
                    draftApprenticeshipResponse.StatusCode, draftApprenticeshipResponse.ErrorContent);
                return new SyncLearnerDataCommandResult
                {
                    Success = false,
                    Message = "Failed to retrieve draft apprenticeship details"
                };
            }

            var draftApprenticeship = draftApprenticeshipResponse.Body;
            if (draftApprenticeship.LearnerDataId == null)
            {
                logger.LogWarning("Draft apprenticeship {DraftApprenticeshipId} has no learnerDataId", request.DraftApprenticeshipId);
                return new SyncLearnerDataCommandResult
                {
                    Success = false,
                    Message = "No learner data associated with this apprenticeship"
                };
            }

            var learnerDataResponse = await learnerDataClient.GetWithResponseCode<GetLearnerForProviderResponse>(
                new GetLearnerForProviderRequest(request.ProviderId, draftApprenticeship.LearnerDataId.Value));

            if (learnerDataResponse.StatusCode != System.Net.HttpStatusCode.OK)
            {
                logger.LogWarning("Failed to get learner data. Status: {StatusCode}, Error: {Error}", 
                    learnerDataResponse.StatusCode, learnerDataResponse.ErrorContent);
                return new SyncLearnerDataCommandResult
                {
                    Success = false,
                    Message = "Failed to retrieve learner data"
                };
            }

            var learnerData = learnerDataResponse.Body;

            var updatedDraftApprenticeship = new GetDraftApprenticeshipResponse
            {
                Id = draftApprenticeship.Id,
                FirstName = learnerData.FirstName,
                LastName = learnerData.LastName,
                Email = draftApprenticeship.Email,
                Uln = draftApprenticeship.Uln,
                CourseCode = draftApprenticeship.CourseCode,
                DeliveryModel = draftApprenticeship.DeliveryModel,
                TrainingCourseName = draftApprenticeship.TrainingCourseName,
                TrainingCourseVersion = draftApprenticeship.TrainingCourseVersion,
                TrainingCourseOption = draftApprenticeship.TrainingCourseOption,
                TrainingCourseVersionConfirmed = draftApprenticeship.TrainingCourseVersionConfirmed,
                StandardUId = draftApprenticeship.StandardUId,
                Cost = learnerData.TrainingPrice + learnerData.EpaoPrice,
                TrainingPrice = learnerData.TrainingPrice,
                EndPointAssessmentPrice = learnerData.EpaoPrice,
                StartDate = learnerData.StartDate,
                ActualStartDate = draftApprenticeship.ActualStartDate,
                EndDate = learnerData.PlannedEndDate,
                DateOfBirth = learnerData.Dob,
                Reference = draftApprenticeship.Reference,
                EmployerReference = draftApprenticeship.EmployerReference,
                ProviderReference = draftApprenticeship.ProviderReference,
                ReservationId = draftApprenticeship.ReservationId,
                IsContinuation = draftApprenticeship.IsContinuation,
                ContinuationOfId = draftApprenticeship.ContinuationOfId,
                OriginalStartDate = draftApprenticeship.OriginalStartDate,
                HasStandardOptions = draftApprenticeship.HasStandardOptions,
                EmploymentPrice = draftApprenticeship.EmploymentPrice,
                EmploymentEndDate = draftApprenticeship.EmploymentEndDate,
                RecognisePriorLearning = draftApprenticeship.RecognisePriorLearning,
                DurationReducedBy = draftApprenticeship.DurationReducedBy,
                PriceReducedBy = draftApprenticeship.PriceReducedBy,
                RecognisingPriorLearningStillNeedsToBeConsidered = draftApprenticeship.RecognisingPriorLearningStillNeedsToBeConsidered,
                RecognisingPriorLearningExtendedStillNeedsToBeConsidered = draftApprenticeship.RecognisingPriorLearningExtendedStillNeedsToBeConsidered,
                IsOnFlexiPaymentPilot = draftApprenticeship.IsOnFlexiPaymentPilot,
                EmployerHasEditedCost = draftApprenticeship.EmployerHasEditedCost,
                EmailAddressConfirmed = draftApprenticeship.EmailAddressConfirmed,
                TrainingTotalHours = draftApprenticeship.TrainingTotalHours,
                DurationReducedByHours = draftApprenticeship.DurationReducedByHours,
                IsDurationReducedByRpl = draftApprenticeship.IsDurationReducedByRpl,
                LearnerDataId = draftApprenticeship.LearnerDataId,
                HasLearnerDataChanges = false,
                LastLearnerDataSync = DateTime.UtcNow
            };

            logger.LogInformation("Successfully merged learner data for draft apprenticeship {DraftApprenticeshipId}", 
                request.DraftApprenticeshipId);

            return new SyncLearnerDataCommandResult
            {
                Success = true,
                Message = "Learner data has been successfully merged",
                UpdatedDraftApprenticeship = updatedDraftApprenticeship
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error syncing learner data for draft apprenticeship {DraftApprenticeshipId}", 
                request.DraftApprenticeshipId);
                
            return new SyncLearnerDataCommandResult
            {
                Success = false,
                Message = "An error occurred while syncing learner data"
            };
        }
    }
}
