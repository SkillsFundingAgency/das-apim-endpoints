using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests.IncentiveApplication
{
    public class CreateIncentiveApplicationRequest : IPostApiRequest
    {
        public string PostUrl => "applications";
        public object Data { get; set; }
    }
}