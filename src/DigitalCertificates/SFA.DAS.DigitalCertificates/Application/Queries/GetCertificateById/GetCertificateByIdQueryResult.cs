using System;

namespace SFA.DAS.DigitalCertificates.Application.Queries.GetCertificateById
{
    public class GetCertificateByIdQueryResult
    {
        public string FamilyName { get; set; }
        public string GivenNames { get; set; }
        public string Uln { get; set; }
        public string CertificateType { get; set; }
        public string CertificateReference { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public string CourseOption { get; set; }
        public string CourseLevel { get; set; }
        public DateTime? DateAwarded { get; set; }
        public string OverallGrade { get; set; }
        public string ProviderName { get; set; }
        public long Ukprn { get; set; }
        public string EmployerName { get; set; }
        public string AssessorName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? PrintRequestedAt { get; set; }
        public string PrintRequestedBy { get; set; }
        public object DeliveryInformation { get; set; }
    }
}
