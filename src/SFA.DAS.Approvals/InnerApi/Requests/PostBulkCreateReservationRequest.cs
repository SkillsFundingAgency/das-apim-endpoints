using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;

namespace SFA.DAS.Approvals.InnerApi.Requests
{
    public class PostBulkCreateReservationRequest : IPostApiRequest
    {
        public readonly long ProviderId;
        public object Data { get; set; }
        public string PostUrl => $"/api/Reservations/accounts/bulk-create";

        public PostBulkCreateReservationRequest(long providerId, List<BulkCreateReservations> listOfReservations )
        {
            var reservationsToCreate = new BulkCreateReservationsWithNonLevyRequest();
            reservationsToCreate.Reservations = listOfReservations;

            ProviderId = providerId;
            Data = reservationsToCreate;
        }
    }
}
