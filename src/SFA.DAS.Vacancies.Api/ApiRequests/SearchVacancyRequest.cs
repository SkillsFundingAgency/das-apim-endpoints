using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.Vacancies.Api
{
    public class SearchVacancyRequest
    {
        /// <summary>
        /// Page number you wish to get - defaults to 1
        /// </summary>
        [FromQuery]
        public int PageNumber { get; set; } = 1;
        /// <summary>
        /// Page size you wish to get - defaults to 10
        /// </summary>
        [FromQuery]
        public int PageSize { get; set; } = 10;
        /// <summary>
        /// If `FilterBySubscription` is `true` then you can supply the `AccountLegalEntityPublicHashedId` you wish to filter by, obtained from `GET AccountLegalEntities`. You can only supply a value that is linked to your account, or as a Provider you have permission to act on behalf of.
        /// </summary>
        [FromQuery]
        public string AccountLegalEntityPublicHashedId { get; set; } = null;
        /// <summary>
        /// The UKPRN which you wish to return adverts by. If `FilterBySubscription` is `true` and you have a Provider subscription, the value in your subscription will be used.
        /// </summary>
        [FromQuery]
        public int? Ukprn { get; set; } = null;
        /// <summary>
        /// You can supply a maximum of 3 routes to be filtered by, these can be obtained from `GET referencedata/courses/routes`. If there are `StandardLarsCode` values supplied the route filtering will be ignored.
        /// </summary>
        [FromQuery]
        [MaxLength(3, ErrorMessage = "Exceeded maximum of 3 routes to be filtered by")]
        public List<string> Routes { get; set; } = null;
        /// <summary>
        /// Latitude to search from, must be supplied with `Longitude` and `DistanceInMiles`
        /// </summary>
        [FromQuery]
        public double? Lat { get ; set ; } = null;
        /// <summary>
        /// Longitude to search from, must be supplied with `Latitude` and `DistanceInMiles`
        /// </summary>
        [FromQuery]
        public double? Lon { get ; set ; } = null;
        /// <summary>
        /// If not supplied, defaults to `VacancySort.AgeDesc`
        /// `AgeDesc` From newest to oldest apprenticeship adverts
        /// `AgeAsc` From oldest to newest apprenticeship adverts
        /// `DistanceDesc` From furthest to closest away apprenticeship adverts - can only be used if `Lat`, `Lon` and `DistanceInMiles` supplied
        /// `DistanceAsc` From closest to furthest away apprenticeship adverts - can only be used if `Lat`, `Lon` and `DistanceInMiles` supplied
        /// `ExpectedStartDateDesc` Ordering by apprenticeship adverts that are closest to starting
        /// `ExpectedStartDateAsc` Ordering by apprenticeship adverts that are further in the future to starting
        /// </summary>
        [FromQuery]
        public VacancySort? Sort { get ; set ; } = null;
        /// <summary>
        /// To be used with `Lat` and `Lon` to provide apprenticeship adverts that fall into that radius
        /// </summary>
        [FromQuery]
        public uint? DistanceInMiles { get ; set ; } = null;
        /// <summary>
        /// If not set returns all
        /// If `true` returns Nation Wide apprenticeship adverts only
        /// If `false` returns non Nation Wide apprenticeship adverts only
        /// </summary>
        [FromQuery]
        public bool? NationWideOnly { get ; set ; } = null;
        /// <summary>
        /// 
        /// </summary>
        [FromQuery]
        public uint? PostedInLastNumberOfDays { get ; set ; } = null;
        /// <summary>
        /// The Id or Ids of the standard you are searching for - can be obtained from `GET referencedata/courses`. If supplied will cause any route filtering to be ignored
        /// </summary>
        [FromQuery]
        public List<int> StandardLarsCode { get ; set ; }
        /// <summary>
        /// If set to `true` - then filters by the AccountId or UKPRN linked to your subscription.
        /// </summary>
        [FromQuery]
        public bool? FilterBySubscription { get; set; }
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