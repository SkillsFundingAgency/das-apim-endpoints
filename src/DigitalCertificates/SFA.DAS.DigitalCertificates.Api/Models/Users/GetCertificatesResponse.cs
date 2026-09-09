using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.DigitalCertificates.Application.Queries.GetCertificates;

namespace SFA.DAS.DigitalCertificates.Api.Models.Users
{
    public class GetCertificatesResponse
    {
        public UlnAuthorisationDto Authorisation { get; set; }
        public List<ApprenticeCertificateSummaryDto> Certificates { get; set; } = new List<ApprenticeCertificateSummaryDto>();

        public static implicit operator GetCertificatesResponse(GetCertificatesResult source)
        {
            if (source == null) return null;

            return new GetCertificatesResponse
            {
                Authorisation = source.Authorisation == null ? null : new UlnAuthorisationDto
                {
                    AuthorisationId = source.Authorisation.AuthorisationId,
                    AuthorisedAt = source.Authorisation.AuthorisedAt,
                    Uln = source.Authorisation.Uln
                },
                Certificates = source.Certificates?.Select(c => new ApprenticeCertificateSummaryDto
                {
                    CertificateId = c.CertificateId,
                    CertificateType = c.CertificateType,
                    CourseName = c.CourseName,
                    CourseLevel = c.CourseLevel,
                    DateAwarded = c.DateAwarded
                }).ToList() ?? new List<ApprenticeCertificateSummaryDto>()
            };
        }
    }

    public class UlnAuthorisationDto
    {
        public Guid AuthorisationId { get; set; }
        public DateTime AuthorisedAt { get; set; }
        public long Uln { get; set; }
    }

    public class ApprenticeCertificateSummaryDto
    {
        public Guid CertificateId { get; set; }
        public string CertificateType { get; set; }
        public string CourseName { get; set; }
        public string CourseLevel { get; set; }
        public DateTime DateAwarded { get; set; }
    }
}
