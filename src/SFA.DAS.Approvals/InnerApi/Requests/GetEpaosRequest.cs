using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests
{
    public class GetEpaosRequest : IGetApiRequest
    {
        public string GetUrl => "api/v1/organisations";
    }
}