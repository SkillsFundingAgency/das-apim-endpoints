using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests
{
    public class PostValidateReservationRequest : IPostApiRequest
    {
        public readonly long ProviderId;
        public object Data { get; set; }
        public string PostUrl => "api/Reservations/accounts/bulk-validate";

        public PostValidateReservationRequest(long providerId, List<ReservationRequest> data)
        {
            ProviderId = providerId;
            Data = data;
        }
    }
}
