using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.BulkUpload.Commands;

public class ValidateBulkUploadRecordsCommandHandler(
    ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient,
    IReservationApiClient<ReservationApiConfiguration> reservationApiClient,
    IProviderStandardsService providerStandardsService,
    IBulkCourseMetadataService bulkCourseMetadataService,
    IAddCourseTypeDataToCsvService courseTypesToCsvService)
    : IRequestHandler<ValidateBulkUploadRecordsCommand, Unit>
{
    public async Task<Unit> Handle(ValidateBulkUploadRecordsCommand command, CancellationToken cancellationToken)
    {
        var reservationRequests = command.CsvRecords.Select(response =>
        {
            Guid.TryParse(command.UserInfo.UserId, out var parsedUserId);
            return new ReservationRequest
            {
                CourseId = response.CourseCode,
                AccountLegalEntityId = response.LegalEntityId ?? 0,
                ProviderId = (uint?)response.ProviderId,
                RowNumber = response.RowNumber,
                Id = Guid.NewGuid(),
                StartDate = GetStartDate(response.StartDateAsString),
                TransferSenderAccountId = response.TransferSenderId
            };

        }).ToList();
        var reservationValidationResult =
            await reservationApiClient.PostWithResponseCode<BulkReservationValidationResults>(
                new PostValidateReservationRequest(command.ProviderId, reservationRequests));

        var providerStandardResults = await providerStandardsService.GetStandardsData(command.ProviderId);

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