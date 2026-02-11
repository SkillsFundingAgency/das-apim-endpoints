using System;

namespace SFA.DAS.DigitalCertificates.InnerApi.Responses.Assessor
{
    public class GetStandardCertificateResponse
    {
        public long? Uln { get; set; }
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
        public int? StandardLevel { get; set; }
        public DateTime? AchievementDate { get; set; }
        public DateTime? LearningStartDate { get; set; }
        public CertificateData CertificateData { get; set; }
    }

    public class CertificateData
    {
        public string EmployerName { get; set; }
    }
}
