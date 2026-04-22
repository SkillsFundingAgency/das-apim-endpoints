using SFA.DAS.Recruit.InnerApi.Recruit.Requests.EmployerProfiles;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

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
