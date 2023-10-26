using MediatR;
using SFA.DAS.ApprenticeAan.Application.Infrastructure;

namespace SFA.DAS.ApprenticeAan.Application.MemberProfiles;
public class UpdateMemberProfilesCommandHandler : IRequestHandler<UpdateMemberProfilesCommand, Unit>
{
    private readonly IAanHubRestApiClient _aanHubRestApiClient;

    public UpdateMemberProfilesCommandHandler(IAanHubRestApiClient aanHubRestApiClient)
    {
        _aanHubRestApiClient = aanHubRestApiClient;
    }

    public async Task<Unit> Handle(UpdateMemberProfilesCommand request, CancellationToken cancellationToken)
    {
        await _aanHubRestApiClient.PutMemberProfile(request.MemberId, request.RequestedByMemberId, request.MemberProfile, cancellationToken);
        return Unit.Value;
    }
}
