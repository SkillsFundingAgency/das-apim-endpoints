using System.Web;
using Microsoft.AspNetCore.WebUtilities;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.RecruitQa.InnerApi.Requests;

public class GetVacancyReviewsByUserRequest(string userId, DateTime? assignationExpiry, string? reviewStatus) : IGetApiRequest
{
    public string GetUrl => QueryHelpers.AddQueryString("api/users/vacancyreviews", new Dictionary<string, string?>
    {
        ["assignationExpiry"] = assignationExpiry is null ? null : $"{assignationExpiry:O}",
        ["status"] = reviewStatus,
        ["userId"] = userId,
    });
}
