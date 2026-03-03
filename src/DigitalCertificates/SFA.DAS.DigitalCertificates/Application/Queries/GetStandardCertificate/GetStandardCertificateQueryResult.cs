using System;
using System.Collections.Generic;
using SFA.DAS.DigitalCertificates.InnerApi.Responses.Assessor;

namespace SFA.DAS.DigitalCertificates.Application.Queries.GetStandardCertificate
{
    public class GetStandardCertificateQueryResult
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
        // TODO: This 'DeliveryInformation field is not required for P2-2550. We need to discuss with Alan what data is actually required for this field.
        public List<object> DeliveryInformation { get; set; }

        public static implicit operator GetStandardCertificateQueryResult(GetStandardCertificateResponse source)
        {
            if (source == null) return null;

            var employerName = source.CertificateData?.EmployerName ?? "";

            return new GetStandardCertificateQueryResult
            {
                FamilyName = source.LearnerFamilyName,
                GivenNames = source.LearnerGivenNames,
                Uln = source.Uln,
                CertificateType = source.Type,
                CertificateReference = source.CertificateReference,
                CourseCode = source.StandardReference,
                CourseName = source.StandardName,
                CourseOption = source.CourseOption,
                CourseLevel = source.StandardLevel,
                DateAwarded = source.AchievementDate,
                OverallGrade = source.OverallGrade,
                ProviderName = source.ProviderName,
                Ukprn = source.ProviderUkPrn ?? 0,
                EmployerName = employerName,
                AssessorName = "",
                StartDate = source.LearningStartDate,
                DeliveryInformation = null,
                PrintRequestedAt = source.PrintRequestedAt,
                PrintRequestedBy = source.PrintRequestedBy,
            };
        }
    }
}
