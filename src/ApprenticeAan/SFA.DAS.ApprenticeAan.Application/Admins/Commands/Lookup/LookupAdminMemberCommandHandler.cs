using MediatR;
using SFA.DAS.ApprenticeAan.Application.Infrastructure;

namespace SFA.DAS.ApprenticeAan.Application.Admins.Commands.Lookup;

public class LookupAdminMemberCommandHandler : IRequestHandler<LookupAdminMemberCommand, LookupAdminMemberCommandResult?>
{
    private readonly IAanHubRestApiClient _apiClient;

    public LookupAdminMemberCommandHandler(IAanHubRestApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<LookupAdminMemberCommandResult?> Handle(LookupAdminMemberCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _apiClient.GetMemberByEmail(request.Email, cancellationToken);

        return result.ResponseMessage.StatusCode != System.Net.HttpStatusCode.OK
            ? null
            : new LookupAdminMemberCommandResult { MemberId = result.GetContent().MemberId, Status = result.GetContent().Status };
    }
}