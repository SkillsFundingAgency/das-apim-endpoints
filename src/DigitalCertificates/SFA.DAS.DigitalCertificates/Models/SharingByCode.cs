using System;

namespace SFA.DAS.DigitalCertificates.Models
{
    public class SharingByCode
    {
        public Guid CertificateId { get; set; }
        public string CertificateType { get; set; }
        public DateTime ExpiryTime { get; set; }
        public Guid? SharingId { get; set; }
        public Guid? SharingEmailId { get; set; }
    }
}
