using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.ApprenticeFeedback.InnerApi.Requests
{
    public class GetAllTrainingProvidersForApprenticeRequest : IGetApiRequest
    {
        public Guid ApprenticeId { get; set; }
        public string GetUrl => $"api/providers/{ApprenticeId}";
    }
}
