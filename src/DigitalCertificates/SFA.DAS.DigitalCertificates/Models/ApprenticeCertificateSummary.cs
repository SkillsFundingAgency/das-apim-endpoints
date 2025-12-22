using System;

namespace SFA.DAS.DigitalCertificates.Models
{
    public class ApprenticeCertificateSummary
    {
        public Guid CertificateId { get; set; }
        public string CertificateType { get; set; }
        public string CourseName { get; set; }
        public string CourseLevel { get; set; }
        public DateTime DateAwarded { get; set; }
    }
}
