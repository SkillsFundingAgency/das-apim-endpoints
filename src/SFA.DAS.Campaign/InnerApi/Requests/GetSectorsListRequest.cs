using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Campaign.InnerApi.Requests
{
    public class GetSectorsListRequest : IGetApiRequest
    {
        public string GetUrl => "api/courses/sectors";
    }
}