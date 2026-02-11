using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.DigitalCertificates.InnerApi.Requests
{
    public class GetSharingByIdRequest : IGetApiRequest
    {
        public Guid SharingId { get; }
        public int? Limit { get; }

        public GetSharingByIdRequest(Guid sharingId, int? limit)
        {
            SharingId = sharingId;
            Limit = limit;
        }

        public string GetUrl => $"api/sharing/{SharingId}?limit={Limit}";
    }
}
