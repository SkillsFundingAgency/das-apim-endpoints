using SFA.DAS.Apim.Shared.Interfaces;

using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.RequestApprenticeTraining
{
    public class ExpireEmployerRequestsRequest : IPutApiRequest
    {
        public object Data { get; set; }
        public string PutUrl => $"api/employer-requests/expire";
    }
}
