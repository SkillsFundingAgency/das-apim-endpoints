using System.Collections.Generic;
using MediatR;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.Vacancies.Application.Vacancies.Queries
{
    public class GetTraineeshipVacanciesQuery : IRequest<GetTraineeshipVacanciesQueryResult>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int? Ukprn { get; set; }
        public string AccountPublicHashedId { get; set; }
        public string AccountLegalEntityPublicHashedId { get; set; }
        public AccountIdentifier AccountIdentifier { get ; set ; }
        public List<int> RouteId { get ; set ; }
        public bool? NationWideOnly { get ; set ; }
        public double? Lat { get ; set ; }
        public double? Lon { get ; set ; }
        public uint? DistanceInMiles { get ; set ; }
        public List<string> Routes { get ; set ; }
        public uint? PostedInLastNumberOfDays { get ; set ; }
        public string Sort { get ; set ; }
    }
}
