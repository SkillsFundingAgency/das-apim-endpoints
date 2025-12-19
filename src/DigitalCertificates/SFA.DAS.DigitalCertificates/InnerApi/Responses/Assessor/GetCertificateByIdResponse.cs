using System;
using SFA.DAS.DigitalCertificates.Application.Queries.GetCertificateById;

namespace SFA.DAS.DigitalCertificates.InnerApi.Responses.Assessor
{
    public class GetCertificateByIdResponse
    {
        public long Uln { get; set; }
        public Guid OrganisationId { get; set; }
        public string Type { get; set; }
        public string CertificateReference { get; set; }
        public long? ProviderUkPrn { get; set; }
        public DateTime? PrintRequestedAt { get; set; }
        public string PrintRequestedBy { get; set; }
        public string LearnerFamilyName { get; set; }
        public string LearnerGivenNames { get; set; }
        public string ProviderName { get; set; }
        public string CourseOption { get; set; }
        public string OverallGrade { get; set; }
        public string StandardReference { get; set; }
        public string StandardName { get; set; }
        public int StandardLevel { get; set; }
        public DateTime? AchievementDate { get; set; }
        public DateTime? LearningStartDate { get; set; }
        public CertificateData CertificateData { get; set; }

        public static implicit operator GetCertificateByIdQueryResult(GetCertificateByIdResponse source)
        {
            if (source == null) return null;

            var employerName = source.CertificateData?.EmployerName ?? "";

            return new GetCertificateByIdQueryResult
            {
                FamilyName = source.LearnerFamilyName,
                GivenNames = source.LearnerGivenNames,
                Uln = source.Uln.ToString(),
                CertificateType = source.Type,
                CertificateReference = source.CertificateReference,
                CourseCode = source.StandardReference,
                CourseName = source.StandardName,
                CourseOption = source.CourseOption,
                CourseLevel = source.StandardLevel.ToString(),
                DateAwarded = source.AchievementDate,
                OverallGrade = source.OverallGrade,
                ProviderName = source.ProviderName,
                Ukprn = source.ProviderUkPrn ?? 0,
                EmployerName = employerName,
                AssessorName = "",
                StartDate = source.LearningStartDate,
                // TODO: This 'DeliveryInformation field is not required for P2-2550. We need to discuss with Alan what data is actually required for this field.
                DeliveryInformation = null,
                PrintRequestedAt = source.PrintRequestedAt,
                PrintRequestedBy = source.PrintRequestedBy,
            };
        }
    }

    public class CertificateData
    {
        public string EmployerName { get; set; }
    }
}
