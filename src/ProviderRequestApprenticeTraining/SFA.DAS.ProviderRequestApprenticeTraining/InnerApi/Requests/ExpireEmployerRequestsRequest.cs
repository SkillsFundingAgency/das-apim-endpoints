using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;

namespace SFA.DAS.ProviderRequestApprenticeTraining.InnerApi.Requests
{
    public class ExpireEmployerRequestsRequest : IPostApiRequest<ExpireEmployerRequestsData>
    {
        public ExpireEmployerRequestsData Data { get; set; }
        public string PostUrl => $"api/employerrequest/expire-requests";
    }

    public class ExpireEmployerRequestsData
    {
    }
}
