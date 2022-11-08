using SFA.DAS.SharedOuterApi.Interfaces;

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
