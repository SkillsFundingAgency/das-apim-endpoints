using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests.IncentiveApplication
{
    public class ConfirmIncentiveApplicationRequest : IPatchApiRequest<ConfirmIncentiveApplicationRequestData>
    {
        public ConfirmIncentiveApplicationRequestData Data { get; set; }
        public string PatchUrl => $"applications/{Data.IncentiveApplicationId}";
    }
}
