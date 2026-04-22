using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;
using System;

namespace SFA.DAS.DigitalCertificates.InnerApi.Requests
{
    public class DeleteSharingRequest : IDeleteApiRequest
    {
        public Guid SharingId { get; }

        public DeleteSharingRequest(Guid sharingId)
        {
            SharingId = sharingId;
        }

        public string DeleteUrl => $"api/sharing/{SharingId}";
    }
}
