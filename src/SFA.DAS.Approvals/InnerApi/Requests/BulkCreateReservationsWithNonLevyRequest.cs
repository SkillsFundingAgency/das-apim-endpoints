using System.Collections.Generic;

namespace SFA.DAS.Approvals.InnerApi.Requests
{
    public class BulkCreateReservationsWithNonLevyRequest
    {
        public List<BulkCreateReservations> Reservations { get; set; }
    }
}
