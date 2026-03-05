using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.DigitalCertificates.InnerApi.Requests
{
    public class GetSharingsRequest : IGetApiRequest
    {
        public Guid UserId { get; }
        public Guid CertificateId { get; }
        public int? Limit { get; }

        public GetSharingsRequest(Guid userId, Guid certificateId, int? limit)
        {
            UserId = userId;
            CertificateId = certificateId;
            Limit = limit;
        }

        public string GetUrl => $"api/users/{UserId}/sharings?certificateId={CertificateId}&limit={Limit}";
    }
}
