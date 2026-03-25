using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Recruit.InnerApi.Recruit.Requests;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.Apim.Shared.Infrastructure;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.Recruit.Application.User.Commands.UpsertUser;

public class UpsertUserCommandHandler(IRecruitApiClient<RecruitApiConfiguration> apiClient) : IRequestHandler<UpsertUserCommand>
{
    public async Task Handle(UpsertUserCommand request, CancellationToken cancellationToken)
    {
        await apiClient.PutWithResponseCode<NullResponse>(
            new PutUserRequest(request.Id, request.User));
    }
}