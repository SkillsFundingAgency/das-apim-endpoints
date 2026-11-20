using System;
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
        public string LatestEPAOutcome { get; set; }

        public string Status { get; set; }
        public DateTime? PrintRequestedAt { get; set; }
        public string PrintRequestedBy { get; set; }
        public string PrintStatus { get; set; }
        public DateTime? PrintStatusAt { get; set; }

        public bool CoronationEmblem { get; set; }

        public static implicit operator GetStandardCertificateQueryResult(
            GetStandardCertificateResponse source)
        {
            if (source == null)
                return null;

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
                EmployerName = source.CertificateData?.EmployerName ?? "",
                AssessorName = "",
                StartDate = source.LearningStartDate,
                LatestEPAOutcome = source.LatestEPAOutcome,
                Status = source.Status,
                PrintRequestedAt = source.PrintRequestedAt,
                PrintRequestedBy = source.PrintRequestedBy,
                PrintStatus = source.PrintStatus,
                PrintStatusAt = source.PrintStatusAt,
                CoronationEmblem = source.CertificateData?.CoronationEmblem ?? false
            };
        }
    }
}