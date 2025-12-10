using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.DigitalCertificates.InnerApi.Requests
{
    public class GetAuthorisationRequest : IGetApiRequest
    {
        public Guid UserId { get; set; }

        public GetAuthorisationRequest(Guid userId)
        {
            UserId = userId;
        }

        public string GetUrl => $"api/users/{UserId}/authorisation";
    }
}
