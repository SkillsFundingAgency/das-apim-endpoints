using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.DigitalCertificates.InnerApi.Requests
{
    public class GetSharingByEmailLinkCodeRequest : IGetApiRequest
    {
        public Guid Code { get; }

        public GetSharingByEmailLinkCodeRequest(Guid code)
        {
            Code = code;
        }

        public string GetUrl => $"api/sharing/emaillinkcode/{Code}";
    }
}
