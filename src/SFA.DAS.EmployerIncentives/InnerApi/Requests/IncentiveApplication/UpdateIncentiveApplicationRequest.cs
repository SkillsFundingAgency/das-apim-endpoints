using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests.IncentiveApplication
{
    public class UpdateIncentiveApplicationRequest : IPutApiRequest<UpdateIncentiveApplicationRequestData>
    {
        public string BaseUrl { get; set; }
        public string PutUrl => $"{BaseUrl}applications";
        public UpdateIncentiveApplicationRequestData Data { get; set; }
    }
}
