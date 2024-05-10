using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.LegacyApi.Requests
{
    public class GetLegacyApplicationsByEmailApiRequest(string email) : IGetApiRequest
    {
        public string GetUrl => $"api/apprenticeship/{email}";
    }
}
