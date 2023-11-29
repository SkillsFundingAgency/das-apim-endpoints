using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using SFA.DAS.FindAnApprenticeship.Application.Queries.SearchApprenticeships;
using SFA.DAS.FindAnApprenticeship.Domain;

namespace SFA.DAS.FindAnApprenticeship.Api.Models
{
    public class GetSearchApprenticeshipsModel
    {
        [FromQuery] public List<string>? RouteIds { get; set; }
        [FromQuery] public string? Location { get; set; }
        [FromQuery] public int? Distance { get; set; }
        [FromQuery] public int PageNumber { get; set; } = 1;
        [FromQuery] public int PageSize { get; set; } = Constants.SearchApprenticeships.PageSize;

        public static implicit operator SearchApprenticeshipsQuery(GetSearchApprenticeshipsModel model) => new()
        {
            SelectedRouteIds = model.RouteIds,
            Location = model.Location,
            Distance = model.Distance,
            PageNumber = model.PageNumber,
            PageSize = model.PageSize
        };
    }
}
