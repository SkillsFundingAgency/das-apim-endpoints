using SFA.DAS.DigitalCertificates.Models;
using System;
using System.Collections.Generic;

namespace SFA.DAS.DigitalCertificates.InnerApi.Responses
{
    public class GetSharingsResponse
    {
        public Guid UserId { get; set; }
        public Guid CertificateId { get; set; }
        public string CertificateType { get; set; }
        public string CourseName { get; set; }
        public List<Sharing> Sharings { get; set; }
    }
}
