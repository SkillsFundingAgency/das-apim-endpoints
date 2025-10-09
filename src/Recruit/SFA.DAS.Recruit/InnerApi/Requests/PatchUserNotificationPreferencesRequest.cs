using System;
using Microsoft.AspNetCore.JsonPatch;
using SFA.DAS.Recruit.Domain;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.InnerApi.Requests;

public class PatchUserNotificationPreferencesRequest(Guid id, JsonPatchDocument<RecruitUser> patch): IPatchApiRequest<JsonPatchDocument<RecruitUser>>
{
    public string PatchUrl => $"api/user/{id}";
    public JsonPatchDocument<RecruitUser> Data { get; set; } = patch;
}