using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests
{
    public class GetNotYetApprovedStandardsRequest : IGetApiRequest
    {
        public string GetUrl => "api/courses/standards?filter=NotYetApproved";
    }
}