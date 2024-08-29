using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;

namespace SFA.DAS.ProviderRequestApprenticeTraining.InnerApi.Requests
{
    public class CreateProviderResponseEmployerRequestRequest : IPostApiRequest<CreateEmployerResponseEmployerRequestData>
    {
        public CreateEmployerResponseEmployerRequestData Data { get; set; }

        public string PostUrl => $"api/employerrequest/provider/{Data.Ukprn}/acknowledge-requests";

        public CreateProviderResponseEmployerRequestRequest(CreateEmployerResponseEmployerRequestData data)
        {
            Data = data;
        }
    }
    public class CreateEmployerResponseEmployerRequestData 
    {
        public List<Guid> EmployerRequestIds { get; set; }
        public long Ukprn { get; set; }
    }
}
