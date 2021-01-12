using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EpaoRegister.InnerApi.Requests
{
    public class GetEpaosRequest : IGetAllApiRequest
    {
        public string GetAllUrl => "api/ao/assessment-organisations";
    }
}