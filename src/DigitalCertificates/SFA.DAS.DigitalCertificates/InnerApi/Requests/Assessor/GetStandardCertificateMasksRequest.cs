using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;

namespace SFA.DAS.DigitalCertificates.InnerApi.Requests.Assessor
{
    public class GetStandardCertificateMasksRequest : IGetApiRequest
    {
        public IEnumerable<long> Exclude { get; }

        public GetStandardCertificateMasksRequest(IEnumerable<long> exclude)
        {
            Exclude = exclude;
        }

        public string GetUrl
        {
            get
            {
                var url = "api/v1/certificates/masks?";
                var first = true;
                if (Exclude != null)
                {
                    foreach (var uln in Exclude)
                    {
                        if (!first) url += "&";
                        url += $"exclude={uln}";
                        first = false;
                    }
                }
                return url;
            }
        }
    }
}
