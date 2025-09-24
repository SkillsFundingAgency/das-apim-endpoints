using MediatR;
using SFA.DAS.FindAnApprenticeship.Domain.Models;
using System;
using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Domain;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.SearchApprenticeships;

public record SearchApprenticeshipsQuery : IRequest<SearchApprenticeshipsResult>
{
    public string? Location { get; init; }
    public IReadOnlyCollection<int>? SelectedRouteIds { get; init; }
    public IReadOnlyCollection<int>? SelectedLevelIds { get; init; }
    public int? Distance { get; init; }
    public string? SearchTerm { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public VacancySort Sort { get; set; } = VacancySort.DistanceAsc;
    public WageType? SkipWageType { get; set; } = null;
    public bool DisabilityConfident { get; set; }
    public Guid? CandidateId { get; set; }
    public bool? ExcludeNational { get; set; }
    public List<ApprenticeshipTypes> ApprenticeshipTypes { get; set; }
}