using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EpaoRegister.InnerApi.Requests
{
    public class GetEpaoRequest : IGetApiRequest
    {
        public string EpaoId { get; set; }
        public string GetUrl => $"api/ao/assessment-organisations/{EpaoId}";
    }
}