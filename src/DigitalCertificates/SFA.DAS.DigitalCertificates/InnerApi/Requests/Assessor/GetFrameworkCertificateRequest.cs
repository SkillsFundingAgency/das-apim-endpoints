using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.DigitalCertificates.InnerApi.Requests.Assessor
{
    public class GetFrameworkCertificateRequest : IGetApiRequest
    {
        public Guid Id { get; }

        public GetFrameworkCertificateRequest(Guid id)
        {
            Id = id;
        }

        public string GetUrl => $"api/v1/learnerdetails/framework-learner/{Id}?allLogs=false";
    }
}