using Microsoft.AspNetCore.JsonPatch;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.SharedOuterApi.Types.Domain.Recruit;
using SFA.DAS.SharedOuterApi.Types.Models;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Recruit;

public class PatchableVacancyDto
{
    public VacancyStatus Status { get; init; }
    public DateTime ApprovedDate { get; init; }
    public DateTime LiveDate { get; init; }
    public List<Address> EmployerLocations { get; set; }
    public GeoCodeMethod? GeoCodeMethod { get; set; }
}

public sealed class PatchVacancyRequest(Guid vacancyId, JsonPatchDocument<PatchableVacancyDto> data): IPatchApiRequest<JsonPatchDocument<PatchableVacancyDto>>
{
    public string PatchUrl { get; } = $"api/vacancies/{vacancyId}";
    public JsonPatchDocument<PatchableVacancyDto> Data { get; set; } = data;
}