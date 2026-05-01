using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.DigitalCertificates.InnerApi.Requests.Assessor
{
    public class GetCertificatesRequest : IGetApiRequest
    {
        public long Uln { get; set; }

        public GetCertificatesRequest(long uln)
        {
            Uln = uln;
        }

        public string GetUrl => $"api/v1/certificates/uln/{Uln}";
    }
}
