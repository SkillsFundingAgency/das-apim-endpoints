using SFA.DAS.Recruit.Domain;
using SFA.DAS.Recruit.InnerApi.Responses.Base;

namespace SFA.DAS.Recruit.InnerApi.Responses;

public record GetEmployerDashboardApiResponse : DashboardApiResponse
{
    public EmployerTransferredVacanciesAlertModel EmployerRevokedTransferredVacanciesAlert { get; set; } = new();
    public EmployerTransferredVacanciesAlertModel BlockedProviderTransferredVacanciesAlert { get; set; } = new();
    public BlockedProviderAlertModel BlockedProviderAlert { get; set; } = new();
    public WithdrawnVacanciesAlertModel WithDrawnByQaVacanciesAlert { get; set; } = new();
}