using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;
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
