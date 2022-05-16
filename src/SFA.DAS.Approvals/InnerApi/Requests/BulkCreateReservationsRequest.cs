using System.Collections.Generic;

namespace SFA.DAS.Approvals.InnerApi.Requests
{
    public class BulkCreateReservationsRequest
    {
        public List<BulkCreateReservations> Reservations { get; set; }
    }
}
