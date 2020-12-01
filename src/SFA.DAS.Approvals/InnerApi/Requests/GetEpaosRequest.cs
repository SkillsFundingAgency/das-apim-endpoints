using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests
{
    public class GetEpaosRequest : IGetAllApiRequest
    {
        public string GetAllUrl => "api/v1/organisations";
    }
}