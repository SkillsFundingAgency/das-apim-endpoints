using System.Collections.Generic;

namespace SFA.DAS.Approvals.InnerApi.Responses
{
    public class BulkReservationValidationResults
    {
        public ICollection<BulkReservationValidation> ValidationErrors { get; set; }
    }

    public class BulkReservationValidation
    {
        public string Reason { get; set; }

        public int RowNumber { get; set; }
    }
}
