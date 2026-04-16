using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerFeedback.InnerApi.Requests
{
    public class GetAttributesRequest : IGetApiRequest
    {
        public string GetUrl => "api/attributes";

    }
}
