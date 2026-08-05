using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;
using System;

namespace SFA.DAS.ApprenticeFeedback.InnerApi.Requests
{
    public class GetTrainingProviderForApprenticeRequest : IGetApiRequest
    {
        public Guid ApprenticeId { get; set; }
        public long Ukprn { get; set; }
        public string GetUrl => $"api/providers/{ApprenticeId}/{Ukprn}";
    }
}
