using System.Collections.Generic;

namespace SFA.DAS.DigitalCertificates.InnerApi.Responses.Assessor
{
    public class GetCertificateMasksResponse
    {
        public List<CertificateMask> Masks { get; set; } = [];
    }

    public class CertificateMask
    {
        public string CertificateType { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public string CourseLevel { get; set; }
        public string ProviderName { get; set; }
    }
}
