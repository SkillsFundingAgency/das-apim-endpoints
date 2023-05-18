using System.Net;
using MediatR;
using SFA.DAS.ApprenticeAan.Api.Configuration;
using SFA.DAS.ApprenticeAan.Application.InnerApi.Apprentices;
using SFA.DAS.ApprenticeAan.Application.Services;

namespace SFA.DAS.ApprenticeAan.Application.Apprentices.Commands.CreateApprenticeMember;

public class CreateApprenticeMemberCommandHandler : IRequestHandler<CreateApprenticeMemberCommand, Unit>
{
    private readonly IAanHubApiClient<AanHubApiConfiguration> _apiClient;

    public CreateApprenticeMemberCommandHandler(IAanHubApiClient<AanHubApiConfiguration> apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<Unit> Handle(CreateApprenticeMemberCommand request, CancellationToken cancellationToken)
    {
        var result = await _apiClient.PostWithResponseCode<object>(new PostApprenticeRequest(request), true);
        if (result.StatusCode == HttpStatusCode.Created) return Unit.Value;
        throw new InvalidOperationException($"An attempt to create member for apprentice: {request.ApprenticeId}, came back with unsuccessful response status: {result.StatusCode}");
    }
}