using System.Collections.Generic;
using SFA.DAS.Campaign.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.Campaign.Application.Queries.Adverts
{
    public class GetAdvertsQueryResult
    {
        public IEnumerable<GetVacanciesListItem> Vacancies { get ; set ; }
        public long TotalFound { get ; set ; }
        public IEnumerable<GetRoutesListItem> Routes { get ; set ; }
        public LocationItem Location { get ; set ; }
    }
}