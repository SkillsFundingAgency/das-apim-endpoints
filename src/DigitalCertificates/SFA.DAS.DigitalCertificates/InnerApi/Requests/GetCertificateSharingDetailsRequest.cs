using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.DigitalCertificates.InnerApi.Requests
{
    public class GetCertificateSharingDetailsRequest : IGetApiRequest
    {
        public Guid UserId { get; }
        public Guid CertificateId { get; }
        public int? Limit { get; }

        public GetCertificateSharingDetailsRequest(Guid userId, Guid certificateId, int? limit)
        {
            UserId = userId;
            CertificateId = certificateId;
            Limit = limit;
        }

        public string GetUrl => $"api/sharing?user={UserId}&certificateid={CertificateId}&limit={Limit}";
    }
}
