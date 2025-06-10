using System;
using Microsoft.AspNetCore.JsonPatch;
using SFA.DAS.Recruit.Enums;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.InnerApi.Requests;

public class PatchApplicationApiRequest : IPatchApiRequest<JsonPatchDocument<Application>>
{
    private readonly Guid _applicationId;
    private readonly Guid _candidateId;
    public JsonPatchDocument<Application> Data { get; set; }

    public PatchApplicationApiRequest(
        Guid applicationId,
        Guid candidateId,
        JsonPatchDocument<Application> data)
    {
        _applicationId = applicationId;
        _candidateId = candidateId;
        Data = data;
    }

    public string PatchUrl => $"api/Candidates/{_candidateId}/applications/{_applicationId}";
}
public class Application
{
    public string ResponseNotes { get; set; }
    public ApplicationStatus Status { get; set; }
}