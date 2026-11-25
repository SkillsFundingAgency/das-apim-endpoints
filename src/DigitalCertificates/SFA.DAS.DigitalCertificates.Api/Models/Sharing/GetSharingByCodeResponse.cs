using System;
using SFA.DAS.DigitalCertificates.Application.Queries.GetSharingByCode;

namespace SFA.DAS.DigitalCertificates.Api.Models.Sharing
{
    public class GetSharingByCodeResponse
    {
        public SharingByCode Response { get; set; }
        public bool BothFound { get; set; }

        public static implicit operator GetSharingByCodeResponse(GetSharingByCodeQueryResult source)
        {
            if (source == null) return null;
            return new GetSharingByCodeResponse
            {
                Response = source.Response == null ? null : new SharingByCode
                {
                    CertificateId = source.Response.CertificateId,
                    CertificateType = source.Response.CertificateType,
                    ExpiryTime = source.Response.ExpiryTime,
                    SharingId = source.Response.SharingId,
                    SharingEmailId = source.Response.SharingEmailId
                },
                BothFound = source.BothFound
            };
        }
    }

    public class SharingByCode
    {
        public Guid CertificateId { get; set; }
        public string CertificateType { get; set; }
        public DateTime ExpiryTime { get; set; }
        public Guid? SharingId { get; set; }
        public Guid? SharingEmailId { get; set; }
    }
}
