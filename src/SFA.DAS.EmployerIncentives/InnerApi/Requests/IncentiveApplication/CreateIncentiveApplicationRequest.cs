using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests.IncentiveApplication
{
    public class CreateIncentiveApplicationRequest : IPostApiRequest
    {
        public string BaseUrl { get; set; }
        public string PostUrl => $"{BaseUrl}applications";
        public object Data { get; set; }
    }
}