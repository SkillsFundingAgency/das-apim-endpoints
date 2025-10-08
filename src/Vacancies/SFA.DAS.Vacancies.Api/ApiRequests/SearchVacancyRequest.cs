using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Vacancies.Api.ApiRequests.Base;

namespace SFA.DAS.Vacancies.Api.ApiRequests
{
    public class SearchVacancyRequest : BaseSearchVacancyRequest
    {
        /// <summary>
        /// If not set returns all
        /// If `true` returns Nation Wide apprenticeship adverts only
        /// If `false` returns non Nation Wide apprenticeship adverts only
        /// </summary>
        [FromQuery]
        public bool? NationWideOnly { get; set; } = null;
    }
}