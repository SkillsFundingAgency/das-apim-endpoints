using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.ApprenticeFeedback.InnerApi.Requests
{
    public class GetAllApprenticeFeedbackTargetsRequest : IGetAllApiRequest
    {
        public Guid ApprenticeId { get; set; }
        public string GetAllUrl => $"api/apprenticefeedbacktarget/{ApprenticeId}";
    }
}
