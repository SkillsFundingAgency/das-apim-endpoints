using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Assessors.InnerApi.Requests
{
    public class GetStandardOptionsListRequest : IGetApiRequest
    {
        public string GetUrl => "api/courses/standards/options";
    }
}
