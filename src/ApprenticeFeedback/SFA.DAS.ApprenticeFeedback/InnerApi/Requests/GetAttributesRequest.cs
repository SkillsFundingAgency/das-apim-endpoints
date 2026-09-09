using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.ApprenticeFeedback.InnerApi.Requests
{
    public class GetAttributesRequest : IGetApiRequest
    {
        public string GetUrl => $"api/attributes/{AttributeType}";

        public string AttributeType { get; internal set; }

        public GetAttributesRequest(string attributeType)
        {
            AttributeType = attributeType;
        }
    }
}
