using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LevyTransferMatching.InnerApi.Requests.Reference
{
    public class GetSectorsRequest : IGetAllApiRequest
    {
        public string GetAllUrl => "reference/sectors";
    }
}
