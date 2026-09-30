using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.Core;
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
        var learnerDataResponse = await learnerDataClient.PostWithResponseCode<List<LearnerDataRecord>>(
           new PostGetLearnersForProviderByIdsRequest(
               command.ProviderId, new GetLearnersForProviderByIdsRequest
               {
                   LearnerIds = command.LearnerIds
               }
           ));

        if (!string.IsNullOrEmpty(learnerDataResponse.ErrorContent))
        {
            throw new ApplicationException($"Getting Learner Data Failed, Status Code {learnerDataResponse.StatusCode} Error : {learnerDataResponse.ErrorContent}");
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
                //TransferSenderAccountId = response.TransferSenderId ?!? could it be transfer sender for multiselect, we don't have cohort at this point, previous check were ignoring it when no cohort id 
            };

        }).ToList();
        var reservationValidationResult =
            await reservationApiClient.PostWithResponseCode<BulkReservationValidationResults>(
                new PostValidateReservationRequest(command.ProviderId, reservationRequests));


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
                //OriginatorReference = ,
                //EPAOrgId = ,
                CostAsString = learner.TrainingPrice.ToString(),
                AgreementId = learner.AgreementId,
                //CohortRef = ,
                //CohortId = ,
                LegalEntityId = command.AccountLegalEntityId,
                //TransferSenderId = , ??
                //RecognisePriorLearningAsString = ,
                //TrainingTotalHoursAsString = ,
                //TrainingHoursReductionAsString = ,
                //IsDurationReducedByRPLAsString = ,
                //DurationReducedByAsString = ,
                //PriceReducedByAsString = 
            };
        }).ToList();

        ValidateSelectMultipleLearnersApiRequest validateSelectMultipleLearnersApiRequest = new ValidateSelectMultipleLearnersApiRequest
        {
            CsvRecords = await courseTypesToCsvService.MapAndAddCourseTypeData(csvRecords),
            ProviderId = command.ProviderId,
            //UserInfo = command.UserInfo,
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
        
        return new ValidateSelectMultipleLearnerRecordsQueryResult
        {
            ValidationErrors = validationResponse.Body.ValidationErrors
        };
    }

    public static DateTime? GetStartDate(string date, string format = "yyyy-MM-dd")
    {
        if (!string.IsNullOrWhiteSpace(date) &&
            DateTime.TryParseExact(date, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime outDateTime))
        {
            return new DateTime(outDateTime.Year, outDateTime.Month, 1);
        }

        return null;
    }
}