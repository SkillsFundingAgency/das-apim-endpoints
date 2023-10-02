using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.Requests
{
    public class GetRoutesRequest : IGetApiRequest
    {
        public string GetUrl => "/api/courses/routes";
    }
}