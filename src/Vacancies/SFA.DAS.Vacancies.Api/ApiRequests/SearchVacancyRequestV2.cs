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

        /// <summary>
        /// If the page size is less than or equal to 100, then with this set to true then
        /// the full text details will be returned in the response 
        /// </summary>
        [FromQuery] 
        public bool IncludeDetails { get; set; } = false;
    }
}