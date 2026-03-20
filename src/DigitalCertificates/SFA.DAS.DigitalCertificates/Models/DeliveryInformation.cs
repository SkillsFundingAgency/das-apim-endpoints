using System;

namespace SFA.DAS.DigitalCertificates.Models
{
    public class DeliveryInformation
    {
        public Guid Id { get; set; }
        public string Action { get; set; }
        public string Status { get; set; }
        public DateTime EventTime { get; set; }
    }
}

