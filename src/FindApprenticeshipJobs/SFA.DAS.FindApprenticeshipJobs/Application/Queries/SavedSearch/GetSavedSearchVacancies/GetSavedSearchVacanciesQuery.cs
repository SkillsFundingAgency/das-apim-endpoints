using MediatR;
using SFA.DAS.FindApprenticeshipJobs.Domain.Models;
using SFA.DAS.SharedOuterApi.Domain;

namespace SFA.DAS.FindApprenticeshipJobs.Application.Queries.SavedSearch.GetSavedSearchVacancies;

public record GetSavedSearchVacanciesQuery(
    VacancySort ApprenticeshipSearchResultsSortOrder,
    Guid Id,
    Guid UserId,
    decimal? Distance,
    string? SearchTerm,
    string? Location,
    bool DisabilityConfident,
    bool? ExcludeNational,
    string? Longitude,
    string? Latitude,
    List<int>? SelectedLevelIds,
    List<int>? SelectedRouteIds,
    string? UnSubscribeToken,
    DateTime LastRunDateFilter,
    int PageNumber,
    int PageSize,
    List<ApprenticeshipTypes>? SelectedApprenticeshipTypes) : IRequest<GetSavedSearchVacanciesQueryResult?>;