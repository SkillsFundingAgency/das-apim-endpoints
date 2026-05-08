using SFA.DAS.DigitalCertificates.InnerApi.Responses.Assessor;
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
        public int? Ukprn { get; set; }

        public static implicit operator CertificateMatchResult(CertificateSearchMatch m)
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
                ProviderName = m.ProviderName,
                Ukprn = int.TryParse(m.Ukprn, out var ukprn) ? ukprn : (int?)null
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

        public static implicit operator CertificateMaskResult(CertificateMask m)
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
