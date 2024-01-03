using System.Collections.Generic;
using MediatR;
using SFA.DAS.FindAnApprenticeship.Models;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.SearchApprenticeships
{
    public record SearchApprenticeshipsQuery : IRequest<SearchApprenticeshipsResult>
    {
        public string? Location { get; init; }
        public IReadOnlyCollection<string>? SelectedRouteIds { get; init; }
        public int? Distance { get; init; }
        public string? SearchTerm { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public VacancySort Sort { get; set; } = VacancySort.DistanceAsc;
    }
}