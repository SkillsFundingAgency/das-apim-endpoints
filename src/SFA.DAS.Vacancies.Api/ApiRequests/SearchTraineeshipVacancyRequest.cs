using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.Vacancies.Api
{
    public class SearchTraineeshipVacancyRequest
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
        /// Latitude to search from, must be supplied with `Longitude` and `DistanceInMiles`
        /// </summary>
        [FromQuery]
        public double? Lat { get; set; } = null;
        /// <summary>
        /// Longitude to search from, must be supplied with `Latitude` and `DistanceInMiles`
        /// </summary>
        [FromQuery]
        public double? Lon { get; set; } = null;
        /// <summary>
        /// If not supplied, defaults to `VacancySort.AgeDesc`
        /// `AgeDesc` From newest to oldest traineeship adverts
        /// `AgeAsc` From oldest to newest traineeship adverts
        /// `DistanceDesc` From furthest to closest away traineeship adverts - can only be used if `Lat`, `Lon` and `DistanceInMiles` supplied
        /// `DistanceAsc` From closest to furthest away traineeship adverts - can only be used if `Lat`, `Lon` and `DistanceInMiles` supplied
        /// `ExpectedStartDateDesc` Ordering by traineeship adverts that are closest to starting
        /// `ExpectedStartDateAsc` Ordering by traineeship adverts that are further in the future to starting
        /// </summary>
        [FromQuery]
        public VacancySort? Sort { get; set; } = null;
        /// <summary>
        /// To be used with `Lat` and `Lon` to provide traineeship adverts that fall into that radius
        /// </summary>
        [FromQuery]
        public uint? DistanceInMiles { get; set; } = null;
        /// <summary>
        /// If not set returns all
        /// If `true` returns Nation Wide traineeship adverts only
        /// If `false` returns non Nation Wide traineeship adverts only
        /// </summary>
        [FromQuery]
        public bool? NationWideOnly { get; set; } = null;
        /// <summary>
        /// Traineeship adverts Posted In Last Number Of Days you wish to get.
        /// </summary>
        [FromQuery]
        public uint? PostedInLastNumberOfDays { get; set; } = null;
        /// <summary>
        /// The Id or Ids of the standard you are searching for - can be obtained from `GET referencedata/courses`. If supplied will cause any route filtering to be ignored
        /// </summary>
        [FromQuery]
        public List<int> RouteIds { get; set; }
        /// <summary>
        /// If set to `true` - then filters by the AccountId or UKPRN linked to your subscription.
        /// </summary>
        [FromQuery]
        public bool? FilterBySubscription { get; set; }
    }

    public enum TraineeshipVacancySort
    {
        AgeDesc,
        AgeAsc,
        DistanceDesc,
        DistanceAsc,
        ExpectedStartDateDesc,
        ExpectedStartDateAsc
    }
}