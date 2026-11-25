using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests;

public class GetCourseEpaosStandardVersionsRequest(string id, int larsCode) : IGetApiRequest
{
    public string organisationId { get; } = id;
    public int LarsCode { get; } = larsCode;
    public string GetUrl => $"api/v1/standard-version/standards/epao/{organisationId}/{LarsCode}";

}