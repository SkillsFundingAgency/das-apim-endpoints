using MediatR;
using SFA.DAS.FindApprenticeshipJobs.Domain.Models;

namespace SFA.DAS.FindApprenticeshipJobs.Application.Queries.SavedSearch.GetSavedSearchVacancies;

public record GetSavedSearchVacanciesQuery(
    VacancySort ApprenticeshipSearchResultsSortOrder,
    Guid Id,
    Guid UserId,
    decimal? Distance,
    string? SearchTerm,
    string? Location,
    bool DisabilityConfident,
    string? Longitude,
    string? Latitude,
    List<int>? SelectedLevelIds,
    List<int>? SelectedRouteIds,
    string? UnSubscribeToken,
    DateTime LastRunDateFilter,
    int PageNumber,
    int PageSize) : IRequest<GetSavedSearchVacanciesQueryResult?>;