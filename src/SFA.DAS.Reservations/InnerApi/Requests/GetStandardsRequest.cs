using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Reservations.InnerApi.Requests
{
    public class GetStandardsRequest : IGetApiRequest
    {
        public string GetUrl => $"api/courses/standards";
    }
}