using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests
{
    public class GetStandardsClosedToNewStartsRequest : IGetApiRequest
    {
        public string GetUrl => "api/courses/standards?filter=ClosedToNewStarts";
    }
}