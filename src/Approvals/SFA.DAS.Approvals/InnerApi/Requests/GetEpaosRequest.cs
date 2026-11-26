using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests
{
    public class GetEpaosRequest : IGetAllApiRequest
    {
        public string GetAllUrl => "api/v1/organisations";
    }
}