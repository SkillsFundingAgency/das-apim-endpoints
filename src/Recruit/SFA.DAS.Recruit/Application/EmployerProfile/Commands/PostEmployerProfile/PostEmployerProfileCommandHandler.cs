using SFA.DAS.Recruit.InnerApi.Recruit.Requests.EmployerProfiles;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Recruit.Application.EmployerProfile.Commands.PostEmployerProfile;

public class PostEmployerProfileCommandHandler(IRecruitApiClient<RecruitApiConfiguration> recruitApiClient) : MediatR.IRequestHandler<PostEmployerProfileCommand>
{
    public async Task Handle(PostEmployerProfileCommand command, CancellationToken cancellationToken)
    {
        var response = await recruitApiClient.PutWithResponseCode<InnerApi.Models.EmployerProfile>(new PostEmployerProfileApiRequest(command.AccountLegalEntityId,
            new PostEmployerProfileApiRequest.PostEmployerProfileApiRequestData
            {
                AccountId = command.AccountId,
                TradingName = command.TradingName,
                AboutOrganisation = command.AboutOrganisation
            }));

        response.EnsureSuccessStatusCode();
    }
}
