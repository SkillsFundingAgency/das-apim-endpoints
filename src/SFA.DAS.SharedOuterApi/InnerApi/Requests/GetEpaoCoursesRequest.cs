using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests
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