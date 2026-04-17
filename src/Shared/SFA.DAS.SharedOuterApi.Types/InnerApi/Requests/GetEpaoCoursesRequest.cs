using SFA.DAS.Apim.Shared.Interfaces;

using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests
{
    public class GetEpaoCoursesRequest : IGetAllApiRequest
    {
        public GetEpaoCoursesRequest(string epaoId)
        {
            EpaoId = epaoId;
        }

        public string EpaoId { get; }
        public string GetAllUrl => $"api/ao/assessment-organisations/{EpaoId}/standards";
    }
}