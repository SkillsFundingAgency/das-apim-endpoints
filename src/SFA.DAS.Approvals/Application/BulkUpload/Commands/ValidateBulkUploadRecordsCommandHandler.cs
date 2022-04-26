using MediatR;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
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
            var reservationRequests = command.CsvRecords.Select(x => (ReservationRequest)x).ToList();
            var reservationValidationResult = await _reservationApiClient.PostWithResponseCode<BulkReservationValidationResults>(new PostValidateReservationRequest(command.ProviderId, reservationRequests));
       
            // If any errors this call will throw a bulkupload domain exception, which is handled through middleware.
            await _apiClient.PostWithResponseCode<object>(new PostValidateBulkUploadRequest(command.ProviderId,
                 new BulkUploadValidateApiRequest
                 {
                     CsvRecords = command.CsvRecords,
                     ProviderId = command.ProviderId,
                     UserInfo = command.UserInfo,
                     ReservationValidationResults = reservationValidationResult.Body
                 }));
            return Unit.Value;
        }
    }
}
