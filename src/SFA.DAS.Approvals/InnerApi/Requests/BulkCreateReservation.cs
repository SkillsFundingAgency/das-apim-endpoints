using System;

namespace SFA.DAS.Approvals.InnerApi.Requests
{
    public class BulkCreateReservations
    {
        public Guid Id { get; set; }
        public DateTime? StartDate { get; set; }
        public string CourseId { get; set; }
        public uint? ProviderId { get; set; }
        public long? AccountLegalEntityId { get; set; }
        public long? TransferSenderAccountId { get; set; }
        public Guid? UserId { get; set; }
        public string ULN { get; set; }
        public int RowNumber { get; set; }
    }
}
