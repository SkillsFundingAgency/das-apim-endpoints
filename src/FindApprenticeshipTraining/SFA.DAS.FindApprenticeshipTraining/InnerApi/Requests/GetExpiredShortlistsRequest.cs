using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests
{
    public class GetExpiredShortlistsRequest : IGetApiRequest
    {
        private readonly uint _expiryInDays;

        public GetExpiredShortlistsRequest(uint expiryInDays)
        {
            _expiryInDays = expiryInDays;
        }

        public string GetUrl => $"api/Shortlist/users/expired?expiryInDays={_expiryInDays}";
    }
}