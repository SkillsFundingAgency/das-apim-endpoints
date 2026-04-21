using Microsoft.AspNetCore.JsonPatch;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.RecruitJobs.InnerApi.Requests;

public class PatchVacancyRequest(Guid vacancyId, JsonPatchDocument<Domain.Vacancy> data) : IPatchApiRequest<JsonPatchDocument<Domain.Vacancy>>
{
    public string PatchUrl => $"api/vacancies/{vacancyId}";
    public JsonPatchDocument<Domain.Vacancy> Data { get; set; } = data;
}