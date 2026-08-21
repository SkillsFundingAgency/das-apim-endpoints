using System;
using System.Collections.Generic;
using SFA.DAS.DigitalCertificates.Application.Queries.GetStandardCertificate;

namespace SFA.DAS.DigitalCertificates.Api.Models.Certificates
{
    public class GetStandardCertificateResponse
    {
        public string FamilyName { get; set; }
        public string GivenNames { get; set; }
        public long? Uln { get; set; }
        public string CertificateType { get; set; }
        public string CertificateReference { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public string CourseOption { get; set; }
        public int? CourseLevel { get; set; }
        public DateTime? DateAwarded { get; set; }
        public string OverallGrade { get; set; }
        public string ProviderName { get; set; }
        public long Ukprn { get; set; }
        public string EmployerName { get; set; }
        public string AssessorName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? PrintRequestedAt { get; set; }
        public string PrintRequestedBy { get; set; }
        public List<DeliveryInformationDto> DeliveryInformation { get; set; }
        public bool CoronationEmblem { get; set; }

        public static implicit operator GetStandardCertificateResponse(GetStandardCertificateQueryResult source)
        {
            if (source == null) return null;

            return new GetStandardCertificateResponse
            {
                FamilyName = source.FamilyName,
                GivenNames = source.GivenNames,
                Uln = source.Uln,
                CertificateType = source.CertificateType,
                CertificateReference = source.CertificateReference,
                CourseCode = source.CourseCode,
                CourseName = source.CourseName,
                CourseOption = source.CourseOption,
                CourseLevel = source.CourseLevel,
                DateAwarded = source.DateAwarded,
                OverallGrade = source.OverallGrade,
                ProviderName = source.ProviderName,
                Ukprn = source.Ukprn,
                EmployerName = source.EmployerName,
                AssessorName = source.AssessorName,
                StartDate = source.StartDate,
                PrintRequestedAt = source.PrintRequestedAt,
                PrintRequestedBy = source.PrintRequestedBy,
                DeliveryInformation = source.DeliveryInformation == null ? null : source.DeliveryInformation.ConvertAll(d => new DeliveryInformationDto { Id = d.Id, Action = d.Action, Status = d.Status, EventTime = d.EventTime }),
                CoronationEmblem = source.CoronationEmblem
            };
        }
    }
}
