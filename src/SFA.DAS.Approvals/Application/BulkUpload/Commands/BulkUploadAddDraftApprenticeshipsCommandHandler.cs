using MediatR;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Approvals.Application.BulkUpload.Commands
{
    public class BulkUploadAddDraftApprenticeshipsCommandHandler : IRequestHandler<BulkUploadAddDraftApprenticeshipsCommand, GetBulkUploadAddDraftApprenticeshipsResult>
    {
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _apiClient;
        private readonly IReservationApiClient<ReservationApiConfiguration> _reservationApiClient;

        public BulkUploadAddDraftApprenticeshipsCommandHandler(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient, IReservationApiClient<ReservationApiConfiguration> reservationApiClient)
        {
            _apiClient = apiClient;
            _reservationApiClient = reservationApiClient;
        }

        public async Task<GetBulkUploadAddDraftApprenticeshipsResult> Handle(BulkUploadAddDraftApprenticeshipsCommand command, CancellationToken cancellationToken)
        {
            ApiResponse<BulkCreateReservationsWithNonLevyResult> reservationResult = await GetReservations(command);
            MergeReservationWithDraftApprenticeships(command.BulkUploadAddDraftApprenticeships, reservationResult);

            var dataToSend = new BulkUploadAddDraftApprenticeshipsRequest
            {
                BulkUploadDraftApprenticeships = command.BulkUploadAddDraftApprenticeships,
                ProviderId = command.ProviderId,
                UserInfo = command.UserInfo,
                BulkReservationValidationResults = new BulkReservationValidationResults { ValidationErrors = reservationResult.Body.ValidationErrors }
            };

            var result = await _apiClient.PostWithResponseCode<GetBulkUploadAddDraftApprenticeshipsResponse>(
                new PostAddDraftApprenticeshipsRequest(command.ProviderId, dataToSend));

            result.EnsureSuccessStatusCode();

            return new GetBulkUploadAddDraftApprenticeshipsResult
            {
                BulkUploadAddDraftApprenticeshipsResponse = result.Body.BulkUploadAddDraftApprenticeshipsResponse.Select(x => (BulkUploadAddDraftApprenticeshipsResult)x)
            };
        }

        private void MergeReservationWithDraftApprenticeships(IEnumerable<BulkUploadAddDraftApprenticeshipRequest> bulkUploadAddDraftApprenticeships, ApiResponse<BulkCreateReservationsWithNonLevyResult> reservationResult)
        {
            reservationResult.Body.BulkCreateResults.ForEach(x => bulkUploadAddDraftApprenticeships.First(y => y.Uln == x.ULN).ReservationId = x.ReservationId);
        }

        private async Task<ApiResponse<BulkCreateReservationsWithNonLevyResult>> GetReservations(BulkUploadAddDraftApprenticeshipsCommand command)
        {
            var reservationRequests = command.BulkUploadAddDraftApprenticeships.Select(x =>
            {
                Guid.TryParse(command.UserInfo.UserId, out var parsedUserId);
                return new BulkCreateReservations
                {
                    CourseId = x.CourseCode,
                    AccountLegalEntityId = x.LegalEntityId ?? 0,
                    ProviderId = (uint?)x.ProviderId,
                    RowNumber = x.RowNumber,
                    Id = Guid.NewGuid(),
                    StartDate = GetStartDate(x.StartDateAsString),
                    TransferSenderAccountId = x.TransferSenderId,
                    ULN = x.Uln
                };
            }).ToList();

            var reservationResult = await _reservationApiClient.PostWithResponseCode<BulkCreateReservationsWithNonLevyResult>(new PostBulkCreateReservationRequest(command.ProviderId, reservationRequests));
            return reservationResult;
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
