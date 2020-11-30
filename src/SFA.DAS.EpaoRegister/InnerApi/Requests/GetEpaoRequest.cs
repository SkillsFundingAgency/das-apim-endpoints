using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EpaoRegister.InnerApi.Requests
{
    public class GetEpaoRequest : IGetAllApiRequest
    {
        public string EpaoId { get; set; }
        public string GetAllUrl => $"api/v1/organisationsearch/organisations?searchTerm={EpaoId}&pageSize=10&pageIndex=1";
    }
}