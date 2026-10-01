using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Approvals.InnerApi.LearnerData;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.Approvals.Application.SelectMultiple.Queries;

public class ValidateSelectMultipleLearnerRecordsQueryHandler(
    ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient,
    IReservationApiClient<ReservationApiConfiguration> reservationApiClient,
    IInternalApiClient<LearnerDataInnerApiConfiguration> learnerDataClient,
    IProviderCoursesOrStandardsService providerCoursesService,
    IBulkCourseMetadataService bulkCourseMetadataService,
    IAddCourseTypeDataToCsvService courseTypesToCsvService)
    : IRequestHandler<ValidateSelectMultipleLearnerRecordsQuery, ValidateSelectMultipleLearnerRecordsQueryResult>
{
    public async Task<ValidateSelectMultipleLearnerRecordsQueryResult> Handle(ValidateSelectMultipleLearnerRecordsQuery command, CancellationToken cancellationToken)
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

        var reservationRequests = learnerDataResponse.Body.Select((learner, index) =>
        {
            return new ReservationRequest
            {
                CourseId = learner.TrainingCode,
                AccountLegalEntityId = command.AccountLegalEntityId ?? 0,
                ProviderId = (uint?)command.ProviderId,
                RowNumber = index + 1,
                Id = Guid.NewGuid(),
                StartDate = learner.StartDate,
            };

        }).ToList();
        var reservationValidationResult =
            await reservationApiClient.PostWithResponseCode<BulkReservationValidationResults>(
                new PostValidateReservationRequest(command.ProviderId, reservationRequests));

        if (!string.IsNullOrEmpty(reservationValidationResult.ErrorContent))
        {
            throw new InvalidOperationException($"Validating Reservations Failed, Status Code {reservationValidationResult.StatusCode} Error : {reservationValidationResult.ErrorContent}");
        }

        if (reservationValidationResult.Body == null)
        {
            throw new InvalidOperationException("Validating Reservations Failed, response body was null");
        }

        var providerStandardResults = await providerCoursesService.GetCoursesData(command.ProviderId);

        var uniqueCourseCodes = learnerDataResponse.Body.Select(r => r.TrainingCode).Distinct();
        var otjTrainingHours = await bulkCourseMetadataService.GetOtjTrainingHoursForBulkUploadAsync(uniqueCourseCodes);

        List<BulkUploadAddDraftApprenticeshipRequest> csvRecords = learnerDataResponse.Body.Select((learner, index) =>
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
            };
        }).ToList();

        ValidateSelectMultipleLearnersApiRequest validateSelectMultipleLearnersApiRequest = new ValidateSelectMultipleLearnersApiRequest
        {
            CsvRecords = await courseTypesToCsvService.MapAndAddCourseTypeData(csvRecords),
            ProviderId = command.ProviderId,
            UserInfo = command.UserInfo,
            BulkReservationValidationResults = reservationValidationResult.Body,
            ProviderStandardsData = providerStandardResults,
            OtjTrainingHours = otjTrainingHours
        };

        if (!validateSelectMultipleLearnersApiRequest.ProviderStandardsData.IsMainProvider)
        {
            validateSelectMultipleLearnersApiRequest.ProviderStandardsData.Standards = null;
        }

        var validationResponse = await apiClient.PostWithResponseCode<PostValidateSelectMultipleLearnersResponse>(
            new PostValidateSelectMultipleLearnersRequest(command.ProviderId, validateSelectMultipleLearnersApiRequest));

        if (!string.IsNullOrEmpty(validationResponse.ErrorContent))
        {
            throw new InvalidOperationException($"Validating Select Multiple Learners Failed, Status Code {validationResponse.StatusCode} Error : {validationResponse.ErrorContent}");
        }

        if (validationResponse.Body == null)
        {
            throw new InvalidOperationException("Validating Select Multiple Learners Failed, response body was null");
        }

        return new ValidateSelectMultipleLearnerRecordsQueryResult
        {
            ValidationErrors = validationResponse.Body.ValidationErrors ?? []
        };
    }
}
