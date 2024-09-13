using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.RequestApprenticeTraining
{
    public class ExpireEmployerRequestsRequest : IPostApiRequest<ExpireEmployerRequestsData>
    {
        public ExpireEmployerRequestsData Data { get; set; }
        public string PostUrl => $"api/employer-requests/expire";
    }

    public class ExpireEmployerRequestsData
    {
    }
}
