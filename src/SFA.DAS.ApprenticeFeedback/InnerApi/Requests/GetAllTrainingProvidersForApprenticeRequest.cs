using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.ApprenticeFeedback.InnerApi.Requests
{
    public class GetAllTrainingProvidersForApprenticeRequest : IGetAllApiRequest
    {
        public Guid ApprenticeId { get; set; }
        public string GetAllUrl => $"api/providers/{ApprenticeId}";
    }
}
