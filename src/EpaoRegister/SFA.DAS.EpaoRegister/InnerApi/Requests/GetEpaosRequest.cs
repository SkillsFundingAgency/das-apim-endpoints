using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.EpaoRegister.InnerApi.Requests
{
    public class GetEpaosRequest : IGetAllApiRequest
    {
        public string GetAllUrl => "api/ao/assessment-organisations";
    }
}