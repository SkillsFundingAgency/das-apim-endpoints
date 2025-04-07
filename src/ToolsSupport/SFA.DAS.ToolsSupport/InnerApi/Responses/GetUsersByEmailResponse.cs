﻿namespace SFA.DAS.ToolsSupport.InnerApi.Responses;

public class GetUsersByEmailResponse
{
    public List<UserProfile> Users { get; set; } = new ();
}

public class UserProfile
{
    public Guid Id { get; set; }
    public string? DisplayName { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? GovUkIdentifier { get; set; }
    public bool IsSuspended { get; set; }
}

