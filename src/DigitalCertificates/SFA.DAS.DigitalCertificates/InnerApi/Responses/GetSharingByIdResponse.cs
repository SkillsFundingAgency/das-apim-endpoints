using SFA.DAS.DigitalCertificates.Models;
using System;
using System.Collections.Generic;

namespace SFA.DAS.DigitalCertificates.InnerApi.Responses
{
    public class GetSharingByIdResponse
    {
        public Guid UserId { get; set; }
        public Guid CertificateId { get; set; }
        public string CertificateType { get; set; }
        public string CourseName { get; set; }
        public Guid SharingId { get; set; }
        public int SharingNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid LinkCode { get; set; }
        public DateTime ExpiryTime { get; set; }
        public List<DateTime> SharingAccess { get; set; }
        public List<SharingEmail> SharingEmails { get; set; }
    }
}
