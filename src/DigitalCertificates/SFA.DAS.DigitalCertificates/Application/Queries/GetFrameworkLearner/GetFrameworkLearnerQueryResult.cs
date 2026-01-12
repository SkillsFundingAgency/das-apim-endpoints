using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.DigitalCertificates.InnerApi.Responses.Assessor;

namespace SFA.DAS.DigitalCertificates.Application.Queries.GetFrameworkLearner
{
    public class GetFrameworkLearnerQueryResult
    {
        public string FamilyName { get; set; }
        public string GivenNames { get; set; }
        public long? Uln { get; set; }
        public string CertificateReference { get; set; }
        public string FrameworkCertificateNumber { get; set; }
        public string CourseName { get; set; }
        public string PathwayName { get; set; }
        public string CourseLevel { get; set; }
        public DateTime DateAwarded { get; set; }
        public string ProviderName { get; set; }
        public string EmployerName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? PrintRequestedAt { get; set; }
        public string PrintRequestedBy { get; set; }
        public List<QualificationDetails> QualificationsAndAwardingBodies { get; set; }
        // TODO: This 'DeliveryInformation field is not required for P2-2551. We need to discuss with Alan what data is actually required for this field.
        public List<object> DeliveryInformation { get; set; }

        // TODO: The fields below are not available from the Inner API and are not required for P2-2551.
        // They can be populated in future tickets if needed or it can be remove if not required for the upcoming tickets
        public string CertificateType { get; set; }
        public string CourseCode { get; set; }
        public string CourseOption { get; set; }
        public string OverallGrade { get; set; }
        public long? Ukprn { get; set; }
        public string AssessorName { get; set; }

        public static implicit operator GetFrameworkLearnerQueryResult(GetFrameworkLearnerResponse source)
        {
            if (source == null) return null;

            return new GetFrameworkLearnerQueryResult
            {
                FamilyName = source.ApprenticeSurname,
                GivenNames = source.ApprenticeForename,
                Uln = source.ApprenticeULN,
                CertificateReference = source.CertificateReference,
                FrameworkCertificateNumber = source.FrameworkCertificateNumber,
                CourseName = source.FrameworkName,
                PathwayName = source.PathwayName,
                CourseLevel = source.ApprenticeshipLevelName,
                DateAwarded = source.CertificationDate,
                ProviderName = source.ProviderName,
                EmployerName = source.EmployerName,
                StartDate = source.ApprenticeStartdate,
                QualificationsAndAwardingBodies = source.QualificationsAndAwardingBodies?.Select(q => new QualificationDetails { Name = q.Name, AwardingBody = q.AwardingBody }).ToList(),
                PrintRequestedAt = source.PrintRequestedAt,
                PrintRequestedBy = source.PrintRequestedBy
            };
        }
    }

    public class QualificationDetails
    {
        public string Name { get; set; }
        public string AwardingBody { get; set; }
    }
}
