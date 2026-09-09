using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.DigitalCertificates.Api.Models.Users
{
    public class GetSharingsResponse
    {
        public Guid UserId { get; set; }
        public Guid CertificateId { get; set; }
        public string CertificateType { get; set; }
        public string CourseName { get; set; }
        public IEnumerable<SharingDto> Sharings { get; set; }

        public static implicit operator GetSharingsResponse(InnerApi.Responses.GetSharingsResponse source)
        {
            if (source == null) return null;
            return new GetSharingsResponse
            {
                UserId = source.UserId,
                CertificateId = source.CertificateId,
                CertificateType = source.CertificateType,
                CourseName = source.CourseName,
                Sharings = source.Sharings?.Select(s => new SharingDto
                {
                    SharingId = s.SharingId,
                    SharingNumber = s.SharingNumber,
                    CreatedAt = s.CreatedAt,
                    LinkCode = s.LinkCode,
                    ExpiryTime = s.ExpiryTime,
                    SharingAccess = s.SharingAccess?.ToList() ?? new List<DateTime>(),
                    SharingEmails = s.SharingEmails?.Select(e => new SharingEmailDto
                    {
                        SharingEmailId = e.SharingEmailId,
                        EmailAddress = e.EmailAddress,
                        EmailLinkCode = e.EmailLinkCode,
                        SentTime = e.SentTime,
                        SharingEmailAccess = e.SharingEmailAccess?.ToList() ?? new List<DateTime>()
                    }).ToList() ?? new List<SharingEmailDto>()
                }).ToList()
            };
        }
    }

    public class SharingDto
    {
        public Guid SharingId { get; set; }
        public int SharingNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid LinkCode { get; set; }
        public DateTime ExpiryTime { get; set; }
        public List<DateTime> SharingAccess { get; set; } = new List<DateTime>();
        public List<SharingEmailDto> SharingEmails { get; set; } = new List<SharingEmailDto>();
    }

    public class SharingEmailDto
    {
        public Guid SharingEmailId { get; set; }
        public string EmailAddress { get; set; }
        public Guid EmailLinkCode { get; set; }
        public DateTime SentTime { get; set; }
        public List<DateTime> SharingEmailAccess { get; set; } = new List<DateTime>();
    }
}
