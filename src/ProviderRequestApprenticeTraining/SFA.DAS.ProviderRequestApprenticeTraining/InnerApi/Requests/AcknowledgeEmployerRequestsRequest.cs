using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;

namespace SFA.DAS.ProviderRequestApprenticeTraining.InnerApi.Requests
{
    public class AcknowledgeEmployerRequestsRequest : IPostApiRequest<AcknowledgeEmployerRequestsData>
    {
        public AcknowledgeEmployerRequestsData Data { get; set; }

        public string PostUrl => $"api/providers/{Data.Ukprn}/employer-requests/acknowledge";

        public AcknowledgeEmployerRequestsRequest(AcknowledgeEmployerRequestsData data)
        {
            Data = data;
        }
    }
    public class AcknowledgeEmployerRequestsData
    {
        public List<Guid> EmployerRequestIds { get; set; }
        public long Ukprn { get; set; }
    }
}
