using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerDemand.InnerApi.Requests
{
    public class GetEmployerDemandsOlderThan3YearsRequest : IGetApiRequest
    {
        public string GetUrl => "api/demand/older-than-3-years";
    }
}