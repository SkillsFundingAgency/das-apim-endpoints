using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EpaoRegister.InnerApi.Requests
{
    public class GetEpaoRequest : IGetApiRequest
    {
        public GetEpaoRequest(string epaoId)
        {
            EpaoId = epaoId;
        }

        public string EpaoId { get; }
        public string GetUrl => $"api/ao/assessment-organisations/{EpaoId}";
    }
}