using System.Collections.Generic;

namespace SFA.DAS.Recruit.Domain;
public record ProviderTransferredVacanciesAlertModel
{
    public List<string> LegalEntityNames { get; set; } = [];
}