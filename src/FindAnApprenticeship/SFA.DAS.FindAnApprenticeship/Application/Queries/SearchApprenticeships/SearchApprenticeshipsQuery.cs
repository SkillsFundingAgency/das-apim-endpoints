using System.Collections.Generic;
using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.SearchApprenticeships
{
    public class SearchApprenticeshipsQuery : IRequest<SearchApprenticeshipsResult>
    {
        public string? Location { get; set; }
        public List<string>? SelectedRouteIds { get; set; }

        public int? Distance { get; set; }
    }
}