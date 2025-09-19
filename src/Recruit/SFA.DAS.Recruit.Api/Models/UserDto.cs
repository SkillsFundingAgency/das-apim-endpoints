using System;
using System.Collections.Generic;

namespace SFA.DAS.Recruit.Api.Models;

public class UserDto
{
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
    
    public static explicit operator InnerApi.Recruit.Requests.UserDto(UserDto source)
    {
        return new InnerApi.Recruit.Requests.UserDto
        {
            Name = source.Name,
            IdamsUserId = source.IdamsUserId,
            UserType = source.UserType,
            Email = source.Email,
            CreatedDate = source.CreatedDate,
            LastSignedInDate = source.LastSignedInDate,
            Ukprn = source.Ukprn,
            EmployerAccountIds = source.EmployerAccountIds,
            DfEUserId = source.DfEUserId,
            ClosedVacanciesBlockedProviderAlertDismissedOn = source.ClosedVacanciesBlockedProviderAlertDismissedOn,
            TransferredVacanciesBlockedProviderAlertDismissedOn = source.TransferredVacanciesBlockedProviderAlertDismissedOn,
            ClosedVacanciesWithdrawnByQaAlertDismissedOn = source.ClosedVacanciesWithdrawnByQaAlertDismissedOn,
            TransferredVacanciesEmployerRevokedPermissionAlertDismissedOn = source.TransferredVacanciesEmployerRevokedPermissionAlertDismissedOn
        };
    }
}