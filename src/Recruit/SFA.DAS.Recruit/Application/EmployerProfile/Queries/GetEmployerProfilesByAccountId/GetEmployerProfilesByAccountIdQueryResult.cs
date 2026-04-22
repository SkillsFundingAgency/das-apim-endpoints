using System.Collections.Generic;

namespace SFA.DAS.Recruit.Application.EmployerProfile.Queries.GetEmployerProfilesByAccountId;

public record GetEmployerProfilesByAccountIdQueryResult
{
    public List<InnerApi.Models.EmployerProfile> EmployerProfiles { get; set; } = [];
}