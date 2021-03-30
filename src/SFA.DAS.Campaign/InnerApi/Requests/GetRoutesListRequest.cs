using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Campaign.InnerApi.Requests
{
    public class GetRoutesListRequest : IGetApiRequest
    {
        public string GetUrl => "api/courses/routes";
    }
}