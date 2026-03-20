using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.DigitalCertificates.Constants;
using SFA.DAS.DigitalCertificates.InnerApi.Responses.Assessor;
using SFA.DAS.DigitalCertificates.Models;

namespace SFA.DAS.DigitalCertificates.Application.Queries.GetFrameworkCertificate
{
    public class GetFrameworkCertificateQueryResult
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
        public List<QualificationDetails> QualificationsAndAwardingBodies { get; set; }
        public List<DeliveryInformation> DeliveryInformation { get; set; }

        // TODO: The fields below are not available from the Inner API and are not required for P2-2551.
        // They can be populated in future tickets if needed or it can be remove if not required for the upcoming tickets
        public string CourseCode { get; set; }
        public string OverallGrade { get; set; }
        public long? Ukprn { get; set; }
        public string AssessorName { get; set; }

        public static implicit operator GetFrameworkCertificateQueryResult(GetFrameworkCertificateResponse source)
        {
            if (source == null) return null;

            return new GetFrameworkCertificateQueryResult
            {
                FamilyName = source.ApprenticeSurname,
                GivenNames = source.ApprenticeForename,
                CertificateType = Enums.CertificateType.Framework.ToString(),
                Uln = source.ApprenticeULN,
                CertificateReference = source.CertificateReference,
                FrameworkCertificateNumber = source.FrameworkCertificateNumber,
                CourseName = source.FrameworkName,
                CourseOption = source.PathwayName,
                CourseLevel = source.ApprenticeshipLevelName,
                DateAwarded = source.CertificationDate,
                ProviderName = source.ProviderName,
                EmployerName = source.EmployerName,
                StartDate = source.ApprenticeStartdate,
                QualificationsAndAwardingBodies = source.QualificationsAndAwardingBodies?.Select(q => new QualificationDetails { Name = q.Name, AwardingBody = q.AwardingBody }).ToList(),
                PrintRequestedAt = source.PrintRequestedAt,
                PrintRequestedBy = source.PrintRequestedBy,
                DeliveryInformation = BuildDeliveryInformation(source.CertificateLogs),
            };
        }

        private static List<DeliveryInformation> BuildDeliveryInformation(List<CertificateLog> logs)
        {
            if (logs == null || logs.Count == 0)
                return null;

            return logs
                .Where(log => CertificateConstants.DeliveryInformationStatuses.Any(entry => entry.Action == log.Action && entry.Status == log.Status))
                .GroupBy(log => (log.Action, log.Status))
                .Select(group => group.OrderByDescending(log => log.EventTime).First())
                .OrderByDescending(log => log.EventTime)
                .Select(log => new DeliveryInformation
                {
                    Id = log.Id,
                    Action = log.Action,
                    Status = log.Status,
                    EventTime = log.EventTime,
                })
                .ToList();
        }
    }

    public class QualificationDetails
    {
        public string Name { get; set; }
        public string AwardingBody { get; set; }
    }
}

