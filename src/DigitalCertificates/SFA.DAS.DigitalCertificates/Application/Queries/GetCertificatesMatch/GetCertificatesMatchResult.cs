using System;
using System.Collections.Generic;

namespace SFA.DAS.DigitalCertificates.Application.Queries.GetCertificatesMatch
{
    public class GetCertificatesMatchResult
    {
        public List<CertificateMatchResult> Matches { get; set; } = new List<CertificateMatchResult>();
        public List<CertificateMaskResult> Masks { get; set; } = new List<CertificateMaskResult>();
    }

    public class CertificateMatchResult
    {
        public long Uln { get; set; }
        public string CertificateType { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public string CourseLevel { get; set; }
        public DateTime? DateAwarded { get; set; }
        public string ProviderName { get; set; }
        
        public static implicit operator CertificateMatchResult(SFA.DAS.DigitalCertificates.InnerApi.Responses.Assessor.CertificateSearchMatch m)
        {
            if (m == null) return null;
            return new CertificateMatchResult
            {
                Uln = m.Uln,
                CertificateType = m.CertificateType,
                CourseCode = m.CourseCode,
                CourseName = m.CourseName,
                CourseLevel = m.CourseLevel,
                DateAwarded = m.DateAwarded,
                ProviderName = m.ProviderName
            };
        }
    }

    public class CertificateMaskResult
    {
        public string CertificateType { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public string CourseLevel { get; set; }
        public string ProviderName { get; set; }

        public static implicit operator CertificateMaskResult(SFA.DAS.DigitalCertificates.InnerApi.Responses.Assessor.CertificateMask m)
        {
            if (m == null) return null;
            return new CertificateMaskResult
            {
                CertificateType = m.CertificateType,
                CourseCode = m.CourseCode,
                CourseName = m.CourseName,
                CourseLevel = m.CourseLevel,
                ProviderName = m.ProviderName
            };
        }
    }
}
