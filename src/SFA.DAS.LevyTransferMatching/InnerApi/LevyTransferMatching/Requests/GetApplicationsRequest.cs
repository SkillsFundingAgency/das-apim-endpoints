using System.Text;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests
{
    public class GetApplicationsRequest : IGetApiRequest
    {
        public int? PledgeId { get; set; }
        public long? AccountId { get; set; }

        public string GetUrl
        {
            get
            {
                var sb = new StringBuilder("applications");

                if (PledgeId.HasValue)
                {
                    sb.Append($"?pledgeId={PledgeId}");
                }

                if (!AccountId.HasValue)
                {
                    return sb.ToString();
                }
                
                if (PledgeId.HasValue)
                {
                    sb.Append("&");
                }

                sb.Append($"?accountId={AccountId}");

                return sb.ToString();
            }
        }
    }
}
