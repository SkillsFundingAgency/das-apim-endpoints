using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.Recruit.Api.Models.Requests;

public record VacancyListFilterParams
{
    [FromQuery] public string? SearchTerm { get; init; } = string.Empty;
}