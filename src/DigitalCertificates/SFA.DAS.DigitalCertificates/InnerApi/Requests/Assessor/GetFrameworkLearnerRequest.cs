using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.DigitalCertificates.InnerApi.Requests.Assessor
{
    public class GetFrameworkLearnerRequest : IGetApiRequest
    {
        public Guid FrameworkLearnerId { get; }

        public GetFrameworkLearnerRequest(Guid frameworkLearnerId)
        {
            FrameworkLearnerId = frameworkLearnerId;
        }

        public string GetUrl => $"api/v1/learnerdetails/framework-learner/{FrameworkLearnerId}?allLogs=false";
    }
}