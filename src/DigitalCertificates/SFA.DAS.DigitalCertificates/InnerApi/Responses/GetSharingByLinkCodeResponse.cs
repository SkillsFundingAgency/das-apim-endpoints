using System;

namespace SFA.DAS.DigitalCertificates.InnerApi.Responses
{
    public class GetSharingByLinkCodeResponse
    {
        public Guid CertificateId { get; set; }
        public string CertificateType { get; set; }
        public DateTime ExpiryTime { get; set; }
        public Guid SharingId { get; set; }
    }
}
