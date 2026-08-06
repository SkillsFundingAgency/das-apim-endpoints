using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.DigitalCertificates.Application.Queries.GetCertificatesMatch;

namespace SFA.DAS.DigitalCertificates.Api.Models.Users
{
    public class GetCertificatesMatchResponse
    {
        public List<CertificateMatch> Matches { get; set; } = new List<CertificateMatch>();
        public List<CertificateMask> Masks { get; set; } = new List<CertificateMask>();

        public static implicit operator GetCertificatesMatchResponse(GetCertificatesMatchResult source)
        {
            if (source == null) return null;

            return new GetCertificatesMatchResponse
            {
                Matches = source.Matches?.Select(m => new CertificateMatch
                {
                    Uln = m.Uln,
                    UserIdentityId = m.UserIdentityId,
                    CertificateType = m.CertificateType,
                    CourseCode = m.CourseCode,
                    CourseName = m.CourseName,
                    CourseLevel = m.CourseLevel,
                    DateAwarded = m.DateAwarded,
                    ProviderName = m.ProviderName,
                    Ukprn = m.Ukprn
                }).ToList() ?? new List<CertificateMatch>(),
                Masks = source.Masks?.Select(m => new CertificateMask
                {
                    CertificateType = m.CertificateType,
                    CourseCode = m.CourseCode,
                    CourseName = m.CourseName,
                    CourseLevel = m.CourseLevel,
                    ProviderName = m.ProviderName
                }).ToList() ?? new List<CertificateMask>()
            };
        }
    }

    public class CertificateMatch
    {
        public long Uln { get; set; }
        public Guid UserIdentityId { get; set; }
        public string CertificateType { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public string CourseLevel { get; set; }
        public DateTime? DateAwarded { get; set; }
        public string ProviderName { get; set; }
        public int? Ukprn { get; set; }
    }

    public class CertificateMask
    {
        public string CertificateType { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public string CourseLevel { get; set; }
        public string ProviderName { get; set; }
    }
}
