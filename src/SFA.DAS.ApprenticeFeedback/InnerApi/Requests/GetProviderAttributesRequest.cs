using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeFeedback.InnerApi.Requests
{
    public class GetProviderAttributesRequest : IGetApiRequest
    {
        public string GetUrl => "api/providerattributes";
    }
}
