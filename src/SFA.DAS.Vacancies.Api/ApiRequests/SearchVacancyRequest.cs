using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.Vacancies.Api
{
    public class SearchVacancyRequest
    {
        
        [FromQuery]
        public int PageNumber { get; set; } = 1;
        [FromQuery]
        public int PageSize { get; set; } = 10;
        [FromQuery]
        public string AccountLegalEntityPublicHashedId { get; set; } = null;
        [FromQuery]
        public int? Ukprn { get; set; } = null;
        [FromQuery]
        public List<string> Routes { get; set; } = null;
        [FromQuery]
        public double? Lat { get ; set ; } = null;
        [FromQuery]
        public double? Lon { get ; set ; } = null;
        [FromQuery]
        public VacancySort? Sort { get ; set ; } = null;
        [FromQuery]
        public uint? DistanceInMiles { get ; set ; } = null;
        [FromQuery]
        public bool? NationWideOnly { get ; set ; } = null;
        [FromQuery]
        public uint? PostedInLastNumberOfDays { get ; set ; } = null;
        [FromQuery]
        public int? StandardLarsCode { get ; set ; }
    }

    public enum VacancySort
    {
        AgeDesc,
        AgeAsc,
        DistanceDesc,
        DistanceAsc,
        ExpectedStartDateDesc,
        ExpectedStartDateAsc
    }
}