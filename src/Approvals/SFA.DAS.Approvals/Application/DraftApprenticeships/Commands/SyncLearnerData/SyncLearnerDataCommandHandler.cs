using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.InnerApi;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;
using SFA.DAS.Approvals.InnerApi.LearnerData;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.DraftApprenticeships.Commands.SyncLearnerData;

public abstract class SyncLearnerDataCommandHandler(
    ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> commitmentsApiClient,
    IInternalApiClient<LearnerDataInnerApiConfiguration> learnerDataClient,
    ITrainingProgrammeResolutionService trainingProgrammeResolutionService,
    ILogger<SyncLearnerDataCommandHandler> logger)
    : IRequestHandler<SyncLearnerDataCommand, GetDraftApprenticeshipResponse>
{
    public async Task<GetDraftApprenticeshipResponse> Handle(SyncLearnerDataCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Syncing learner data for draft apprenticeship {DraftApprenticeshipId} in cohort {CohortId}", 
            request.DraftApprenticeshipId, request.CohortId);

        var draftApprenticeshipResponse = await commitmentsApiClient.GetWithResponseCode<GetDraftApprenticeshipResponse>(
            new GetDraftApprenticeshipRequest(request.CohortId, request.DraftApprenticeshipId));

        if (draftApprenticeshipResponse.StatusCode != System.Net.HttpStatusCode.OK)
        {
            logger.LogWarning("Failed to get draft apprenticeship. Status: {StatusCode}, Error: {Error}", 
                draftApprenticeshipResponse.StatusCode, draftApprenticeshipResponse.ErrorContent);
            throw new LearnerDataSyncException("Failed to retrieve draft apprenticeship details");
        }

        var draftApprenticeship = draftApprenticeshipResponse.Body;
        if (draftApprenticeship.LearnerDataId == null)
        {
            logger.LogWarning("Draft apprenticeship {DraftApprenticeshipId} has no learnerDataId", request.DraftApprenticeshipId);
            throw new LearnerDataSyncException("No learner data associated with this apprenticeship");
        }

        var learnerDataResponse = await learnerDataClient.GetWithResponseCode<GetLearnerForProviderResponse>(
            new GetLearnerForProviderRequest(request.ProviderId, draftApprenticeship.LearnerDataId.Value));

        if (learnerDataResponse.StatusCode != System.Net.HttpStatusCode.OK)
        {
            logger.LogWarning("Failed to get learner data. Status: {StatusCode}, Error: {Error}", 
                learnerDataResponse.StatusCode, learnerDataResponse.ErrorContent);
            throw new LearnerDataSyncException("Failed to retrieve learner data");
        }

        var learnerData = learnerDataResponse.Body;

        var (courseCode, trainingCourseName, trainingCourseVersion, trainingCourseOption, standardUId) =
            await ResolveCourseFields(learnerData);

        var updatedDraftApprenticeship = new GetDraftApprenticeshipResponse
        {
            Id = draftApprenticeship.Id,
            FirstName = learnerData.FirstName,
            LastName = learnerData.LastName,
            Email = draftApprenticeship.Email,
            Uln = draftApprenticeship.Uln,
            CourseCode = courseCode,
            DeliveryModel = learnerData.IsFlexiJob ? DeliveryModel.FlexiJobAgency : DeliveryModel.Regular,
            TrainingCourseName = trainingCourseName,
            TrainingCourseVersion = trainingCourseVersion,
            TrainingCourseOption = trainingCourseOption,
            TrainingCourseVersionConfirmed = draftApprenticeship.TrainingCourseVersionConfirmed,
            StandardUId = standardUId,
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

        return updatedDraftApprenticeship;
    }

    private async Task<(string CourseCode, string TrainingCourseName, string TrainingCourseVersion, string TrainingCourseOption, string StandardUId)> ResolveCourseFields(GetLearnerForProviderResponse learnerData)
    {
        var isApprenticeshipUnit = string.Equals(learnerData.LearningType, "ApprenticeshipUnit", StringComparison.OrdinalIgnoreCase);
        var courseCode = isApprenticeshipUnit ? learnerData.TrainingCode ?? string.Empty : learnerData.StandardCode.ToString();
        var courseName = learnerData.TrainingName ?? string.Empty;

        if (string.IsNullOrWhiteSpace(courseCode) || (!isApprenticeshipUnit && learnerData.StandardCode <= 0))
        {
            return (courseCode, courseName, string.Empty, string.Empty, string.Empty);
        }

        var isStandard = int.TryParse(courseCode, out var standardId) && standardId > 0;
        var response = await trainingProgrammeResolutionService.GetTrainingProgrammeAsync(courseCode, learnerData.StartDate);
        var programme = response?.TrainingProgramme;

        if (programme == null)
        {
            return (courseCode, courseName, string.Empty, string.Empty, string.Empty);
        }

        var resolvedCourseCode = programme.CourseCode ?? courseCode;
        var resolvedCourseName = !string.IsNullOrWhiteSpace(courseName) ? courseName : programme.Name ?? string.Empty;
        var version = isStandard ? programme.Version ?? string.Empty : string.Empty;
        var option = isStandard && programme.Options?.Count > 0 ? programme.Options[0] : string.Empty;
        var standardUId = isStandard ? programme.StandardUId ?? string.Empty : string.Empty;

        return (resolvedCourseCode, resolvedCourseName, version, option, standardUId);
    }
}