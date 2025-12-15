using System;
using System.Collections.Generic;

namespace SFA.DAS.DigitalCertificates.Models
{
    public class SharingEmail
    {
        public Guid SharingEmailId { get; set; }
        public string EmailAddress { get; set; }
        public Guid EmailLinkCode { get; set; }
        public DateTime SentTime { get; set; }
        public List<DateTime> SharingEmailAccess { get; set; }
    }
}
