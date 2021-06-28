using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerDemand.InnerApi.Requests
{
    public class GetUnmetEmployerDemandsRequest : IGetApiRequest
    {
        private readonly uint _ageOfDemandInDays;

        public GetUnmetEmployerDemandsRequest (uint ageOfDemandInDays)
        {
            _ageOfDemandInDays = ageOfDemandInDays;
        }

        public string GetUrl => $"api/Demand/unmet?ageOfDemandInDays={_ageOfDemandInDays}";
    }
}