using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Vacancies.Api.ApiRequests.Base;

namespace SFA.DAS.Vacancies.Api.ApiRequests
{
    public class SearchVacancyRequestV2 : BaseSearchVacancyRequest
    {
        /// <summary>
        /// If not set returns all
        /// If `true` returns non Nation Wide apprenticeship adverts only 
        /// If `false` returns all apprenticeship adverts including Nation Wide
        /// </summary>
        [FromQuery]
        public bool? ExcludeRecruitingNationally { get; set; } = null;
        
        [FromQuery]
        public string EmployerName { get; set; }
    }
}