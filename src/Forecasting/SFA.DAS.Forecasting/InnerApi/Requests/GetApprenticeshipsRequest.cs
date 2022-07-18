using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Forecasting.InnerApi.Requests
{
    public class GetApprenticeshipsRequest : IGetApiRequest
    {
        public long AccountId { get; private set; }
        public string Status { get; private set; }
        public int PageNumber { get; private set; }
        public int PageItemCount { get; private set; }

        public GetApprenticeshipsRequest(long accountId, string status, int pageNumber, int pageItemCount)
        {
            AccountId = accountId;
            Status = status;
            PageNumber = pageNumber;
            PageItemCount = pageItemCount;
        }

        public string GetUrl => $"api/apprenticeships?accountId={AccountId}&Status={Status}&pageNumber={PageNumber}&pageItemCount={PageItemCount}";
    }
}
