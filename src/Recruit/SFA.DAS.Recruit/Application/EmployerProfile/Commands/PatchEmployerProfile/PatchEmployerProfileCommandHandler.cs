using Microsoft.AspNetCore.JsonPatch;
using SFA.DAS.Recruit.InnerApi.Recruit.Requests.EmployerProfiles;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Recruit.Application.EmployerProfile.Commands.PatchEmployerProfile;

public class PatchEmployerProfileCommandHandler(IRecruitApiClient<RecruitApiConfiguration> recruitApiClient) : MediatR.IRequestHandler<PatchEmployerProfileCommand>
{
    public async Task Handle(PatchEmployerProfileCommand command, CancellationToken cancellationToken)
    {
        var patch = new JsonPatchDocument<InnerApi.Models.EmployerProfile>();

        if (command.EmployerProfile.TradingName != null)
            patch.Replace(x => x.TradingName, command.EmployerProfile.TradingName);

        if (command.EmployerProfile.AboutOrganisation != null)
            patch.Replace(x => x.AboutOrganisation, command.EmployerProfile.AboutOrganisation);

        if (command.EmployerProfile.Addresses != null)
            patch.Replace(x => x.Addresses, command.EmployerProfile.Addresses);

        await recruitApiClient.PatchWithResponseCode(new PatchEmployerProfileApiRequest(command.AccountLegalEntityId, patch));
    }
}