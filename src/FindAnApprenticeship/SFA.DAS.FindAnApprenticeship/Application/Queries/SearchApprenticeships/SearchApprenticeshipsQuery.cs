using System.Collections.Generic;
using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.SearchApprenticeships
{
    public record SearchApprenticeshipsQuery : IRequest<SearchApprenticeshipsResult>
    {
        public string? Location { get; init; }
        public IReadOnlyCollection<string>? SelectedRouteIds { get; init; }
        public int? Distance { get; init; }
        public string Sort = "DistanceAsc";
        public int PageNumber { get; init; }
        public int PageSize { get; init; }
        public IReadOnlyCollection<string> Categories { get; init; }
    }
}