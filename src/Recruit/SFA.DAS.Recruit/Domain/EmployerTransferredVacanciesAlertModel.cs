using System.Collections.Generic;

namespace SFA.DAS.Recruit.Domain;
public record EmployerTransferredVacanciesAlertModel
{
    public int TransferredVacanciesCount { get; init; }
    public List<string> TransferredVacanciesProviderNames { get; init; } = [];
}