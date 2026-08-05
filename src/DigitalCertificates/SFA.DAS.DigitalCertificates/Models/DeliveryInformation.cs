using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.DigitalCertificates.InnerApi.Responses.Assessor;
using SFA.DAS.DigitalCertificates.Constants;

namespace SFA.DAS.DigitalCertificates.Models
{
    public class DeliveryInformation
    {
        public Guid Id { get; set; }
        public string Action { get; set; }
        public string Status { get; set; }
        public DateTime EventTime { get; set; }

        public static List<DeliveryInformation> FromCertificateLogs(List<CertificateLog> logs)
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

