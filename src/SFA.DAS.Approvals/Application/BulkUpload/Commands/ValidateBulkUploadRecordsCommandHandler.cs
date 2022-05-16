﻿using MediatR;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Approvals.Application.BulkUpload.Commands
{
    public class ValidateBulkUploadRecordsCommandHandler : IRequestHandler<ValidateBulkUploadRecordsCommand>
    {
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _apiClient;
        private readonly IReservationApiClient<ReservationApiConfiguration> _reservationApiClient;

        public ValidateBulkUploadRecordsCommandHandler(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient, IReservationApiClient<ReservationApiConfiguration> reservationApiClient)
        {
            _apiClient = apiClient;
            _reservationApiClient = reservationApiClient;
        }

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
            
            var reservationValidationResult = await _reservationApiClient.PostWithResponseCode<BulkReservationValidationResults>(new PostValidateReservationRequest(command.ProviderId, reservationRequests));
       
            // If any errors this call will throw a bulkupload domain exception, which is handled through middleware.
            await _apiClient.PostWithResponseCode<object>(new PostValidateBulkUploadRequest(command.ProviderId,
                 new BulkUploadValidateApiRequest
                 {
                     CsvRecords = command.CsvRecords,
                     ProviderId = command.ProviderId,
                     UserInfo = command.UserInfo,
                     BulkReservationValidationResults = reservationValidationResult.Body
                 }));
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
}
