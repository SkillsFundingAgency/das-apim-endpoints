using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests;

public class GetEpaoCoursesRequest(string epaoId) : IGetAllApiRequest
{
    public string EpaoId { get; } = epaoId;
    public string GetAllUrl => $"api/ao/assessment-organisations/{EpaoId}/standards";
}