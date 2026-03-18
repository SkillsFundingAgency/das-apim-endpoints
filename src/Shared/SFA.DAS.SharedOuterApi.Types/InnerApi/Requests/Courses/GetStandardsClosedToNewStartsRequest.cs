using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Courses
{
    public class GetStandardsClosedToNewStartsRequest : IGetApiRequest
    {
        public string GetUrl => "api/courses/standards?filter=ClosedToNewStarts";
    }
}