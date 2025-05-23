﻿using SFA.DAS.ToolsSupport.InnerApi.Responses;

namespace SFA.DAS.ToolsSupport.Application.Queries.GetUserOverview;

public class GetUserOverviewQueryResult
{
    public string Id { get; set; } = "";
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Email { get; set; } = "";
    public bool IsActive { get; set; }
    public bool IsLocked { get; set; }
    public bool IsSuspended { get; set; }
    public List<AccountSummary> AccountSummaries { get; set; } = [];

}
