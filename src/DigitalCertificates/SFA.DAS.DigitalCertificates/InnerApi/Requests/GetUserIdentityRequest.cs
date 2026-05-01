using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.DigitalCertificates.InnerApi.Requests
{
    public class GetUserIdentityRequest : IGetApiRequest
    {
        public Guid UserId { get; }

        public GetUserIdentityRequest(Guid userId)
        {
            UserId = userId;
        }

        public string GetUrl => $"api/users/{UserId}/identity";
    }
}
