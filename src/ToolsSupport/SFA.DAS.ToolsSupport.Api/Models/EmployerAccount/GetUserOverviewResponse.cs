using SFA.DAS.ToolsSupport.Application.Queries.GetUserOverview;
using SFA.DAS.ToolsSupport.InnerApi.Responses;

namespace SFA.DAS.ToolsSupport.Api.Models.EmployerAccount;

public class GetUserOverviewResponse
{
    public string Id { get; set; } = "";
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Email { get; set; } = "";
    public bool IsActive { get; set; }
    public bool IsLocked { get; set; }
    public bool IsSuspended { get; set; }
    public List<AccountSummary> AccountSummaries { get; set; } = [];


    public static explicit operator GetUserOverviewResponse(GetUserOverviewQueryResult source)
    {
        if (source == null) return new();

        return new GetUserOverviewResponse
        {
            Id = source.Id,
            FirstName = source.FirstName,
            LastName = source.LastName,
            Email = source.Email,
            IsActive = source.IsActive,
            IsLocked = source.IsLocked,
            IsSuspended = source.IsSuspended,
            AccountSummaries = source.AccountSummaries
        };
    }
}
