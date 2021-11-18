using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests
{
    public class GetEpaosStandardVersionsRequest : IGetAllApiRequest
    {
        public GetEpaosStandardVersionsRequest(string id)
        {
            organisationId = id;
        }
        public string organisationId { get; }
        public string GetAllUrl => $"api/ao/assessment-organisations/{organisationId}/standards";

    }
}