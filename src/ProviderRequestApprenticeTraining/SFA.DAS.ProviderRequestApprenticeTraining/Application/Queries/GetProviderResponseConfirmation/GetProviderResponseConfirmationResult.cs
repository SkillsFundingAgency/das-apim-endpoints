using SFA.DAS.ProviderRequestApprenticeTraining.InnerApi.Responses;
using System.Collections.Generic;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetProviderResponseConfirmation
{
    public class GetProviderResponseConfirmationResult
    {
        public long Ukprn { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public List<GetSelectEmployerRequestsResponse> EmployerRequests { get; set; } = new List<GetSelectEmployerRequestsResponse>();

    }
}
