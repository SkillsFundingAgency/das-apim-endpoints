
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;
using SFA.DAS.AdminRoatp.Application.Commands.PatchProviderAllowedCourse;
using SFA.DAS.AdminRoatp.InnerApi.Models;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.AdminRoatp.InnerApi.Requests;

public class PatchProviderAllowedCourseRequest(PatchProviderAllowedCourseCommand command) : IPatchApiRequest<JsonPatchDocument<PatchProviderAllowedCourseModel>>
{
    public string PatchUrl { get; } = $"providers/{command.Ukprn}/allowed-courses/{command.LarsCode}?userId={Uri.EscapeDataString(command.UserId)}&userDisplayName={Uri.EscapeDataString(command.UserDisplayName)}";
    public JsonPatchDocument<PatchProviderAllowedCourseModel> Data { get; set; } = CreatePatchDoc(command);

    private static JsonPatchDocument<PatchProviderAllowedCourseModel> CreatePatchDoc(
        PatchProviderAllowedCourseCommand command)
    {
        var patchDoc = new JsonPatchDocument<PatchProviderAllowedCourseModel>();
        patchDoc.Replace(x => x.LastDateStarts, command.LastDateStarts);
        return patchDoc;
    }
}
