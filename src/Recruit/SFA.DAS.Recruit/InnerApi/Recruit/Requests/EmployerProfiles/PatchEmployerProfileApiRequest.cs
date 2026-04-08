using Microsoft.AspNetCore.JsonPatch;
using SFA.DAS.Recruit.InnerApi.Models;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.InnerApi.Recruit.Requests.EmployerProfiles;

public class PatchEmployerProfileApiRequest(long accountLegalEntityId, JsonPatchDocument<EmployerProfile> patchDocument) : IPatchApiRequest<JsonPatchDocument<EmployerProfile>>
{
    public string PatchUrl => $"api/employer/profiles/{accountLegalEntityId}";
    public JsonPatchDocument<EmployerProfile> Data { get; set; } = patchDocument;
}
