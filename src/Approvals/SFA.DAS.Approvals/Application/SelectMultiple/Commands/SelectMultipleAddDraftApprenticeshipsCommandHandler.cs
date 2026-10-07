using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.Approvals.Application.SelectMultiple.Queries;
using SFA.DAS.Approvals.InnerApi.LearnerData;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.Approvals.Application.SelectMultiple.Commands;

public class SelectMultipleAddDraftApprenticeshipsCommandHandler(
    ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient,
    IReservationApiClient<ReservationApiConfiguration> reservationApiClient,
    IInternalApiClient<LearnerDataInnerApiConfiguration> learnerDataClient,
    IMediator mediator,
    IAddCourseTypeDataToCsvService courseTypesToCsvService = null)
    : IRequestHandler<SelectMultipleAddDraftApprenticeshipsCommand, SelectMultipleAddDraftApprenticeshipsResult>
{
    public async Task<SelectMultipleAddDraftApprenticeshipsResult> Handle(SelectMultipleAddDraftApprenticeshipsCommand command, CancellationToken cancellationToken)
    {
        await Validate(command, cancellationToken);
        var cohort = await CreateCohort(command);
        var learnerDetails = await GetLearnerDetails(command);
        ApiResponse<BulkCreateReservationsWithNonLevyResult> reservationResult = await GetReservations(command, learnerDetails);
        var bulkUploadAddDraftApprenticeships = FormatDataToBulkUpload(command, learnerDetails, cohort);
        MergeReservationWithDraftApprenticeships(bulkUploadAddDraftApprenticeships, reservationResult);

        var dataToSend = new BulkUploadAddDraftApprenticeshipsRequest
        {
            BulkUploadDraftApprenticeships = await courseTypesToCsvService.MapAndAddCourseTypeData(bulkUploadAddDraftApprenticeships),
            ProviderId = command.ProviderId,            
            UserInfo = command.UserInfo
        };

        var result = await apiClient.PostWithResponseCode<GetBulkUploadAddDraftApprenticeshipsResponse>(
            new PostAddDraftApprenticeshipsRequest(command.ProviderId, dataToSend));

        result.EnsureSuccessStatusCode();

        return new GetBulkUploadAddDraftApprenticeshipsResult
        {
            BulkUploadAddDraftApprenticeshipsResponse = result.Body.BulkUploadAddDraftApprenticeshipsResponse.Select(x => (BulkUploadAddDraftApprenticeshipsResult)x)
        };
    }

    private async Task<CreateCohortResponse> CreateCohort(SelectMultipleAddDraftApprenticeshipsCommand command)
    {
        var result = await apiClient.PostWithResponseCode<CreateCohortResponse>(
            new PostCreateEmptyCohortRequest(new CreateEmptyCohortRequest
            {
                UserInfo = command.UserInfo,
                AccountId = command.AccountId,
                AccountLegalEntityId = command.AccountLegalEntityId ?? 0,
                ProviderId = command.ProviderId
            }));

        result.EnsureSuccessStatusCode();

        return result.Body;
    }

    private List<BulkUploadAddDraftApprenticeshipRequest> FormatDataToBulkUpload(SelectMultipleAddDraftApprenticeshipsCommand command, List<LearnerDataRecord> learnerDetails, CreateCohortResponse cohort)
    {
        return learnerDetails.Select((learner, index) =>
        {
            return new BulkUploadAddDraftApprenticeshipRequest
            {
                RowNumber = index + 1,
                Uln = learner.Uln.ToString(),
                FirstName = learner.FirstName,
                LastName = learner.LastName,
                DateOfBirthAsString = learner.Dob.ToString("yyyy-MM-dd"),
                Email = learner.Email,
                CourseCode = learner.TrainingCode,
                StartDateAsString = learner.StartDate.ToString("yyyy-MM-dd"),
                EndDateAsString = learner.PlannedEndDate.ToString("yyyy-MM-dd"),
                ProviderId = command.ProviderId,
                CostAsString = (learner.TrainingPrice + learner.EpaoPrice).ToString(),
                AgreementId = command.AgreementId,
                LegalEntityId = command.AccountLegalEntityId,
                CohortId = cohort.CohortId,
                CohortRef = cohort.CohortReference
            };
        }).ToList();
    }

    private async Task<List<LearnerDataRecord>> GetLearnerDetails(SelectMultipleAddDraftApprenticeshipsCommand command)
    {
        var learnerIds = command.LearnerIds?.Distinct().ToList() ?? [];

        if (learnerIds.Count == 0)
        {
            throw new ArgumentException("LearnerIds must not be empty", nameof(command.LearnerIds));
        }

        var learnerDataResponse = await learnerDataClient.PostWithResponseCode<List<LearnerDataRecord>>(
           new PostGetLearnersForProviderByIdsRequest(
               command.ProviderId, new GetLearnersForProviderByIdsRequest
               {
                   LearnerIds = learnerIds
               }
           ));

        if (!string.IsNullOrEmpty(learnerDataResponse.ErrorContent))
        {
            throw new InvalidOperationException($"Getting Learner Data Failed, Status Code {learnerDataResponse.StatusCode} Error : {learnerDataResponse.ErrorContent}");
        }

        if (learnerDataResponse.Body == null || learnerDataResponse.Body.Count != learnerIds.Count)
        {
            throw new InvalidOperationException($"Getting Learner Data Failed, expected {learnerIds.Count} learners but received {learnerDataResponse.Body?.Count ?? 0}");
        }

        return learnerDataResponse.Body;
    }

    private async Task Validate(SelectMultipleAddDraftApprenticeshipsCommand command, CancellationToken cancellationToken)
    {
        var validateCmd = new ValidateSelectMultipleLearnerRecordsQuery
        {
            LearnerIds = command.LearnerIds,
            ProviderId = command.ProviderId,
            UserInfo = command.UserInfo
        };

        await mediator.Send(validateCmd, cancellationToken);
    }

    private void MergeReservationWithDraftApprenticeships(IEnumerable<BulkUploadAddDraftApprenticeshipRequest> bulkUploadAddDraftApprenticeshipRequests, ApiResponse<BulkCreateReservationsWithNonLevyResult> reservationResult)
    {
        reservationResult.Body.BulkCreateResults.ForEach(x => bulkUploadAddDraftApprenticeshipRequests.First(y => y.Uln.ToString() == x.ULN).ReservationId = x.ReservationId);
    }

    private async Task<ApiResponse<BulkCreateReservationsWithNonLevyResult>> GetReservations(SelectMultipleAddDraftApprenticeshipsCommand command, List<LearnerDataRecord> learnerDetails)
    {
        var reservationRequests = learnerDetails.Select((learner, index) =>
        {
            Guid.TryParse(command.UserInfo.UserId, out var parsedUserId);
            return new BulkCreateReservations
            {
                CourseId = learner.TrainingCode,
                AccountLegalEntityId = command.AccountLegalEntityId ?? 0,
                ProviderId = (uint?)command.ProviderId,
                RowNumber = index + 1,
                Id = Guid.NewGuid(),
                StartDate = learner.StartDate,
                ULN = learner.Uln.ToString()
            };
        }).ToList();

        var reservationResult = await reservationApiClient.PostWithResponseCode<BulkCreateReservationsWithNonLevyResult>(new PostBulkCreateReservationRequest(command.ProviderId, reservationRequests));
        reservationResult.EnsureSuccessStatusCode();
        return reservationResult;
    }
}
