using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.Approvals.Application.BulkUpload.Commands
{
    public class BulkUploadAddAndApproveDraftApprenticeshipsCommandHandler(
        ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient,
        IReservationApiClient<ReservationApiConfiguration> reservationApiClient,
        IMediator mediator,
        IAddCourseTypeDataToCsvService courseTypesToCsvService)
        : IRequestHandler<BulkUploadAddAndApproveDraftApprenticeshipsCommand,
            BulkUploadAddAndApproveDraftApprenticeshipsResult>
    {
        public async Task<BulkUploadAddAndApproveDraftApprenticeshipsResult> Handle(BulkUploadAddAndApproveDraftApprenticeshipsCommand command, CancellationToken cancellationToken)
        {
            await Validate(command, cancellationToken);
            ApiResponse<BulkCreateReservationsWithNonLevyResult> reservationResult = await GetReservations(command);
            MergeReservationWithDraftApprenticeships(command.BulkUploadAddAndApproveDraftApprenticeships, reservationResult);

            var dataToSend = new BulkUploadAddAndApproveDraftApprenticeshipsRequest
            {
                BulkUploadAddAndApproveDraftApprenticeships = await courseTypesToCsvService.MapAndAddCourseTypeData(command.BulkUploadAddAndApproveDraftApprenticeships.ToList()),
                ProviderId = command.ProviderId,
                LogId = command.FileUploadLogId,
                UserInfo = command.UserInfo
            };

            var result = await apiClient.PostWithResponseCode<BulkUploadAddAndApproveDraftApprenticeshipsResponse>(
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

            await mediator.Send(validateCmd, cancellationToken);
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

            var reservationResult = await reservationApiClient.PostWithResponseCode<BulkCreateReservationsWithNonLevyResult>(new PostBulkCreateReservationRequest(command.ProviderId, reservationRequests));
            reservationResult.EnsureSuccessStatusCode();
            return reservationResult;
        }
    }
}
