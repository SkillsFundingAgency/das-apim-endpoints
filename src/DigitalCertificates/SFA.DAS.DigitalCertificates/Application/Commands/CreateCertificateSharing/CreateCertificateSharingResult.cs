using SFA.DAS.DigitalCertificates.InnerApi.Responses;
using System;

namespace SFA.DAS.DigitalCertificates.Application.Commands.CreateCertificateSharing
{
    public class CreateCertificateSharingResult
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

        public static implicit operator CreateCertificateSharingResult(PostCreateSharingResponse response)
        {
            return new CreateCertificateSharingResult
            {
                UserId = response.UserId,
                CertificateId = response.CertificateId,
                CertificateType = response.CertificateType,
                CourseName = response.CourseName,
                SharingId = response.SharingId,
                SharingNumber = response.SharingNumber,
                CreatedAt = response.CreatedAt,
                LinkCode = response.LinkCode,
                ExpiryTime = response.ExpiryTime
            };
        }
    }
}