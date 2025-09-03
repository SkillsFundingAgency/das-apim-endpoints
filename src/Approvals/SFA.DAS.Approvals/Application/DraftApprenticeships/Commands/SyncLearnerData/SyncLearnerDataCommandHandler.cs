using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;
using SFA.DAS.Approvals.InnerApi.LearnerData;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.Approvals.Application.Shared.Enums;

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

            // 1. Get the draft apprenticeship to find the learnerDataId
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

            // 2. Get the latest learner data from das-learner-data-api
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

            // 3. Create an update request by merging learner data with existing draft apprenticeship
            var updateRequest = new UpdateDraftApprenticeshipRequest
            {
                UserInfo = request.UserInfo,
                RequestingParty = SFA.DAS.Approvals.Application.Shared.Enums.Party.Provider,

                // Overwrite with learner data values
                FirstName = learnerData.FirstName,
                LastName = learnerData.LastName,
                Email = learnerData.Email,
                DateOfBirth = learnerData.Dob,
                Uln = learnerData.Uln.ToString(),
                StartDate = learnerData.StartDate,
                EndDate = learnerData.PlannedEndDate,
                Cost = learnerData.TrainingPrice,
                TrainingPrice = learnerData.TrainingPrice,
                EndPointAssessmentPrice = learnerData.EpaoPrice,
                EmploymentPrice = null, // Not available in learner data
                CourseCode = learnerData.StandardCode.ToString(),
                CourseOption = string.Empty, // Not available in learner data
                DeliveryModel = learnerData.IsFlexiJob ? SFA.DAS.Approvals.InnerApi.DeliveryModel.PortableFlexiJob : SFA.DAS.Approvals.InnerApi.DeliveryModel.Regular,
                Reference = draftApprenticeship.ProviderReference, // Keep existing provider reference
                ReservationId = draftApprenticeship.ReservationId,
                IgnoreStartDateOverlap = false,
                IsOnFlexiPaymentPilot = draftApprenticeship.IsOnFlexiPaymentPilot,
                MinimumAgeAtApprenticeshipStart = 16,
                MaximumAgeAtApprenticeshipStart = 120,
                IsLearnerDataSync = true // This will trigger the flag clearing in das-commitments
            };

            // 4. Update the draft apprenticeship (this will run validation and clear flags if successful)
            var updateResponse = await commitmentsApiClient.PutWithResponseCode<NullResponse>(
                new PutUpdateDraftApprenticeshipRequest(request.CohortId, request.DraftApprenticeshipId, updateRequest));

            if (!string.IsNullOrEmpty(updateResponse.ErrorContent))
            {
                logger.LogWarning("Validation failed when updating draft apprenticeship. Status: {StatusCode}, Error: {Error}", 
                    updateResponse.StatusCode, updateResponse.ErrorContent);
                
                // Validation failed - don't reset the flags so banner will still appear
                return new SyncLearnerDataCommandResult
                {
                    Success = false,
                    Message = "Learner data validation failed. Please review the apprenticeship details and correct any errors."
                };
            }

            logger.LogInformation("Successfully synced learner data for draft apprenticeship {DraftApprenticeshipId}", 
                request.DraftApprenticeshipId);

            return new SyncLearnerDataCommandResult
            {
                Success = true,
                Message = "Learner data has been successfully updated"
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
