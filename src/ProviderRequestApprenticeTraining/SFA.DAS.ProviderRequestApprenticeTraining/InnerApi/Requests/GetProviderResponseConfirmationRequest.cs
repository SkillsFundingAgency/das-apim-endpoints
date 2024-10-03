using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.ProviderRequestApprenticeTraining.InnerApi.Requests
{
    public class GetProviderResponseConfirmationRequest : IGetApiRequest
    {
        public Guid ProviderResponseId { get; set; }

        public GetProviderResponseConfirmationRequest(Guid providerResponseId)
        { 
            ProviderResponseId = providerResponseId;
        }

        public string GetUrl => $"api/provider-responses/{ProviderResponseId}/confirmation";
    }
}
