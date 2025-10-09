using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Recruit.InnerApi.Recruit.Requests;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.Application.User.Commands.UpsertUser;

public class UpsertUserCommandHandler(IRecruitApiClient<RecruitApiConfiguration> apiClient) : IRequestHandler<UpsertUserCommand>
{
    public async Task Handle(UpsertUserCommand request, CancellationToken cancellationToken)
    {
        await apiClient.PutWithResponseCode<NullResponse>(
            new PutUserRequest(request.Id, request.User));
    }
}