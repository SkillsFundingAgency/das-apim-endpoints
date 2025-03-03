using SFA.DAS.ProviderRequestApprenticeTraining.InnerApi.Responses;
using System.Collections.Generic;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetEmployerRequestsByIds
{
    public class GetEmployerRequestsByIdsResult
    {
        public IEnumerable<GetEmployerRequestsByIdsResponse> EmployerRequests { get; set; }
    }
}
