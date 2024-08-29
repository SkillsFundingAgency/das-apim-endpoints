using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;

namespace SFA.DAS.ProviderRequestApprenticeTraining.InnerApi.Requests
{
    public class SubmitProviderResponseRequest : IPostApiRequest<SubmitProviderResponseRequestData>
    {
        public SubmitProviderResponseRequestData Data { get; set; }

        public string PostUrl => $"api/employerrequest/provider/submit-response";

        public SubmitProviderResponseRequest(SubmitProviderResponseRequestData data)
        {
            Data = data;
        }
    }
    public class SubmitProviderResponseRequestData 
    {
        public long Ukprn { get; set; }
        public List<Guid> EmployerRequestIds { get; set; } = new List<Guid>();
        public string ContactName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public Guid RespondedBy { get; set; }
    }
}
