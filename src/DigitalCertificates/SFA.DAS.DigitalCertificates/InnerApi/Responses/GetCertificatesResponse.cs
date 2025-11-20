using System.Collections.Generic;
using SFA.DAS.DigitalCertificates.Models;

namespace SFA.DAS.DigitalCertificates.InnerApi.Responses
{
    public class GetCertificatesResponse
    {
        public List<ApprenticeCertificateSummary> Certificates { get; set; }
    }
}
