using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerDemand.InnerApi.Requests
{
    public class GetUnmetEmployerDemandsRequest : IGetApiRequest
    {
        private readonly uint _ageOfDemandInDays;
        private readonly int? _courseId;

        public GetUnmetEmployerDemandsRequest (uint ageOfDemandInDays, int? courseId = null)
        {
            _ageOfDemandInDays = ageOfDemandInDays;
            _courseId = courseId;
        }

        public string GetUrl => $"api/Demand/unmet?ageOfDemandInDays={_ageOfDemandInDays}&courseId={_courseId}";
    }
}