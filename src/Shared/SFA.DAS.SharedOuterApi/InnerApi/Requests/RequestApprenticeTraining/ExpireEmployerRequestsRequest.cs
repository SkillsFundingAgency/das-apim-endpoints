using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.RequestApprenticeTraining
{
    public class ExpireEmployerRequestsRequest : IPutApiRequest
    {
        public object Data { get; set; }
        public string PutUrl => $"api/employer-requests/expire";
    }
}
