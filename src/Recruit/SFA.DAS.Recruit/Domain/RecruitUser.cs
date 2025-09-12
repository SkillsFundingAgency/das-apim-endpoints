using System;
using System.Collections.Generic;

namespace SFA.DAS.Recruit.Domain;

public class RecruitUser
{
    public Guid Id { get; set; }
    public string? IdamsUserId { get; set; } 
    public required string UserType { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime LastSignedInDate { get; set; }
    public IList<string> EmployerAccountIds { get; set; } = new List<string>();
    public long? Ukprn { get; set; }
    public DateTime? TransferredVacanciesEmployerRevokedPermissionAlertDismissedOn { get; set; }
    public DateTime? ClosedVacanciesBlockedProviderAlertDismissedOn { get; set; }
    public DateTime? TransferredVacanciesBlockedProviderAlertDismissedOn { get; set; }
    public DateTime? ClosedVacanciesWithdrawnByQaAlertDismissedOn { get; set; }
    public string? DfEUserId { get; set; }
    public NotificationPreferences NotificationPreferences { get; set; }
}