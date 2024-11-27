using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.FindAnApprenticeship.Application.Queries.SearchApprenticeships;
using SFA.DAS.FindAnApprenticeship.Domain;
using SFA.DAS.FindAnApprenticeship.Domain.Models;

namespace SFA.DAS.FindAnApprenticeship.Api.Models
{
    public class GetSearchApprenticeshipsModel
    {
        [FromQuery] public List<string>? RouteIds { get; set; }
        [FromQuery] public List<string>? LevelIds { get; set; }
        [FromQuery] public string? Location { get; set; }
        [FromQuery] public int? Distance { get; set; }
        [FromQuery] public int? PageNumber { get; set; }
        [FromQuery] public int? PageSize { get; set; }
        [FromQuery] public string? SearchTerm { get; set; }
        [FromQuery] public VacancySort? Sort { get; set; }
        [FromQuery] public WageType? SkipWageType { get; set; } = null;
        [FromQuery] public bool DisabilityConfident { get; set; }
        [FromQuery] public string? CandidateId { get; set; }
        

        public static implicit operator SearchApprenticeshipsQuery(GetSearchApprenticeshipsModel model) => new()
        {
            SelectedRouteIds = model.RouteIds?.Select(c=>Convert.ToInt32(c)).ToList(),
            Location = model.Location,
            Distance = model.Distance,
            Sort = model.Sort ?? Constants.SearchApprenticeships.DefaultSortOrder,
            SkipWageType = model.SkipWageType,
            PageNumber = model.PageNumber is null or <= 0 ? Constants.SearchApprenticeships.DefaultPageNumber : (int)model.PageNumber,
            PageSize = model.PageSize is null or <= 0 ? Constants.SearchApprenticeships.DefaultPageSize : (int)model.PageSize,
            SearchTerm = model.SearchTerm,
            SelectedLevelIds = model.LevelIds?.Select(c=>Convert.ToInt32(c)).ToList(),
            DisabilityConfident = model.DisabilityConfident,
            CandidateId = model.CandidateId != null ? Guid.Parse(model.CandidateId) : null
        };
    }
}
