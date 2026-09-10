using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.Approvals.Application.SelectMultiple.Commands;

public class ValidateSelectMultipleLearnerRecordsCommandHandler(
    ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient,
    IReservationApiClient<ReservationApiConfiguration> reservationApiClient,
    IProviderStandardsService providerStandardsService,
    IBulkCourseMetadataService bulkCourseMetadataService,
    IAddCourseTypeDataToCsvService courseTypesToCsvService)
    : IRequestHandler<ValidateSelectMultipleLearnerRecordsCommand>
{
    public async Task Handle(ValidateSelectMultipleLearnerRecordsCommand command, CancellationToken cancellationToken)
    {
        var reservationRequests = command.Learners.Select(learner =>
        {
            //Guid.TryParse(command.UserInfo.UserId, out var parsedUserId);
            return new ReservationRequest
            {
                CourseId = response.CourseCode,
                AccountLegalEntityId = command.AccountLegalEntityId ?? 0, 
                ProviderId = (uint?)command.ProviderId,
                RowNumber = response.RowNumber,
                Id = Guid.NewGuid(),
                StartDate = learner.StartDate,
                //TransferSenderAccountId = response.TransferSenderId ?!? could it be transfer sender for multiselect, we don't have cohort at this point, previous check were ignoring it when no cohort id 
            };

        }).ToList();
        var reservationValidationResult =
            await reservationApiClient.PostWithResponseCode<BulkReservationValidationResults>(
                new PostValidateReservationRequest(command.ProviderId, reservationRequests));

        //use couses table 
        var providerStandardResults = await providerStandardsService.GetCoursesData(command.ProviderId);

        var uniqueCourseCodes = command.CsvRecords.Select(r => r.CourseCode).Distinct();
        var otjTrainingHours = await bulkCourseMetadataService.GetOtjTrainingHoursForBulkUploadAsync(uniqueCourseCodes);

        BulkUploadValidateApiRequest bulkUploadValidateApiRequest = new BulkUploadValidateApiRequest
        {
            CsvRecords = await courseTypesToCsvService.MapAndAddCourseTypeData(command.CsvRecords),
            ProviderId = command.ProviderId,
            LogId = command.FileUploadLogId,
            UserInfo = command.UserInfo,
            BulkReservationValidationResults = reservationValidationResult.Body,
            ProviderStandardsData = providerStandardResults,
            OtjTrainingHours = otjTrainingHours
        };

        if (!bulkUploadValidateApiRequest.ProviderStandardsData.IsMainProvider)
        {
            bulkUploadValidateApiRequest.ProviderStandardsData.Standards = null;
        }

        await apiClient.PostWithResponseCode<object>(
            new PostValidateBulkUploadRequest(command.ProviderId, bulkUploadValidateApiRequest));
        return Unit.Value;
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