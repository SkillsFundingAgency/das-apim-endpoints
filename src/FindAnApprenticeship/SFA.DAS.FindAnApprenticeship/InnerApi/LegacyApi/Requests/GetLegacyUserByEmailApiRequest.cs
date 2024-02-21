using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.LegacyApi.Requests
{
    public class GetLegacyUserByEmailApiRequest(string email) : IGetApiRequest
    {
        public string GetUrl => $"api/user/{email}";
    }
}
