using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.Apprenticeships.InnerApi
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