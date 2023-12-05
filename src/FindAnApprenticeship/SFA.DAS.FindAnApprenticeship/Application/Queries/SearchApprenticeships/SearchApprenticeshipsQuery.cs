using System.Collections.Generic;
using System.Drawing;
using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.SearchApprenticeships
{
    public class SearchApprenticeshipsQuery : IRequest<SearchApprenticeshipsResult>
    {
        public string? Location { get; set; }
        public List<string>? SelectedRouteIds { get; set; }
        public int? Distance { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string Sort = "DistanceAsc";
    }
}