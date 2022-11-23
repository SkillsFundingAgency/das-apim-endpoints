using SFA.DAS.Approvals.InnerApi.Responses.ProviderEvent;

namespace SFA.DAS.Approvals.Application.CommitmentPayment.Queries.GetDataLockEvents
{
    public class GetDataLockStatusesQueryResult
    {
        public PageOfResults<DataLockStatusEvent> PagedDataLockEvent { get; set; }
    }
}