using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Commitments;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.SelectProvider.Queries;
public class GetSelectProviderQueryHandler : IRequestHandler<GetSelectProviderQuery, GetSelectProviderQueryResult>
{
    private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _apiClient;

    public GetSelectProviderQueryHandler(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient)
    {
        _apiClient = apiClient;
    }
    public async Task<GetSelectProviderQueryResult> Handle(GetSelectProviderQuery request, CancellationToken cancellationToken)
    {
        var aleTask = _apiClient.Get<GetAccountLegalEntityResponse>(new GetAccountLegalEntityRequest(request.AccountLegalEntityId));
        var providersTask = _apiClient.Get<GetProvidersResponse>(new GetProvidersRequest());

        await Task.WhenAll(aleTask, providersTask);

        var ale = await aleTask;
        var providers = await providersTask;

        return new GetSelectProviderQueryResult
        {
            AccountLegalEntity = new AccountLegalEntity { LegalEntityName = ale?.LegalEntityName },
            Providers = providers.Providers
        };
    }
}