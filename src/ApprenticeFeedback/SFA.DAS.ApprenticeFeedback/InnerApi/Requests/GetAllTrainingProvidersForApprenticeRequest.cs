using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;
using System;

namespace SFA.DAS.ApprenticeFeedback.InnerApi.Requests
{
    public class GetAllTrainingProvidersForApprenticeRequest : IGetApiRequest
    {
        public Guid ApprenticeId { get; set; }
        public string GetUrl => $"api/providers/{ApprenticeId}";
    }
}
