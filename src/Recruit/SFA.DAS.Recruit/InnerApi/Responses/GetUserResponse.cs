using System;
using System.Collections.Generic;
using SFA.DAS.Recruit.Domain;

namespace SFA.DAS.Recruit.InnerApi.Responses;

public class GetUserResponse
{
    public Guid Id { get; set; }
    public string? IdamsUserId { get; set; } 
    public required string UserType { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime LastSignedInDate { get; set; }
    public IList<long> EmployerAccountIds { get; set; } = new List<long>();
    public long? Ukprn { get; set; }
    public DateTime? TransferredVacanciesEmployerRevokedPermissionAlertDismissedOn { get; set; }
    public DateTime? ClosedVacanciesBlockedProviderAlertDismissedOn { get; set; }
    public DateTime? TransferredVacanciesBlockedProviderAlertDismissedOn { get; set; }
    public DateTime? ClosedVacanciesWithdrawnByQaAlertDismissedOn { get; set; }
    public string? DfEUserId { get; set; }
    public NotificationPreferences NotificationPreferences { get; set; }
}