using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests
{
    public class GetStandardsLookupRequest : IGetApiRequest
    {
        public string GetUrl => "api/courses/standards?filter=None";
    }
}