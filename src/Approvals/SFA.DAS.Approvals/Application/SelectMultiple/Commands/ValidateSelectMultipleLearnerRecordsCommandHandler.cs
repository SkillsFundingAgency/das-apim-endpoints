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

namespace SFA.DAS.Approvals.Application.SelectMultiple.Commands;

public class ValidateSelectMultipleLearnerRecordsCommandHandler(
    ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient,
    IReservationApiClient<ReservationApiConfiguration> reservationApiClient,
    IInternalApiClient<LearnerDataInnerApiConfiguration> learnerDataClient,
    IProviderCoursesOrStandardsService providerCoursesService,
    IBulkCourseMetadataService bulkCourseMetadataService,
    IAddCourseTypeDataToCsvService courseTypesToCsvService)
    : IRequestHandler<ValidateSelectMultipleLearnerRecordsCommand>
{
    public async Task Handle(ValidateSelectMultipleLearnerRecordsCommand command, CancellationToken cancellationToken)
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

        var reservationRequests = learnerDataResponse.Body.Select(learner =>
        {
            return new ReservationRequest
            {
                CourseId = learner.TrainingCode,
                AccountLegalEntityId = command.AccountLegalEntityId ?? 0, 
                ProviderId = (uint?)command.ProviderId,
                RowNumber = (int)learner.Uln,//fix 
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

        ValidateSelectMultipleLearnersApiRequest validateSelectMultipleLearnersApiRequest = new ValidateSelectMultipleLearnersApiRequest    
        {
            CsvRecords = await courseTypesToCsvService.MapAndAddCourseTypeData(command.CsvRecords),
            ProviderId = command.ProviderId,            
            UserInfo = command.UserInfo,
            BulkReservationValidationResults = reservationValidationResult.Body,
            ProviderStandardsData = providerStandardResults,
            OtjTrainingHours = otjTrainingHours
        };

        //if (!validateSelectMultipleLearnersApiRequest.ProviderStandardsData.IsMainProvider)
        //{
        //    validateSelectMultipleLearnersApiRequest.ProviderStandardsData.Standards = null;
        //}

        await apiClient.PostWithResponseCode<object>(
            new PostValidateSelectMultipleLearnersRequest(command.ProviderId, validateSelectMultipleLearnersApiRequest));
        //return Unit.Value;
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