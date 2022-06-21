using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests
{
    public class GetCourseEpaosStandardVersionsRequest : IGetApiRequest
    {
        public GetCourseEpaosStandardVersionsRequest(string id, int larsCode)
        {
            organisationId = id;
            LarsCode = larsCode;
        }
        public string organisationId { get; }
        public int LarsCode { get; }
        public string GetUrl => $"api/v1/standard-version/standards/epao/{organisationId}/{LarsCode}";

    }
}