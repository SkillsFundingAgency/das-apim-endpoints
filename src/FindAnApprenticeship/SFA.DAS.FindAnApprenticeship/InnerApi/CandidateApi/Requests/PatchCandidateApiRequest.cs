using System;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;

public class PatchCandidateApiRequest(Guid id, JsonPatchDocument<PatchableCandidate> data) : IPatchApiRequest<JsonPatchDocument<PatchableCandidate>>
{
    public string PatchUrl => $"api/candidates/{id}";
    public JsonPatchDocument<PatchableCandidate> Data { get; set; } = data;
}

public class PatchableCandidate
{
    public Guid Id { get; set; }
    public string? GovUkIdentifier { get; set; }
    public string? LastName { get; set; }
    public string? FirstName { get; set; }
    public string? MiddleNames { get; set; }
    public string? PhoneNumber { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public DateTime? UpdatedOn { get; set; }
    public DateTime? TermsOfUseAcceptedOn { get; set; }
    public string? Email { get; set; }
}