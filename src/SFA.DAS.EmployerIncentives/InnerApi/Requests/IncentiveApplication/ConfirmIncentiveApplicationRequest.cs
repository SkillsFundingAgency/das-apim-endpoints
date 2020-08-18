using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests.IncentiveApplication
{
    public class ConfirmIncentiveApplicationRequest : IPatchApiRequest<ConfirmIncentiveApplicationRequestData>
    {
        public string BaseUrl { get; set; }
        public ConfirmIncentiveApplicationRequestData Data { get; set; }
        public string PatchUrl => $"{BaseUrl}applications/{Data.IncentiveApplicationId}";
    }
}
