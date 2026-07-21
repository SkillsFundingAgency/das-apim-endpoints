using System;
using System.Collections.Generic;
using SFA.DAS.DigitalCertificates.Application.Queries.GetSharedFrameworkCertificate;

namespace SFA.DAS.DigitalCertificates.Api.Models.Sharing
{
    public class GetSharedFrameworkCertificateResponse
    {
        public string FamilyName { get; set; }
        public string GivenNames { get; set; }
        public string CertificateType { get; set; }
        public List<QualificationDetailsDto> QualificationsAndAwardingBodies { get; set; }
        public string CertificateReference { get; set; }
        public string FrameworkCertificateNumber { get; set; }
        public string CourseName { get; set; }
        public string CourseOption { get; set; }
        public string CourseLevel { get; set; }
        public DateTime? DateAwarded { get; set; }
        public string ProviderName { get; set; }
        public string EmployerName { get; set; }
        public DateTime? StartDate { get; set; }

        public static implicit operator GetSharedFrameworkCertificateResponse(GetSharedFrameworkCertificateQueryResult source)
        {
            if (source == null) return null;

            return new GetSharedFrameworkCertificateResponse
            {
                FamilyName = source.FamilyName,
                GivenNames = source.GivenNames,
                CertificateType = source.CertificateType,
                QualificationsAndAwardingBodies = source.QualificationsAndAwardingBodies == null ? null : source.QualificationsAndAwardingBodies.ConvertAll(q => new QualificationDetailsDto { Name = q.Name, AwardingBody = q.AwardingBody }),
                CertificateReference = source.CertificateReference,
                FrameworkCertificateNumber = source.FrameworkCertificateNumber,
                CourseName = source.CourseName,
                CourseOption = source.CourseOption,
                CourseLevel = source.CourseLevel,
                DateAwarded = source.DateAwarded,
                ProviderName = source.ProviderName,
                EmployerName = source.EmployerName,
                StartDate = source.StartDate
            };
        }
    }
    public class QualificationDetailsDto
    {
        public string Name { get; set; }
        public string AwardingBody { get; set; }
    }

    public class DeliveryInformationDto
    {
        public Guid Id { get; set; }
        public string Action { get; set; }
        public string Status { get; set; }
        public DateTime EventTime { get; set; }
    }
}
