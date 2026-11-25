using System;

namespace SFA.DAS.DigitalCertificates.InnerApi.Responses.Assessor
{
    public class CertificateLog
    {
        public Guid Id { get; set; }
        public Guid CertificateId { get; set; }
        public string Action { get; set; }
        public string Status { get; set; }
        public DateTime EventTime { get; set; }
    }
}

