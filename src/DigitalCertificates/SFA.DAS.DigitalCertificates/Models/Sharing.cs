using System;
using System.Collections.Generic;

namespace SFA.DAS.DigitalCertificates.Models
{
    public class Sharing
    {
        public Guid SharingId { get; set; }
        public int SharingNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid LinkCode { get; set; }
        public DateTime ExpiryTime { get; set; }
        public List<DateTime> SharingAccess { get; set; }
        public List<SharingEmail> SharingEmails { get; set; }
    }
}
