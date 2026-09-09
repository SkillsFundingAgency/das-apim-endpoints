using System;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;
using SFA.DAS.Recruit.Domain;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.Recruit.InnerApi.Requests;

public class PatchUserNotificationPreferencesRequest(Guid id, JsonPatchDocument<RecruitUser> patch): IPatchApiRequest<JsonPatchDocument<RecruitUser>>
{
    public string PatchUrl => $"api/user/{id}";
    public JsonPatchDocument<RecruitUser> Data { get; set; } = patch;
}