using System.Collections.Generic;
using SFA.DAS.DigitalCertificates.Models;

namespace SFA.DAS.DigitalCertificates.Application.Queries.GetCertificates
{
    public class GetCertificatesResult
    {
        public UlnAuthorisation Authorisation { get; set; }
        public List<ApprenticeCertificateSummary> Certificates { get; set; } = [];
    }
}
