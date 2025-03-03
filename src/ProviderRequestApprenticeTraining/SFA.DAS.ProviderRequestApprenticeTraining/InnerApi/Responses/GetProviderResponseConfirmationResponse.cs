using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;

namespace SFA.DAS.ProviderRequestApprenticeTraining.InnerApi.Responses
{
    public class GetProviderResponseConfirmationResponse 
    {
        public long Ukprn { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public List<GetSelectEmployerRequestsResponse> EmployerRequests { get; set; } = new List<GetSelectEmployerRequestsResponse>();
    }
}
