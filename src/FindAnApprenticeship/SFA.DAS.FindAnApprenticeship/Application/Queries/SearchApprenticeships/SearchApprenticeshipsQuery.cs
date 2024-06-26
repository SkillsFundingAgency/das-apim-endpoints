using System;
using System.Collections.Generic;
using MediatR;
using SFA.DAS.FindAnApprenticeship.Domain.Models;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.SearchApprenticeships
{
    public record SearchApprenticeshipsQuery : IRequest<SearchApprenticeshipsResult>
    {
        public string? Location { get; init; }
        public IReadOnlyCollection<string>? SelectedRouteIds { get; init; }
        public IReadOnlyCollection<string>? SelectedLevelIds { get; init; }
        public int? Distance { get; init; }
        public string? SearchTerm { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public VacancySort Sort { get; set; } = VacancySort.DistanceAsc;
        public bool DisabilityConfident { get; set; }
        public string? CandidateId { get; set; }
    }
}