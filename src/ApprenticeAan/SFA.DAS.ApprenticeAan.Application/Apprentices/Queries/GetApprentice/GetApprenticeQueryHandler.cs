using MediatR;
using SFA.DAS.ApprenticeAan.Api.Configuration;
using SFA.DAS.ApprenticeAan.Application.InnerApi.Apprentices;
using SFA.DAS.ApprenticeAan.Application.Services;

namespace SFA.DAS.ApprenticeAan.Application.Apprentices.Queries.GetApprentice;

public class GetApprenticeQueryHandler : IRequestHandler<GetApprenticeQuery, GetApprenticeQueryResult?>
{
    private readonly IAanHubApiClient<AanHubApiConfiguration> _apiClient;

    public GetApprenticeQueryHandler(IAanHubApiClient<AanHubApiConfiguration> apiClient)
    {
        _apiClient = apiClient;
    }

    public Task<GetApprenticeQueryResult?> Handle(GetApprenticeQuery request, CancellationToken cancellationToken)
    {
        return _apiClient.Get<GetApprenticeQueryResult?>(new GetApprenticeRequest(request.ApprenticeId));
    }
}

