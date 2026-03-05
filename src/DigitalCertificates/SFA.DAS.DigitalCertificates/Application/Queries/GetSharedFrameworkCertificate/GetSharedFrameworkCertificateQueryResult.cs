using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.DigitalCertificates.InnerApi.Responses.Assessor;
using SFA.DAS.DigitalCertificates.Application.Queries.GetFrameworkCertificate;

namespace SFA.DAS.DigitalCertificates.Application.Queries.GetSharedFrameworkCertificate
{
    public class GetSharedFrameworkCertificateQueryResult
    {
        public string FamilyName { get; set; }
        public string GivenNames { get; set; }
        public string CertificateType { get; set; }
        public List<QualificationDetails> QualificationsAndAwardingBodies { get; set; }
        public string CertificateReference { get; set; }
        public string FrameworkCertificateNumber { get; set; }
        public string CourseName { get; set; }
        public string CourseOption { get; set; }
        public string CourseLevel { get; set; }
        public DateTime? DateAwarded { get; set; }
        public string ProviderName { get; set; }
        public string EmployerName { get; set; }
        public DateTime? StartDate { get; set; }

        public static implicit operator GetSharedFrameworkCertificateQueryResult(GetFrameworkCertificateResponse source)
        {
            if (source == null) return null;

            return new GetSharedFrameworkCertificateQueryResult
            {
                FamilyName = source.ApprenticeSurname,
                GivenNames = source.ApprenticeForename,
                CertificateType = Enums.CertificateType.Framework.ToString(),
                QualificationsAndAwardingBodies = source.QualificationsAndAwardingBodies?.Select(q => new QualificationDetails { Name = q.Name, AwardingBody = q.AwardingBody }).ToList(),
                CertificateReference = source.CertificateReference,
                FrameworkCertificateNumber = source.FrameworkCertificateNumber,
                CourseName = source.FrameworkName,
                CourseOption = source.PathwayName,
                CourseLevel = source.ApprenticeshipLevelName,
                DateAwarded = source.CertificationDate,
                ProviderName = source.ProviderName,
                EmployerName = source.EmployerName,
                StartDate = source.ApprenticeStartdate
            };
        }
    }
}
