using Microsoft.AspNetCore.JsonPatch;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;

public class PatchApplicationApiRequest : IPatchApiRequest<JsonPatchDocument<Domain.Models.Application>>
{
    private readonly Guid _applicationId;
    private readonly Guid _candidateId;
    public JsonPatchDocument<Domain.Models.Application> Data { get; set; }

    public PatchApplicationApiRequest(
        Guid applicationId,
        Guid candidateId,
        JsonPatchDocument<Domain.Models.Application> data)
    {
        _applicationId = applicationId;
        _candidateId = candidateId;
        Data = data;
    }

    public string PatchUrl => $"api/Candidates/{_candidateId}/applications/{_applicationId}";
}