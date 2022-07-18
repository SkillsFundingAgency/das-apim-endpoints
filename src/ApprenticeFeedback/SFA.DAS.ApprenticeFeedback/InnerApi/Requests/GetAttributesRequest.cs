using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeFeedback.InnerApi.Requests
{
    public class GetAttributesRequest : IGetApiRequest
    {
        public string GetUrl => "api/attributes";
    }
}
