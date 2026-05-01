using Microsoft.AspNetCore.JsonPatch.SystemTextJson;
using SFA.DAS.Recruit.InnerApi.Recruit.Requests.EmployerProfiles;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

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

        var patchResponse = await recruitApiClient.PatchWithResponseCode(new PatchEmployerProfileApiRequest(command.AccountLegalEntityId, patch));

        if (patchResponse.StatusCode == HttpStatusCode.NotFound)
        {
            return;
        }

        patchResponse.EnsureSuccessStatusCode();
    }
}