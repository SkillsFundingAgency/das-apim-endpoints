using SFA.DAS.Recruit.Domain;
using SFA.DAS.Recruit.InnerApi.Responses.Base;

namespace SFA.DAS.Recruit.InnerApi.Responses;

public record GetProviderDashboardApiResponse : DashboardApiResponse
{
    public ProviderTransferredVacanciesAlertModel ProviderTransferredVacanciesAlert { get; set; } = new();
    public WithdrawnVacanciesAlertModel WithdrawnVacanciesAlert { get; set; } = new();
}