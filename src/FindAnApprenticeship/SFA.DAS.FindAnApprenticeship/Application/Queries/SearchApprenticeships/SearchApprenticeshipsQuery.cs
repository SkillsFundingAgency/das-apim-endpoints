using System.Collections.Generic;
using MediatR;
using SFA.DAS.FindAnApprenticeship.Models;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.SearchApprenticeships
{
    public class SearchApprenticeshipsQuery : IRequest<SearchApprenticeshipsResult>
    {
        public string? Location { get; set; }
        public List<string>? SelectedRouteIds { get; set; }
        public int? Distance { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public VacancySort Sort { get; set; } = VacancySort.DistanceAsc;
    }
}