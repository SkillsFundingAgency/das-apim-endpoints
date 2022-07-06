using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests
{
    public class GetProviderUsersRequest : IGetAllApiRequest
    {
        public long Ukprn { get; }

        public GetProviderUsersRequest(long ukprn)
        {
            Ukprn = ukprn;
        }
        public string GetAllUrl => $"api/account/{Ukprn}/users";
    }
}