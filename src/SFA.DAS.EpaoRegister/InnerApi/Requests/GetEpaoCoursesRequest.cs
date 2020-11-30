using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EpaoRegister.InnerApi.Requests
{
    public class GetEpaoCoursesRequest : IGetAllApiRequest
    {
        public string EpaoId { get; set; }
        public string GetAllUrl => $"api/v1/standards/{EpaoId}";
    }
}