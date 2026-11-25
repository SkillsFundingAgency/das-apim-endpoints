using System;
using System.Collections.Generic;

namespace SFA.DAS.DigitalCertificates.InnerApi.Responses.Assessor
{
    public class GetCertificateSearchResponse
    {
        public List<CertificateSearchMatch> Matches { get; set; } = new List<CertificateSearchMatch>();
    }

    public class CertificateSearchMatch
    {
        public long Uln { get; set; }
        public string CertificateType { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public string CourseLevel { get; set; }
        public DateTime? DateAwarded { get; set; }
        public string ProviderName { get; set; }
        public string Ukprn { get; set; }
    }
}
