using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.DigitalCertificates.InnerApi.Requests
{
    public class GetUserActionsRequest : IGetApiRequest
    {
        public Guid UserId { get; set; }

        public GetUserActionsRequest(Guid userId)
        {
            UserId = userId;
        }

        public string GetUrl => $"api/users/{UserId}/actions";
    }
}
