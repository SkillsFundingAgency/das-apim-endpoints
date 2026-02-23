using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.DigitalCertificates.InnerApi.Requests
{
    public class GetSharingByLinkCodeRequest : IGetApiRequest
    {
        public Guid Code { get; }

        public GetSharingByLinkCodeRequest(Guid code)
        {
            Code = code;
        }

        public string GetUrl => $"api/sharing/linkcode/{Code}";
    }
}
