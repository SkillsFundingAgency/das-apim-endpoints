using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.LevyTransferMatching.InnerApi.Requests.Reference
{
    public class GetLevelsRequest : IGetAllApiRequest
    {
        public string GetAllUrl => "reference/levels";
    }
}
