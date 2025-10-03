using System.Collections.Generic;

namespace SFA.DAS.Recruit.Domain;
public record BlockedProviderAlertModel
{
    public List<string> ClosedVacancies { get; set; } = [];
    public List<string> BlockedProviderNames { get; set; } = [];
}