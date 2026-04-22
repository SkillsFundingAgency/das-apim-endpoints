using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;
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
