using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;

namespace SFA.DAS.DigitalCertificates.InnerApi.Requests.Assessor
{
    public class GetFrameworkCertificateMasksRequest : IGetApiRequest
    {
        public IEnumerable<long> Exclude { get; }

        public GetFrameworkCertificateMasksRequest(IEnumerable<long> exclude)
        {
            Exclude = exclude;
        }

        public string GetUrl
        {
            get
            {
                var url = "api/v1/certificates/framework/masks?";
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
