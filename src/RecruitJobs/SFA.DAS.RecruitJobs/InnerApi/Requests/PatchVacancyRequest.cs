using Microsoft.AspNetCore.JsonPatch;
using SFA.DAS.RecruitJobs.Domain;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RecruitJobs.InnerApi.Requests;

public class PatchVacancyRequest(Guid vacancyId, JsonPatchDocument<Vacancy> data) : IPatchApiRequest<JsonPatchDocument<Vacancy>>
{
    public string PatchUrl => $"api/vacancies/{vacancyId}";
    public JsonPatchDocument<Vacancy> Data { get; set; } = data;
}