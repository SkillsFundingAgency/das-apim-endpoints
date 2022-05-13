using MediatR;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Approvals.Application.BulkUpload.Commands
{
    public class BulkUploadAddAndApproveDraftApprenticeshipsCommandHandler : IRequestHandler<BulkUploadAddAndApproveDraftApprenticeshipsCommand, BulkUploadAddAndApproveDraftApprenticeshipsResult>
    {
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _apiClient;
        private readonly IReservationApiClient<ReservationApiConfiguration> _reservationApiClient;
        private readonly IMediator _mediator;

        public BulkUploadAddAndApproveDraftApprenticeshipsCommandHandler(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient, IReservationApiClient<ReservationApiConfiguration> reservationApiClient, IMediator mediator)
        {
            _apiClient = apiClient;
            _reservationApiClient = reservationApiClient;
            _mediator = mediator;
        }

        public async Task<BulkUploadAddAndApproveDraftApprenticeshipsResult> Handle(BulkUploadAddAndApproveDraftApprenticeshipsCommand command, CancellationToken cancellationToken)
        {
            await Validate(command, cancellationToken);
            ApiResponse<BulkCreateReservationsWithNonLevyResult> reservationResult = await GetReservations(command);
            MergeReservationWithDraftApprenticeships(command.BulkUploadAddAndApproveDraftApprenticeships, reservationResult);

            var dataToSend = new BulkUploadAddAndApproveDraftApprenticeshipsRequest
            {
                BulkUploadAddAndApproveDraftApprenticeships = command.BulkUploadAddAndApproveDraftApprenticeships,
                ProviderId = command.ProviderId,
                UserInfo = command.UserInfo
            };

            var result = await _apiClient.PostWithResponseCode<BulkUploadAddAndApproveDraftApprenticeshipsResponse>(
                new PostAddAndApproveDraftApprenticeshipsRequest(command.ProviderId, dataToSend));

            result.EnsureSuccessStatusCode();

            return new BulkUploadAddAndApproveDraftApprenticeshipsResult
            {
                BulkUploadAddAndApproveDraftApprenticeshipResponse = result.Body.BulkUploadAddAndApproveDraftApprenticeshipResponse.Select(x => (BulkUploadAddDraftApprenticeshipsResult)x)
            };
        }

        private async Task Validate(BulkUploadAddAndApproveDraftApprenticeshipsCommand command, CancellationToken cancellationToken)
        {
            var validateCmd = new ValidateBulkUploadRecordsCommand
            {
                CsvRecords = command.BulkUploadAddAndApproveDraftApprenticeships?.ToList(),
                ProviderId = command.ProviderId,
                UserInfo = command.UserInfo
            };

            await _mediator.Send(validateCmd, cancellationToken);
        }

        private void MergeReservationWithDraftApprenticeships(IEnumerable<BulkUploadAddDraftApprenticeshipRequest> bulkUploadAddDraftApprenticeships, ApiResponse<BulkCreateReservationsWithNonLevyResult> reservationResult)
        {
            reservationResult.Body.BulkCreateResults.ForEach(x => bulkUploadAddDraftApprenticeships.First(y => y.Uln == x.ULN).ReservationId = x.ReservationId);
        }

        private async Task<ApiResponse<BulkCreateReservationsWithNonLevyResult>> GetReservations(BulkUploadAddAndApproveDraftApprenticeshipsCommand command)
        {
            var reservationRequests = command.BulkUploadAddAndApproveDraftApprenticeships.Select(x =>
            {
                var result = (BulkCreateReservations)x;
                System.Guid.TryParse(command.UserInfo.UserId, out var parsedUserId);
                result.UserId = parsedUserId;
                return result;
            }).ToList();

            var reservationResult = await _reservationApiClient.PostWithResponseCode<BulkCreateReservationsWithNonLevyResult>(new PostBulkCreateReservationRequest(command.ProviderId, reservationRequests));
            reservationResult.EnsureSuccessStatusCode();
            return reservationResult;
        }
    }
}
