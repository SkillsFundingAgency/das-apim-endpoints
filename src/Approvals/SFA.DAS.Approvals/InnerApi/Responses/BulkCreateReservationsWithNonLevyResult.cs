﻿using System;
using System.Collections.Generic;

namespace SFA.DAS.Approvals.InnerApi.Responses
{
    public class BulkCreateReservationsWithNonLevyResult
    {
        public List<BulkCreateReservationResult> BulkCreateResults { get; set; } = new List<BulkCreateReservationResult>();
    }

    public class BulkCreateReservationResult
    {
        public string ULN { get; set; }
        public Guid ReservationId { get; set; }
    }
}
