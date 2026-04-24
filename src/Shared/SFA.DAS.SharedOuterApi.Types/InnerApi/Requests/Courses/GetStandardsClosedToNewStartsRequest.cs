using SFA.DAS.Apim.Shared.Interfaces;

using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Courses
{
    public class GetStandardsClosedToNewStartsRequest : IGetApiRequest
    {
        public string GetUrl => "api/courses/standards?filter=ClosedToNewStarts";
    }
}