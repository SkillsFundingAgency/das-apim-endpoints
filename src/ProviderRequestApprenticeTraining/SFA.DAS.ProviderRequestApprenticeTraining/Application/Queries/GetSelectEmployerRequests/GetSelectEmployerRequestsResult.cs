using SFA.DAS.ProviderRequestApprenticeTraining.InnerApi.Responses;
using System.Collections.Generic;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetSelectEmployerRequests
{
    public class GetSelectEmployerRequestsResult
    {
        public IEnumerable<GetSelectEmployerRequestsResponse> SelectEmployerRequests { get; set; }
    }
}
