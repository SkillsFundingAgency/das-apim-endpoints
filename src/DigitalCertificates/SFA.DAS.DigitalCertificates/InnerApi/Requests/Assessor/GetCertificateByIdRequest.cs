using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.DigitalCertificates.InnerApi.Requests.Assessor
{
    public class GetCertificateByIdRequest : IGetApiRequest
    {
        public Guid Id { get; }
        public bool IncludeLogs { get; }

        public GetCertificateByIdRequest(Guid id, bool includeLogs = true)
        {
            Id = id;
            IncludeLogs = includeLogs;
        }

        public string GetUrl => IncludeLogs
            ? $"api/v1/certificates/{Id}"
            : $"api/v1/certificates/{Id}?includeLogs=false";
    }
}
