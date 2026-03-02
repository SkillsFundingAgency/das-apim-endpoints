using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.DigitalCertificates.InnerApi.Requests.Assessor
{
    public class GetFrameworkCertificateRequest : IGetApiRequest
    {
        public Guid Id { get; }
        public bool IncludeLogs { get; }

        public GetFrameworkCertificateRequest(Guid id, bool includeLogs = false)
        {
            Id = id;
            IncludeLogs = includeLogs;
        }

        public string GetUrl => IncludeLogs
            ? $"api/v1/learnerdetails/framework-learner/{Id}"
            : $"api/v1/learnerdetails/framework-learner/{Id}?allLogs=false";
    }
}