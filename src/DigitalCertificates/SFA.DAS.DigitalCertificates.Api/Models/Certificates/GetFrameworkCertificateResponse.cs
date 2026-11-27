using System;
using System.Collections.Generic;
using SFA.DAS.DigitalCertificates.Application.Queries.GetFrameworkCertificate;

namespace SFA.DAS.DigitalCertificates.Api.Models.Certificates
{
    public class GetFrameworkCertificateResponse
    {
        public string FamilyName { get; set; }
        public string GivenNames { get; set; }
        public long? Uln { get; set; }
        public string CertificateReference { get; set; }
        public string FrameworkCertificateNumber { get; set; }
        public string CourseName { get; set; }
        public string CourseOption { get; set; }
        public string CourseLevel { get; set; }
        public DateTime DateAwarded { get; set; }
        public string ProviderName { get; set; }
        public string EmployerName { get; set; }
        public DateTime? StartDate { get; set; }
        public string CertificateType { get; set; }
        public string Status { get; set; }
        public DateTime? PrintRequestedAt { get; set; }
        public string PrintRequestedBy { get; set; }
        public string PrintStatus { get; set; }
        public DateTime? PrintStatusAt { get; set; }
        public List<QualificationDetailsDto> QualificationsAndAwardingBodies { get; set; }

        public static implicit operator GetFrameworkCertificateResponse(
            GetFrameworkCertificateQueryResult source)
        {
            if (source == null)
                return null;

            return new GetFrameworkCertificateResponse
            {
                FamilyName = source.FamilyName,
                GivenNames = source.GivenNames,
                Uln = source.Uln,
                CertificateReference = source.CertificateReference,
                FrameworkCertificateNumber = source.FrameworkCertificateNumber,
                CertificateType = source.CertificateType,
                CourseName = source.CourseName,
                CourseOption = source.CourseOption,
                CourseLevel = source.CourseLevel,
                DateAwarded = source.DateAwarded,
                ProviderName = source.ProviderName,
                EmployerName = source.EmployerName,
                StartDate = source.StartDate,
                Status = source.Status,
                PrintRequestedAt = source.PrintRequestedAt,
                PrintRequestedBy = source.PrintRequestedBy,
                PrintStatus = source.PrintStatus,
                PrintStatusAt = source.PrintStatusAt,
                QualificationsAndAwardingBodies =
                    source.QualificationsAndAwardingBodies?.ConvertAll(q =>
                        new QualificationDetailsDto
                        {
                            Name = q.Name,
                            AwardingBody = q.AwardingBody
                        })
            };
        }
    }

    public class QualificationDetailsDto
    {
        public string Name { get; set; }
        public string AwardingBody { get; set; }
    }
}