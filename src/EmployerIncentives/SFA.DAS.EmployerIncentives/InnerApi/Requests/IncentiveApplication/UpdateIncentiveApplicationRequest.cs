using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests.IncentiveApplication
{
    public class UpdateIncentiveApplicationRequest : IPutApiRequest<UpdateIncentiveApplicationRequestData>
    {
        public string PutUrl => $"applications/{Data.IncentiveApplicationId}";
        public UpdateIncentiveApplicationRequestData Data { get; set; }
    }
}
