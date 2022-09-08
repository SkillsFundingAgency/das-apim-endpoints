using SFA.DAS.SharedOuterApi.InnerApi.Responses.ProviderEvent;

namespace SFA.DAS.Approvals.Application.CommitmentPayment.Queries.GetDataLockEvents
{
    public class GetDataLockEventsQueryResult
    {
        public PageOfResults<GetDataLockEventsResponse> PagedDataLockEvent { get; set; }
    }
}