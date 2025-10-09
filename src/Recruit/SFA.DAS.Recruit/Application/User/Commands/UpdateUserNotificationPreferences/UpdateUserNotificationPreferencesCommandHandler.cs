using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using SFA.DAS.Recruit.Domain;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.Application.User.Commands.UpdateUserNotificationPreferences;

public class UpdateUserNotificationPreferencesCommandHandler(IRecruitApiClient<RecruitApiConfiguration> apiClient) : IRequestHandler<UpdateUserNotificationPreferencesCommand, bool>
{
    public async Task<bool> Handle(UpdateUserNotificationPreferencesCommand command, CancellationToken cancellationToken)
    {
        var patch = new JsonPatchDocument<RecruitUser>();
        patch.Replace(x => x.NotificationPreferences, command.NotificationPreferences);
        var response = await apiClient.PatchWithResponseCode(new PatchUserNotificationPreferencesRequest(command.Id, patch));
        return response.StatusCode == HttpStatusCode.OK;
    }
}