using System;

namespace SFA.DAS.Approvals.InnerApi.Responses;

public class GetAccountReservationsResponse
{
    public Guid Id { get; set; }
    public long AccountLegalEntityId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public int Status { get; set; }
    public long? TransferSenderAccountId { get; set; }
}
