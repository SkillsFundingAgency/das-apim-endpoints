using System;
using Microsoft.AspNetCore.JsonPatch;
using SFA.DAS.SharedOuterApi.Domain.Recruit;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.Recruit;

public class PatchableVacancyDto
{
    public VacancyStatus Status { get; init; }
    public DateTime ApprovedDate { get; init; }
    public DateTime LiveDate { get; init; }
}

public sealed class PatchVacancyRequest(Guid vacancyId, JsonPatchDocument<PatchableVacancyDto> data): IPatchApiRequest<JsonPatchDocument<PatchableVacancyDto>>
{
    public string PatchUrl { get; } = $"api/vacancies/{vacancyId}";
    public JsonPatchDocument<PatchableVacancyDto> Data { get; set; } = data;
}