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
        public DateTime? PrintRequestedAt { get; set; }
        public string PrintRequestedBy { get; set; }
        public string CertificateType { get; set; }
        public List<QualificationDetailsDto> QualificationsAndAwardingBodies { get; set; }
        public List<DeliveryInformationDto> DeliveryInformation { get; set; }

        public static implicit operator GetFrameworkCertificateResponse(GetFrameworkCertificateQueryResult source)
        {
            if (source == null) return null;
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
                PrintRequestedAt = source.PrintRequestedAt,
                PrintRequestedBy = source.PrintRequestedBy,
                QualificationsAndAwardingBodies = source.QualificationsAndAwardingBodies == null ? null : source.QualificationsAndAwardingBodies.ConvertAll(q => new QualificationDetailsDto { Name = q.Name, AwardingBody = q.AwardingBody }),
                DeliveryInformation = source.DeliveryInformation == null ? null : source.DeliveryInformation.ConvertAll(d => new DeliveryInformationDto { Id = d.Id, Action = d.Action, Status = d.Status, EventTime = d.EventTime })
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
