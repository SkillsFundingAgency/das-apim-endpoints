using Microsoft.AspNetCore.WebUtilities;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.LevyTransferMatching;

public class GetApplicationsForAutomaticApprovalRequest : IGetApiRequest
{
    public int? PledgeId { get; set; }

    public string GetUrl
    {
        get
        {
            if (!PledgeId.HasValue)
            {
                return "applications/for-automatic-approval";
            }

            return QueryHelpers.AddQueryString(
                "applications/for-automatic-approval",
                new Dictionary<string, string> { { "pledgeId", PledgeId.Value.ToString() } });
        }
    }
}
