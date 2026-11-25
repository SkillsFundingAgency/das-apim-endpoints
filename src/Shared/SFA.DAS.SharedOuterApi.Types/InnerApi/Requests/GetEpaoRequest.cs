using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests;

public class GetEpaoRequest(string epaoId) : IGetApiRequest
{
    public string EpaoId { get; } = epaoId;
    public string GetUrl => $"api/ao/assessment-organisations/{EpaoId}";
}