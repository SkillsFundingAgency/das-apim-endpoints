using SFA.DAS.Recruit.Domain;

namespace SFA.DAS.Recruit.InnerApi.Responses;
public record GetProviderAlertsApiResponse
{
    public ProviderTransferredVacanciesAlertModel ProviderTransferredVacanciesAlert { get; set; } = new();
    public WithdrawnVacanciesAlertModel WithdrawnVacanciesAlert { get; set; } = new();
}