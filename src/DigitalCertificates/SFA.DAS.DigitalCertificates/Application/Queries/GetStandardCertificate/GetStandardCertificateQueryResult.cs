using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.DigitalCertificates.Constants;
using SFA.DAS.DigitalCertificates.InnerApi.Responses.Assessor;
using SFA.DAS.DigitalCertificates.Models;

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
        public List<DeliveryInformation> DeliveryInformation { get; set; }

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
                DeliveryInformation = BuildDeliveryInformation(source.CertificateLogs),
                PrintRequestedAt = source.PrintRequestedAt,
                PrintRequestedBy = source.PrintRequestedBy,
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
}

