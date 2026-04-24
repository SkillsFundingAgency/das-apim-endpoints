using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.FindApprenticeApi.Requests;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.SaveSearch;

public record UnsubscribeSavedSearchCommandHandler(IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration> apiClient) : IRequestHandler<UnsubscribeSavedSearchCommand>
{
    public async Task Handle(UnsubscribeSavedSearchCommand request, CancellationToken cancellationToken)
    {
        await apiClient.Delete(new DeleteSavedSearchRequest(request.Id));
    }
}