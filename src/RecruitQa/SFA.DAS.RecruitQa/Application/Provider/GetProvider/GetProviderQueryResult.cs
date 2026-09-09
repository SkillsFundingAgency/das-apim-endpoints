using SFA.DAS.RecruitQa.InnerApi.Responses;

namespace SFA.DAS.RecruitQa.Application.Provider.GetProvider;

public class GetProviderQueryResult
{
    public GetProvidersListItem Provider { get; set; } = new();
}