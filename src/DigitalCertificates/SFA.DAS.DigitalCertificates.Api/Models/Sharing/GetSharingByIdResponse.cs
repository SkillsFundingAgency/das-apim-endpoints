using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.DigitalCertificates.Application.Queries.GetSharingById;

namespace SFA.DAS.DigitalCertificates.Api.Models.Sharing
{
    public class GetSharingByIdResponse
    {
        public SharingByIdDto Response { get; set; }

        public static implicit operator GetSharingByIdResponse(GetSharingByIdQueryResult source)
        {
            if (source == null) return null;

            return new GetSharingByIdResponse
            {
                Response = source.Response == null ? null : new SharingByIdDto
                {
                    UserId = source.Response.UserId,
                    CertificateId = source.Response.CertificateId,
                    CertificateType = source.Response.CertificateType,
                    CourseName = source.Response.CourseName,
                    SharingId = source.Response.SharingId,
                    SharingNumber = source.Response.SharingNumber,
                    CreatedAt = source.Response.CreatedAt,
                    LinkCode = source.Response.LinkCode,
                    ExpiryTime = source.Response.ExpiryTime,
                    SharingAccess = source.Response.SharingAccess?.ToList() ?? new List<DateTime>(),
                    SharingEmails = source.Response.SharingEmails?.Select(e => new SharingEmailDto
                    {
                        SharingEmailId = e.SharingEmailId,
                        EmailAddress = e.EmailAddress,
                        EmailLinkCode = e.EmailLinkCode,
                        SentTime = e.SentTime,
                        SharingEmailAccess = e.SharingEmailAccess?.ToList() ?? new List<DateTime>()
                    }).ToList() ?? new List<SharingEmailDto>()
                }
            };
        }
    }

    public class SharingByIdDto
    {
        public Guid UserId { get; set; }
        public Guid CertificateId { get; set; }
        public string CertificateType { get; set; }
        public string CourseName { get; set; }
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
